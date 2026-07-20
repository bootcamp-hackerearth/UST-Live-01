using AutoMapper;
using Healthcare.Shared.DTOs;
using Healthcare.Shared.DTOs.Authentication;
using Healthcare.Shared.DTOs.Doctor;
using HealthCare.Api.Data;
using HealthCare.Api.Exceptions;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;
using  Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace HealthCare.Api.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly HealthCareDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<DoctorService> _logger;
        private readonly IDistributedCache _cache;
        public DoctorService(IDoctorRepository repository, HealthCareDbContext context, IMapper mapper, IAppointmentRepository appointmentRepository,IDistributedCache distributedCache, ILogger<DoctorService> logger)
        {
            _repository = repository;
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _appointmentRepository = appointmentRepository;
            _cache = distributedCache;
        }

        public async Task AddAsync(DoctorRegisterDto dto)
        {
            var doctor = _mapper.Map<Doctor>(dto);
            await _repository.AddAsync(doctor);
            await _context.SaveChangesAsync();
            await InvalidateAvailabilityCache(doctor.Specialisation, DateOnly.FromDateTime(DateTime.Today));
        }

        public async Task UpdateAsync(int id, UpdateDoctorDto dto)
        {
            var doctor = await _repository.GetProfileAsync(id);
            if (doctor == null)
                throw new DoctorNotFoundException("Doctor Not found");
            _mapper.Map(dto, doctor);

            await _repository.UpdateAsync(doctor);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(int id)
        {
            var doctor = await _repository.GetProfileAsync(id);
            if (doctor == null)
                throw new DoctorNotFoundException("Doctor Not Found");
            await _repository.DeleteAsync(id);
            await _context.SaveChangesAsync();
        }

        public async Task<DoctorListDto?> GetByIdAsync(int id)
        {
            var doctor = await _repository.GetProfileAsync(id);
            return doctor == null ? null : _mapper.Map<DoctorListDto?>(doctor);
        }

        public async Task<PagedResult<DoctorListDto>> GetAllAsync(DoctorFilter filter)
        {
            IQueryable<Doctor> query = _context.Doctors.AsQueryable();

            // Name
            if (!string.IsNullOrWhiteSpace(filter.FullName))
            {
                query = query.Where(d => d.FullName.Contains(filter.FullName));
            }

            // Specialisation
            if (!string.IsNullOrWhiteSpace(filter.Specialisation))
            {
                query = query.Where(d => d.Specialisation == filter.Specialisation);
            }

            // Experience
            if (filter.MinExperience.HasValue)
            {
                query = query.Where(d => d.YearsOfExperience >= filter.MinExperience.Value);
            }

            // Status
            if (filter.IsActive.HasValue)
            {
                query = query.Where(d => d.IsActive == filter.IsActive.Value);
            }

            var totalCount = await query.CountAsync(); 

            var items = await query
              .OrderByDescending(d => d.YearsOfExperience)
              .Skip((filter.PageNumber - 1) * filter.PageSize)
              .Take(filter.PageSize)
               .ToListAsync();

            return new PagedResult<DoctorListDto>
            {
                Items = _mapper.Map<IEnumerable<DoctorListDto>>(items),
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }
        public async Task UpdateStatusAsync(int id, bool isActive)
        {
            var doctor = await _repository.GetProfileAsync(id);

            if (doctor is null)
                throw new InvalidOperationException("Doctor not found.");

            doctor.IsActive = isActive;

            await _repository.UpdateAsync(doctor);
            await _context.SaveChangesAsync();
            await InvalidateAvailabilityCache(doctor.Specialisation, DateOnly.FromDateTime(DateTime.Today));
        }

        public async Task<List<string>> GetSlots(int doctorId)
        {
            var slots = await _repository.GetSlots(doctorId);

            if (slots.Count == 0)
                throw new InvalidOperationException("No available slots found for this doctor.");

            return slots;
        }

        public async Task CreateSlots(int id, List<string> timeslots)
        {
            await _repository.CreateSlots(id, timeslots);
            await _context.SaveChangesAsync();

            var doctor= await _repository.GetProfileAsync(id);

            if (doctor is not null)
                await InvalidateAvailabilityCache(doctor.Specialisation, DateOnly.FromDateTime(DateTime.Today));
        }

        public async Task<List<string>> AvailableTimeSlotsCheck(DateOnly date, int doctorId)
        {
            var allSlots = await _repository.GetSlots(doctorId);
            var bookedSlots = await _appointmentRepository.AvailableTimeSlots(date, doctorId);
            return allSlots.Except(bookedSlots).ToList();
        }

        public async Task<CreateLeaveResultDto> CreateLeave(int id, List<CreateLeaveDto> leaves)
        {
            var result = new CreateLeaveResultDto();
            var existingLeaves = await _repository.GetLeavesByDoctorId(id);
            var existingLeaveDates = existingLeaves.Select(l => l.LeaveDate).ToHashSet();

            var leavesToCreate = new List<CreateLeaveDto>();

            foreach (var leave in leaves)
            {
                if (existingLeaveDates.Contains(leave.LeaveDate))
                {
                    result.SkippedDates.Add(leave.LeaveDate);
                    continue;
                }

                var availableSlots = await AvailableTimeSlotsCheck(leave.LeaveDate, id);
                var allSlots = await GetSlots(id);

                if (availableSlots.Count != allSlots.Count)
                {

                    await _appointmentRepository.CancelAppointmentsByDoctorDate(id, leave.LeaveDate);
                    await _context.SaveChangesAsync();
                }
                leavesToCreate.Add(leave);
            }

            if (leavesToCreate.Count > 0)
            {
                await _repository.CreateLeaves(id, leavesToCreate);
                await _context.SaveChangesAsync();

                var doctor = await _repository.GetProfileAsync(id);
                if(doctor != null)

                 foreach (var leave in leavesToCreate)
                    await InvalidateAvailabilityCache(doctor.Specialisation,leave.LeaveDate);
            }

            return result;
        }

        public async Task<List<CreateLeaveDto>> GetLeavesByDoctorIdAsync(int doctorId)
        {
            var leaves = await _repository.GetLeavesByDoctorId(doctorId);

            return leaves
                .OrderBy(l => l.LeaveDate).Select(l => new CreateLeaveDto
                 {
                    LeaveDate = l.LeaveDate,
                    Reason = l.Reason
                 })
                .ToList();
        }
        public async Task<DoctorListDto> GetMyProfileAsync(int doctorId)
        {
            var doctor = await (
                from d in _context.Doctors
                join u in _context.Users
                    on d.UserId equals u.Id

                where d.DoctorId == doctorId

                select new DoctorListDto
                {
                    DoctorId = d.DoctorId,
                    FullName = d.FullName,
                    Specialisation = d.Specialisation,
                    YearsOfExperience = d.YearsOfExperience,
                    ConsultationFee = d.ConsultationFee,
                    IsActive = d.IsActive,
                    Email = u.Email
                }

            ).FirstOrDefaultAsync();

            if (doctor == null)
                throw new DoctorNotFoundException("Doctor not found");

            return doctor;
        }

        public async Task<List<DoctorListDto>> AvailableDoctors(string specialisation, DateOnly date)
        {
            var cachedKey = $"doctor-availability:{specialisation}:{date.ToString("yyyy-MM-dd")}";

            try
            {
                var cachedData = await _cache.GetStringAsync(cachedKey);

                if (!string.IsNullOrEmpty(cachedData))
                {
                    if (_logger.IsEnabled(LogLevel.Information))
                        _logger.LogInformation("CACHE HIT: {CacheKey}", cachedKey);

                    return JsonSerializer.Deserialize<List<DoctorListDto>>(cachedData)!;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache unavailable");
            }
                
            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("CACHE MISS: {CacheKey}", cachedKey);

            var doctors= await _repository.AvailableDoctors(specialisation, date);

            try
            {
                await _cache.SetStringAsync(cachedKey, JsonSerializer.Serialize(doctors), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
                if (_logger.IsEnabled(LogLevel.Information))
                    _logger.LogInformation("CACHE SET: {CacheKey}", cachedKey);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache unavailable");
            }
            return doctors;
        }

        private async Task InvalidateAvailabilityCache(string specialisation, DateOnly date)
        {
            var cacheKey = $"doctor-availability:{specialisation}:{date:yyyy-MM-dd}";
            try
            {
                await _cache.RemoveAsync(cacheKey);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache invalidation failed for {CacheKey}", cacheKey);
            }
        }
    }          
}
