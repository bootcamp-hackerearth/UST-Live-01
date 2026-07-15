using AutoMapper;
using Healthcare.Shared.DTOs;
using Healthcare.Shared.DTOs.Appointments;
using Healthcare.Shared.Events;
using HealthCare.Api.Data;
using HealthCare.Api.Exceptions;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Serilog.Core;
using System.Linq.Expressions;
using System.Numerics;

namespace HealthCare.Api.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;
        private readonly IDoctorService _doctorService;
        private readonly IMapper _mapper;
        private readonly HealthCareDbContext _context;
        private readonly ILogger<AppointmentService> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IDistributedCache _cache;
        public AppointmentService(
            IAppointmentRepository repository, 
            IDoctorService doctorService, 
            HealthCareDbContext context, 
            IMapper mapper,
            IPublishEndpoint publishEndpoint,
            ILogger <AppointmentService>logger,
            IDistributedCache cache)
        {
            _repository = repository;
            _doctorService = doctorService;
            _context = context;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
            _cache = cache;
        }

        public async Task AddAsync(CreateAppointmentDto dto, int patientId)
        {
            await IsAvailable(dto.ScheduledDate,dto.DoctorId,dto.TimeSlot);

            var appointment = _mapper.Map<Appointment>(dto);
            var patient = await _context.Patients.FindAsync(patientId);
            var doctor = await _context.Doctors.FindAsync(appointment.DoctorId);

            appointment.PatientId = patientId;

            appointment.Status = "Pending";

            await _repository.AddAsync(appointment);

            await _context.SaveChangesAsync();
            await InvalidateAvailabilityCache(appointment.Doctor.Specialisation, appointment.ScheduledDate);
            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("Appointment created. AppointmentId={AppointmentId}, PatientId={PatientId}, DoctorId={DoctorId}",appointment.AppointmentId,patientId, appointment.DoctorId);

            // Publish AppointmentBookedEvent
            try
            {
                if (patient != null && doctor != null)
                {

                    var appointmentBookedEvent = new AppointmentBookedEvent
                    {
                        AppointmentId = appointment.AppointmentId,
                        PatientId = patientId,
                        DoctorId = appointment.DoctorId,
                        PatientName = patient.FullName,
                        DoctorName = doctor.FullName,
                        ScheduledDate = appointment.ScheduledDate,
                        TimeSlot = appointment.TimeSlot,
                    };

                    await _publishEndpoint.Publish(appointmentBookedEvent);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing AppointmentBookedEvent for AppointmentId {AppointmentId}", appointment.AppointmentId);

            }
        }

        public async Task UpdateAsync(int id, UpdateAppointmentDto dto)
        {
            var appointment = await _repository.GetProfileAsync(id);
            if (appointment == null)
                throw new AppointmentNotFoundException(id);
            _mapper.Map(dto, appointment);

            await _repository.UpdateAsync(appointment);
            await _context.SaveChangesAsync();
            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("Appointment {AppointmentId} updated", appointment.AppointmentId);
        }

        public async Task DeleteAsync(int id)
        {
            var appointment = await _repository.GetProfileAsync(id);

            if (appointment == null)
                throw new AppointmentNotFoundException(id);
            await _repository.DeleteAsync(id);
            await _context.SaveChangesAsync();
            _logger.LogWarning("Appointment {AppointmentId} cancelled",appointment.AppointmentId);
        }

        public async Task<AppointmentListDto?> GetByIdAsync(int id)
        {
            var appointment = await _repository.GetProfileAsync(id);
            return appointment == null ? null : _mapper.Map<AppointmentListDto?>(appointment);
        }
        public async Task<PagedResult<AppointmentListDto>> GetAllAsync(AppointmentFilter filter)
        {
            // Build predicate (filtering)
            Expression<Func<Appointment, bool>>? predicate = null;

            if (!string.IsNullOrWhiteSpace(filter.Status) && filter.ScheduledDate.HasValue)
            {
                predicate = a =>
                    a.Status == filter.Status &&
                    a.ScheduledDate == filter.ScheduledDate.Value;
            }
            else if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                predicate = a => a.Status == filter.Status;
            }
            else if (filter.ScheduledDate.HasValue)
            {
                predicate = a => a.ScheduledDate == filter.ScheduledDate.Value;
            }

            // Ordering (by scheduled date)
            Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>> orderBy =
                q => q.OrderBy(a => a.ScheduledDate);

            // Call repository
            var pagedResult = await _repository.GetAllAsync(
                filter.PageNumber,
                filter.PageSize,
                predicate,
                orderBy
            );

            // Map result
            return new PagedResult<AppointmentListDto>
            {
                Items = _mapper.Map<IEnumerable<AppointmentListDto>>(pagedResult.Items),
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount
            };
        }

        public async Task UpdateStatusAsync(int id, UpdateAppointmentDto dto)
        {
            var appointment = await _repository.GetProfileAsync(id);

            if (appointment is null)
                throw new InvalidOperationException("Patient not found.");

            appointment.Status = dto.Status;
            appointment.CancellationReason = dto.CancellationReason;

            await _repository.UpdateAsync(appointment);
            await _context.SaveChangesAsync();

            var doctor = await _context.Doctors.FindAsync(appointment.DoctorId);
            if (doctor != null)
                await InvalidateAvailabilityCache(appointment.Doctor.Specialisation, appointment.ScheduledDate);
        }
        public async Task<List<string>> AvailableTimeSlots(DateOnly date, int doctorId)
        {
            if (date < DateOnly.FromDateTime(DateTime.Today))
                throw new InvalidOperationException("Cannot check availability for a past date.");

            var allSlots = await _doctorService.GetSlots(doctorId);
            var bookedSlots = await _repository.AvailableTimeSlots(date, doctorId);

            var freeSlots = allSlots.Except(bookedSlots).ToList();

            return freeSlots;
        }

        public async Task<bool> IsAvailable(DateOnly date, int doctorId, string timeSlot)
        {
            var available = await _repository.IsAvailable(date, doctorId, timeSlot);

            if (!available)
                throw new InvalidOperationException("This time slot is already booked.");

            return true;
        }

        public async Task<List<AppointmentReportDto>> GetDailyReport( DateOnly startDate, DateOnly endDate)
        {
            var report = await _repository.GetDailyReport(startDate, endDate);

            return report.Count == 0
                ? new List<AppointmentReportDto>()
                : report;
        }

        public async Task<List<AppointmentListDto>> GetDoctorSchedule(DateOnly date, int id)
        {
            var schedule = await _repository.GetDoctorSchedule(date, id);
            return schedule.Count == 0 ? new List<AppointmentListDto>() : schedule;
        }

        public async Task<List<AppointmentListDto>> GetPatientSchedule(DateOnly date, int id)
        {
            var schedule = await _repository.GetPatientSchedule(date, id);
            return schedule.Count == 0 ? new List<AppointmentListDto>() : schedule;
        }

        public async Task<List<AppointmentListDto>> GetAppointmentByPatient(int id)
        {
            var appointments = await _repository.GetAppointmentByPatient(id);
            return appointments.Count == 0 ? new List<AppointmentListDto>() : appointments;
        }

        public async Task<List<AppointmentListDto>> GetAppointmentByDoctor(int id)
        {
            var appointments = await _repository.GetAppointmentByDoctor(id);
            return appointments.Count == 0 ? new List<AppointmentListDto>() : appointments;
        }

        public async Task CancelAppointmentsByDoctorDate(int doctorId, DateOnly date)
        {
            await _repository.CancelAppointmentsByDoctorDate(doctorId, date);
            await _context.SaveChangesAsync();


        }

        public async Task<AppointmentSummaryDto> GetSummaryAsync()
        {
            var doctors = await _context.Doctors.CountAsync();
            var patients = await _context.Patients.CountAsync();

            var totalAppointments = await _context.Appointments.CountAsync();

            var pending   = await _context.Appointments.CountAsync(a => a.Status == "Pending");
            var confirmed = await _context.Appointments.CountAsync(a => a.Status == "Confirmed");
            var completed = await _context.Appointments.CountAsync(a => a.Status == "Completed");
            var cancelled = await _context.Appointments.CountAsync(a => a.Status == "Cancelled");

            var revenue = await _context.Appointments
                .Where(a => a.Status == "Completed")
                .SumAsync(a => a.Doctor.ConsultationFee);

            return new AppointmentSummaryDto
            {
                TotalDoctors = doctors,
                TotalPatients = patients,
                TotalAppointments = totalAppointments,

                PendingCount = pending,
                ConfirmedCount = confirmed,
                CompletedCount = completed,
                CancelledCount = cancelled,

                TotalRevenue = revenue
            };
        }
        private async Task InvalidateAvailabilityCache(string specialisation,DateOnly date)
        {
            var cacheKey = $"doctor-availability:{specialisation}:{date:yyyy-MM-dd}";
            try
            {
                await _cache.RemoveAsync(cacheKey);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,"Cache invalidation failed for {CacheKey}",cacheKey);
            }
        }
    }
}
