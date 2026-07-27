using AutoMapper;
using HealthCare.Api.Data;

using HealthCare.Api.DTOs.Doctor;
using HealthCare.Api.Exceptions;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;
using HealthCare.Shared.DTOs;
using HealthCare.Shared.DTOs.Doctor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Serilog;
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

        public DoctorService(
     IDoctorRepository repository,
     IAppointmentRepository appointmentRepository,
     HealthCareDbContext context,
     IMapper mapper)
        {
            _repository = repository;
            _appointmentRepository = appointmentRepository;
            _context = context;
            _mapper = mapper;
        }



        public async Task<DoctorListDto> GetByIdAsync(int id)
        {
            var doctor = await _repository.GetByIdAsync(id);

            if (doctor is null)
                throw new DoctorNotFoundException(id);

            return _mapper.Map<DoctorListDto>(doctor);
        }

        public async Task<DoctorProfileDto?> GetMyProfileAsync(int doctorId)
        {
            var profile = await (
                from doctor in _context.Doctors
                join user in _context.Users
                    on doctor.UserId equals user.Id
                where doctor.DoctorId == doctorId
                select new DoctorProfileDto
                {
                    DoctorId = doctor.DoctorId,
                    FullName = doctor.FullName,
                    Email = user.Email ?? string.Empty,
                    Specialisation = doctor.Specialisation,
                    YearsOfExperience = doctor.YearsOfExperience,
                    ConsultationFee = doctor.ConsultationFee
                }
            ).FirstOrDefaultAsync();

            return profile;
        }

        public async Task<PagedResult<DoctorListDto>> GetAllAsync(DoctorFilter filter)
        {
            var query = _repository.GetQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(d => d.FullName.Contains(filter.Search));
            }

            if (!string.IsNullOrWhiteSpace(filter.Specialisation))
            {
                query = query.Where(d => d.Specialisation == filter.Specialisation);
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(d => d.IsActive == filter.IsActive.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                query = filter.SortBy.ToLower() switch
                {
                    "experience" => filter.IsDescending
                        ? query.OrderByDescending(d => d.YearsOfExperience)
                        : query.OrderBy(d => d.YearsOfExperience),

                    "fee" => filter.IsDescending
                        ? query.OrderByDescending(d => d.ConsultationFee)
                        : query.OrderBy(d => d.ConsultationFee),

                    _ => query
                };
            }
            else
            {
                query = query.OrderByDescending(d => d.YearsOfExperience);
            }

            var totalCount = await query.CountAsync();

            var items = await query
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
                throw new InvalidOperationException(
                    "Failed to delete Doctor. It may be referenced by existing appointments or health records.",
                    ex);
            }
        }

        public async Task<List<string>> GetSlots(int doctorId)
        {
            var slots = await _repository.GetSlots(doctorId);

            if (slots.Count == 0)
                throw new NoAvailableSlotsException();

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

            if (allSlots == null || allSlots.Count == 0)
                return new List<string>();

            var bookedSlots = await _appointmentRepository.BookedTimeSlots(date, doctorId);

            return allSlots
                .Except(bookedSlots)
                .ToList();
        }

        public async Task<CreateLeaveResultDto> CreateLeave(
    int id,
    List<CreateLeaveDto> leaves)
        {
            var result = new CreateLeaveResultDto();
            var today = DateOnly.FromDateTime(DateTime.Today);

            var existingLeaves =
                await _repository.GetLeavesByDoctorId(id);

            var existingLeaveDates = existingLeaves
                .Select(leave => leave.LeaveDate)
                .ToHashSet();

            var leavesToCreate = new List<CreateLeaveDto>();

            var allSlots = await GetSlots(id);

            foreach (var leave in leaves)
            {
                if (leave.LeaveDate <= today)
                {
                    throw new InvalidOperationException(
                        "Leave can only be created from tomorrow onwards.");
                }

                if (existingLeaveDates.Contains(leave.LeaveDate))
                {
                    result.SkippedDates.Add(leave.LeaveDate);
                    continue;
                }

                var availableSlots = await AvailableTimeSlotsCheck(
                    leave.LeaveDate,
                    id);

                if (availableSlots.Count != allSlots.Count)
                {
                    result.CreatedWithCancelledAppointments.Add(
                        leave.LeaveDate);
                }

                leavesToCreate.Add(leave);

                // Prevent duplicate dates in the same request.
                existingLeaveDates.Add(leave.LeaveDate);
            }

            if (leavesToCreate.Count > 0)
            {
                await _repository.CreateLeaves(
                    id,
                    leavesToCreate);

                await _context.SaveChangesAsync();
            }

            return result;
        }

        public async Task<List<DoctorListDto>> AvailableDoctors(string specialisation, DateOnly date)
        {
            if (date < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new InvalidOperationException("Cannot check availability for a past date.");
            }

            

            var doctors = await _repository.AvailableDoctors(specialisation, date);

            return doctors;
        }
        

       

        public async Task<DoctorSummaryDto> GetSummaryAsync()
        {
            return await _repository.GetSummaryAsync();
        }

        public async Task<DoctorDashboardSummaryDto> GetDashboardSummaryAsync(int doctorId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var upcomingAppointments = await _context.Appointments
                .CountAsync(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate > today &&
                    a.Status != "Cancelled");

            var completedAppointments = await _context.Appointments
                .CountAsync(a =>
                    a.DoctorId == doctorId &&
                    (
                        a.Status == "Completed" ||
                        (a.ScheduledDate < today && a.Status == "Confirmed")
                    ));

            var todaysAppointments = await _context.Appointments
                .CountAsync(a =>
                    a.DoctorId == doctorId &&
                    a.ScheduledDate == today &&
                    a.Status != "Cancelled");

            var leaves = await _repository.GetLeavesByDoctorId(doctorId);

            var upcomingLeaves = leaves.Count(l => l.LeaveDate >= today);

            return new DoctorDashboardSummaryDto
            {
                UpcomingAppointments = upcomingAppointments,
                CompletedAppointments = completedAppointments,
                UpcomingLeaves = upcomingLeaves,
                TodaysAppointments = todaysAppointments
            };
        }
    }
}