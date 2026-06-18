using AutoMapper;
using HealthCare.Api.Data;
using HealthCare.Api.DTOs;
using HealthCare.Api.DTOs.Doctor;
using HealthCare.Api.Exceptions;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Implementations;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;
using System.Linq.Expressions;

namespace HealthCare.Api.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;
        private readonly IAppointmentService _appointmentService;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly HealthCareDbContext _context;
        private readonly IMapper _mapper;

        public DoctorService(IDoctorRepository repository, HealthCareDbContext context, IMapper mapper, IAppointmentService appointmentService, IAppointmentRepository appointmentRepository)
        {
            _repository = repository;
            _context = context;
            _mapper = mapper;
            _appointmentService = appointmentService;
            _appointmentRepository = appointmentRepository;
        }

        public async Task AddAsync(CreateDoctorDto dto)
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
            // Build predicate (filtering)
            Expression<Func<Doctor, bool>>? predicate = null;

            if (!string.IsNullOrWhiteSpace(filter.Specialisation) && filter.MinExperience.HasValue)
            {
                predicate = d => d.Specialisation == filter.Specialisation
                              && d.YearsOfExperience >= filter.MinExperience.Value;
            }
            else if (!string.IsNullOrWhiteSpace(filter.Specialisation))
            {
                predicate = d => d.Specialisation == filter.Specialisation;
            }
            else if (filter.MinExperience.HasValue)
            {
                predicate = d => d.YearsOfExperience >= filter.MinExperience.Value;
            }

            // Ordering (by experience)
            Func<IQueryable<Doctor>, IOrderedQueryable<Doctor>> orderBy =
                q => q.OrderByDescending(d => d.YearsOfExperience);

            // Call repository
            var pagedResult = await _repository.GetAllAsync(
                filter.PageNumber,
                filter.PageSize,
                predicate,
                orderBy
            );

            // Map result
            return new PagedResult<DoctorListDto>
            {
                Items = _mapper.Map<IEnumerable<DoctorListDto>>(pagedResult.Items),
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount
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
                    // Doctor has confirmed/pending appointments that day — cancel them and proceed
                    await _appointmentService.CancelAppointmentsByDoctorDate(id, leave.LeaveDate);
                    result.CreatedWithCancelledAppointments.Add(leave.LeaveDate);
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
