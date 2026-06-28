using AutoMapper;
using Healthcare.Shared.DTOs;
using Healthcare.Shared.DTOs.Authentication;
using Healthcare.Shared.DTOs.Doctor;
using HealthCare.Api.Data;
using HealthCare.Api.Exceptions;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;
using System.Linq.Expressions;
using  Microsoft.EntityFrameworkCore;

namespace HealthCare.Api.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly HealthCareDbContext _context;
        private readonly IMapper _mapper;

        public DoctorService(IDoctorRepository repository, HealthCareDbContext context, IMapper mapper, IAppointmentRepository appointmentRepository)
        {
            _repository = repository;
            _context = context;
            _mapper = mapper;
            _appointmentRepository = appointmentRepository;
        }

        public async Task AddAsync(DoctorRegisterDto dto)
        {
            var doctor = _mapper.Map<Doctor>(dto);
            await _repository.AddAsync(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateDoctorDto dto)
        {
            var doctor = await _repository.GetByIdAsync(id);
            if (doctor == null)
                throw new DoctorNotFoundException(id);
            _mapper.Map(dto, doctor);

            await _repository.UpdateAsync(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var doctor = await _repository.GetByIdAsync(id);
            if (doctor == null)
                throw new DoctorNotFoundException(id);
            await _repository.DeleteAsync(id);
            await _context.SaveChangesAsync();
        }

        public async Task<DoctorListDto?> GetByIdAsync(int id)
        {
            var doctor = await _repository.GetByIdAsync(id);
            return doctor == null ? null : _mapper.Map<DoctorListDto?>(doctor);
        }

        public async Task<PagedResult<DoctorListDto>> GetAllAsync(DoctorFilter filter)
        {
            IQueryable<Doctor> query = _context.Doctors.AsQueryable();

            // ✅ Name
            if (!string.IsNullOrWhiteSpace(filter.FullName))
            {
                query = query.Where(d => d.FullName.Contains(filter.FullName));
            }

            // ✅ Specialisation
            if (!string.IsNullOrWhiteSpace(filter.Specialisation))
            {
                query = query.Where(d => d.Specialisation == filter.Specialisation);
            }

            // ✅ Experience
            if (filter.MinExperience.HasValue)
            {
                query = query.Where(d => d.YearsOfExperience >= filter.MinExperience.Value);
            }

            // ✅ Status
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
            var doctor = await _repository.GetByIdAsync(id);

            if (doctor is null)
                throw new InvalidOperationException("Doctor not found.");

            doctor.IsActive = isActive;

            await _repository.UpdateAsync(doctor);
            await _context.SaveChangesAsync();
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
            }

            return result;
        }

        public async Task<List<DoctorListDto>> AvailableDoctors(string specialisation, DateOnly date) =>
            await _repository.AvailableDoctors(specialisation, date);


    }
}
