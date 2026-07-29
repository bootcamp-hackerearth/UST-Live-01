using AutoMapper;
using HealthCare.Api.Data;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;
using HealthCare.Shared.DTOs;
using HealthCare.Shared.DTOs.Doctor;
using HealthCare.Shared.DTOs.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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
        private const string NotFoundExceptionMessage = "Doctor not found.";

        public DoctorService(IDoctorRepository repository, IAppointmentRepository appointmentRepository, HealthCareDbContext context, IMapper mapper)
        {
            _repository = repository;
            _appointmentRepository = appointmentRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<DoctorListDto?> GetByIdAsync(int id)
        {
            var doctor = await (
                from d in _context.Doctors
                join u in _context.Users
            on d.UserId equals u.Id
        where d.DoctorId == id
 
        select new DoctorListDto
        {
            DoctorId = d.DoctorId,
            FullName = d.FullName,
            Email = u.Email ?? string.Empty,
            Specialisation = d.Specialisation,
            YearsOfExperience = d.YearsOfExperience,
            ConsultationFee = d.ConsultationFee,
            IsActive = d.IsActive
        }
     ).FirstOrDefaultAsync();

            return doctor;
        }

        public async Task<PagedResult<DoctorListDto>> GetAllAsync(DoctorFilter filter)
        {
            Expression<Func<Doctor, bool>> predicate = d =>
                    (string.IsNullOrEmpty(filter.Name) ||
                        (d.FullName != null &&
                         EF.Functions.Like(d.FullName, $"%{filter.Name}%"))) &&

                    (string.IsNullOrEmpty(filter.Specialisation) ||
                        d.Specialisation == filter.Specialisation) &&

                    (!filter.IsActive.HasValue ||
                        d.IsActive == filter.IsActive.Value);

            Func<IQueryable<Doctor>, IOrderedQueryable<Doctor>> orderBy = q =>
            {

                if (!string.IsNullOrEmpty(filter.ExperienceOrder))
                {
                    if (filter.ExperienceOrder == "asc")
                        return q.OrderBy(d => d.YearsOfExperience);

                    return q.OrderByDescending(d => d.YearsOfExperience);
                }

                return q.OrderBy(d => d.DoctorId);
            };

            var pagedResult = await _repository.GetAllAsync(
                filter.PageNumber,
                filter.PageSize,
                predicate,
                orderBy
            );

            return new PagedResult<DoctorListDto>
            {
                Items = _mapper.Map<IEnumerable<DoctorListDto>>(pagedResult.Items),
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount
            };
        }

        public async Task AddAsync(CreateDoctorDto dto)
        {
            var doctor = _mapper.Map<Doctor>(dto);
            await _repository.AddAsync(doctor);

            await _repository.CreateSlots(doctor.DoctorId, dto.TimeSlots);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateDoctorDto dto)
        {
            var doctor = await _repository.GetByIdAsync(id);

            if (doctor is null)
                throw new InvalidOperationException(NotFoundExceptionMessage);

            _mapper.Map(dto, doctor); 

            await _repository.UpdateAsync(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(int id, bool isActive)
        {
            var doctor = await _repository.GetByIdAsync(id);

            if (doctor is null)
                throw new InvalidOperationException(NotFoundExceptionMessage);

            doctor.IsActive = isActive;

            await _repository.UpdateAsync(doctor);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(int id)
        {
            var doctor = await _repository.GetByIdAsync(id);

            if (doctor is null)
                throw new InvalidOperationException(NotFoundExceptionMessage);

            try
            {
                await _repository.DeleteAsync(id);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to delete Doctor. It may be referenced by existing appointments or health records.", ex);
            }
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

        private async Task<List<string>> AvailableTimeSlotsCheck(DateOnly date, int doctorId)
        {
            var allSlots = await _repository.GetSlots(doctorId);
            var bookedSlots = await _appointmentRepository.BookedTimeSlots(date, doctorId);
            return allSlots.Except(bookedSlots).ToList();
        }

        public async Task<CreateLeaveResultDto> CreateLeave(
    int id,
    List<CreateLeaveDto> leaves)
        {
            var result = new CreateLeaveResultDto();

            var existingLeaves = await _repository.GetLeavesByDoctorId(id);

            var existingLeaveDates =
                existingLeaves.Select(l => l.LeaveDate)
                              .ToHashSet();

            var leavesToCreate = new List<CreateLeaveDto>();

            foreach (var leave in leaves)
            {
                await ProcessLeaveRequest(
                    id,
                    leave,
                    existingLeaveDates,
                    leavesToCreate,
                    result);
            }

            return result;
        }

        private async Task ProcessLeaveRequest(
    int doctorId,
    CreateLeaveDto leave,
    HashSet<DateOnly> existingLeaveDates,
    List<CreateLeaveDto> leavesToCreate,
    CreateLeaveResultDto result)
        {
            if (existingLeaveDates.Contains(leave.LeaveDate))
            {
                result.SkippedDates.Add(leave.LeaveDate);
                return;
            }

            var availableSlots =
                await AvailableTimeSlotsCheck(
                    leave.LeaveDate,
                    doctorId);

            var allSlots =
                await GetSlots(doctorId);

            if (availableSlots.Count != allSlots.Count)
            {
                await _appointmentRepository
                    .CancelAppointmentsByDoctorDate(
                        doctorId,
                        leave.LeaveDate);

                result.CreatedWithCancelledAppointments
                      .Add(leave.LeaveDate);
            }

            leavesToCreate.Add(leave);
        }

       
        public async Task<AvailableDoctorsResponseDto> AvailableDoctors(
    string specialisation,
    DateOnly date)
        {

            var allDoctors = await _context.Doctors
                .Where(d => d.Specialisation == specialisation)
                .ToListAsync();

            if (allDoctors.Count == 0)
                return CreateEmptyResponse(
                    "No doctors available for this specialization");

            var activeDoctors = allDoctors
                .Where(d => d.IsActive)
                .ToList();

            if (activeDoctors.Count == 0)
                return CreateEmptyResponse(
                    "Doctor is not active");

            var availableDoctors =
                await GetAvailableDoctors(activeDoctors, date);

            var response = BuildAvailabilityResponse(
                availableDoctors);

            return response;
        }

        private static AvailableDoctorsResponseDto
    CreateEmptyResponse(string message)
        {
            return new AvailableDoctorsResponseDto
            {
                Doctors = new List<DoctorListDto>(),
                Message = message
            };
        }

        private async Task<List<Doctor>>
    GetAvailableDoctors(
        List<Doctor> activeDoctors,
        DateOnly date)
        {
            var availableDoctors =
                new List<Doctor>();

            foreach (var doctor in activeDoctors)
            {
                var isOnLeave =
                    await _context.DoctorLeaves
                        .AnyAsync(l =>
                            l.DoctorId == doctor.DoctorId &&
                            l.LeaveDate == date);

                if (!isOnLeave)
                {
                    availableDoctors.Add(doctor);
                }
            }

            return availableDoctors;
        }

        private AvailableDoctorsResponseDto
    BuildAvailabilityResponse(
        List<Doctor> availableDoctors)
        {
            var result =
                _mapper.Map<List<DoctorListDto>>(
                    availableDoctors);

            return new AvailableDoctorsResponseDto
            {
                Doctors = result,
                Message = result.Count == 0
                    ? "Doctor is on leave"
                    : string.Empty
            };
        }

        public async Task<DoctorSummaryDto> GetSummaryAsync()
        {
            var fromDate = DateTimeOffset.UtcNow.AddDays(-30);
            var toDate = DateTimeOffset.UtcNow;

            var result = await _context.Doctors
                .Where(d => d.CreatedDate >= fromDate && d.CreatedDate <= toDate)
                .GroupBy(d => 1)
                .Select(g => new DoctorSummaryDto
                {
                    TotalDoctors = g.Count(),
                    ActiveDoctors = g.Count(d => d.IsActive),
                    InactiveDoctors = g.Count(d => !d.IsActive)
                })
                .FirstOrDefaultAsync();

            return result ?? new DoctorSummaryDto();
        }

        public async Task<DoctorDashboardDto> GetDashboardAsync(int doctorId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var upcomingAppointments = await _context.Appointments
                .CountAsync(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate >= today &&
                    a.Status != "Cancelled");

            var patientsTreated = await _context.Appointments
                .CountAsync(a =>
                    a.DoctorId == doctorId &&
                    a.Status == "Completed");

            var upcomingLeaves = await _context.DoctorLeaves
                .CountAsync(l =>
                    l.DoctorId == doctorId &&
                    l.LeaveDate >= today);

            var todaysSchedule = await _context.Appointments
                .Where(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate == today &&
                    a.Status != "Cancelled")
                .OrderBy(a => a.TimeSlot)
                .Select(a => a.TimeSlot)
                .ToListAsync();

            return new DoctorDashboardDto
            {
                UpcomingAppointments = upcomingAppointments,
                PatientsTreated = patientsTreated,
                UpcomingLeaves = upcomingLeaves,
                TodaysSchedule = todaysSchedule
            };
        }
    }
}