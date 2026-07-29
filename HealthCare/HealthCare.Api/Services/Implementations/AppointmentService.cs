using AutoMapper;
using HealthCare.Api.Data;
using HealthCare.Api.DTOs;
using HealthCare.Api.DTOs.Appointment;
using HealthCare.Api.Events;
using HealthCare.Api.Exceptions;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;
using HealthCare.Shared.DTOs;
using HealthCare.Shared.DTOs.Appointment;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace HealthCare.Api.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;
        private readonly IDoctorService _doctorService;
        private readonly HealthCareDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public AppointmentService(
            IAppointmentRepository repository,
            IDoctorService doctorService,
            HealthCareDbContext context,
            IMapper mapper,
            IPublishEndpoint publishEndpoint)
        {
            _repository = repository;
            _doctorService = doctorService;
            _context = context;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<AppointmentListDto?> GetByIdAsync(int id)
        {
            var appointment = await _repository.GetByIdAsync(id);

            if (appointment is null)
            {
                Log.Warning(
                    "Appointment not found. AppointmentId: {AppointmentId}",
                    id);

                throw new InvalidOperationException(
                    "Appointment not found.");
            }

            return new AppointmentListDto
            {
                AppointmentId = appointment.AppointmentId,
                PatientId = appointment.PatientId,
                PatientName = appointment.Patient.FullName,
                DoctorName = appointment.Doctor.FullName,
                ScheduledDate = appointment.ScheduledDate,
                TimeSlot = appointment.TimeSlot,
                Status = appointment.Status
            };
        }

        public async Task<PagedResult<AppointmentListDto>> GetAllAsync(
            AppointmentFilter filter)
        {
            var query = _repository.GetQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                query = query.Where(a =>
                    a.Patient.FullName.Contains(filter.Search) ||
                    a.Doctor.FullName.Contains(filter.Search));
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                query = query.Where(a =>
                    a.Status == filter.Status);
            }

            query = filter.IsDescending
                ? query.OrderByDescending(a => a.ScheduledDate)
                : query.OrderBy(a => a.ScheduledDate);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(a => new AppointmentListDto
                {
                    AppointmentId = a.AppointmentId,
                    PatientId = a.PatientId,
                    PatientName = a.Patient.FullName,
                    DoctorName = a.Doctor.FullName,
                    ScheduledDate = a.ScheduledDate,
                    TimeSlot = a.TimeSlot,
                    Status = a.Status
                })
                .ToListAsync();

            Log.Information(
                "Appointments fetched successfully. PageNumber: {PageNumber}, PageSize: {PageSize}, TotalCount: {TotalCount}",
                filter.PageNumber,
                filter.PageSize,
                totalCount);

            return new PagedResult<AppointmentListDto>
            {
                Items = items,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task AddAsync(
    CreateAppointmentDto dto,
    int patientId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            if (dto.ScheduledDate <= today)
            {
                Log.Warning(
                    "Appointment booking failed. Appointments can only be booked from tomorrow onwards. PatientId: {PatientId}, DoctorId: {DoctorId}, ScheduledDate: {ScheduledDate}, TimeSlot: {TimeSlot}",
                    patientId,
                    dto.DoctorId,
                    dto.ScheduledDate,
                    dto.TimeSlot);

                throw new InvalidOperationException(
                    "Appointments can only be booked from tomorrow onwards.");
            }

            try
            {
                await IsAvailable(
                    dto.ScheduledDate,
                    dto.DoctorId,
                    dto.TimeSlot);

                var appointment = _mapper.Map<Appointment>(dto);

                appointment.PatientId = patientId;
                appointment.Status = "Pending";
                appointment.CreatedDate = DateTimeOffset.UtcNow;

                await _repository.AddAsync(appointment);
                await _context.SaveChangesAsync();

                Log.Information(
                    "Appointment booked successfully. AppointmentId: {AppointmentId}, PatientId: {PatientId}, DoctorId: {DoctorId}, ScheduledDate: {ScheduledDate}, TimeSlot: {TimeSlot}",
                    appointment.AppointmentId,
                    appointment.PatientId,
                    appointment.DoctorId,
                    appointment.ScheduledDate,
                    appointment.TimeSlot);

                var patient = await _context.Patients
                    .FirstOrDefaultAsync(p => p.PatientId == patientId);

                await _publishEndpoint.Publish(
                    new AppointmentBookedEvent
                    {
                        AppointmentId = appointment.AppointmentId,
                        PatientName = patient?.FullName ?? "Unknown",
                        DoctorId = appointment.DoctorId,
                        ScheduledDate = appointment.ScheduledDate,
                        TimeSlot = appointment.TimeSlot
                    });

                Log.Information(
                    "AppointmentBookedEvent published successfully. AppointmentId: {AppointmentId}, DoctorId: {DoctorId}",
                    appointment.AppointmentId,
                    appointment.DoctorId);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException(
                    $"Failed to book the appointment for patient {patientId} " +
                    $"with doctor {dto.DoctorId} on {dto.ScheduledDate}.",
                    ex);
            }
        }

        public async Task UpdateAsync(
    int id,
    UpdateAppointmentDto dto)
        {
            var appointment = await _repository.GetByIdAsync(id);

            if (appointment is null)
            {
                Log.Warning(
                    "Appointment update failed because the appointment was not found. AppointmentId: {AppointmentId}",
                    id);

                throw new InvalidOperationException(
                    "Appointment not found.");
            }

            try
            {
                _mapper.Map(dto, appointment);

                await _repository.UpdateAsync(appointment);
                await _context.SaveChangesAsync();

                Log.Information(
                    "Appointment updated successfully. AppointmentId: {AppointmentId}",
                    id);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException(
                    $"Failed to update appointment {id}.",
                    ex);
            }
        }

        public async Task UpdateStatusAsync(
      int id,
      UpdateAppointmentDto dto)
        {
            var appointment = await _repository.GetByIdAsync(id);

            if (appointment is null)
            {
                Log.Warning(
                    "Appointment status update failed because the appointment was not found. AppointmentId: {AppointmentId}",
                    id);

                throw new InvalidOperationException(
                    "Appointment not found.");
            }

            var oldStatus = appointment.Status;

            appointment.Status = dto.Status;
            appointment.CancellationReason = dto.CancellationReason;

            try
            {
                await _repository.UpdateAsync(appointment);
                await _context.SaveChangesAsync();

                Log.Information(
                    "Appointment status updated successfully. AppointmentId: {AppointmentId}, OldStatus: {OldStatus}, NewStatus: {NewStatus}",
                    appointment.AppointmentId,
                    oldStatus,
                    appointment.Status);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException(
                    $"Failed to update status for appointment {id}.",
                    ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var appointment = await _repository.GetByIdAsync(id);

            if (appointment is null)
            {
                throw new AppointmentNotFoundException(id);
            }

            try
            {
                await _repository.DeleteAsync(id);
                await _context.SaveChangesAsync();

                Log.Information(
                    "Appointment deleted successfully. AppointmentId: {AppointmentId}",
                    id);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException(
                    $"Failed to delete appointment {id}. It may be referenced by existing health records.",
                    ex);
            }
        }

        public async Task<List<string>> AvailableTimeSlots(
            DateOnly date,
            int doctorId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            if (date <= today)
            {
                Log.Warning(
                    "Availability check failed. Appointments can only be booked from tomorrow onwards. DoctorId: {DoctorId}, Date: {Date}",
                    doctorId,
                    date);

                throw new InvalidOperationException(
                    "Appointments can only be booked from tomorrow onwards.");
            }

            var allSlots =
                await _doctorService.GetSlots(doctorId);

            if (allSlots == null || allSlots.Count == 0)
            {
                Log.Warning(
                    "No time slots configured for doctor. DoctorId: {DoctorId}, Date: {Date}",
                    doctorId,
                    date);

                return new List<string>();
            }

            var bookedSlots =
                await _repository.BookedTimeSlots(
                    date,
                    doctorId);

            var freeSlots = allSlots
                .Except(bookedSlots)
                .ToList();

            Log.Information(
                "Available time slots fetched. DoctorId: {DoctorId}, Date: {Date}, AvailableCount: {AvailableCount}",
                doctorId,
                date,
                freeSlots.Count);

            return freeSlots;
        }

        public async Task<bool> IsAvailable(
            DateOnly date,
            int doctorId,
            string timeSlot)
        {
            var available =
                await _repository.IsAvailable(
                    date,
                    doctorId,
                    timeSlot);

            if (!available)
            {
                Log.Warning(
                    "Time slot already booked. DoctorId: {DoctorId}, Date: {Date}, TimeSlot: {TimeSlot}",
                    doctorId,
                    date,
                    timeSlot);

                throw new InvalidOperationException(
                    "This time slot is already booked.");
            }

            return true;
        }

        public async Task<List<AppointmentReportDto>> GetReport(
            AppointmentReportFilter filter)
        {
            var fromDate = filter.FromDate ??
                DateOnly.FromDateTime(
                    DateTime.Today.AddDays(-7));

            var toDate = filter.ToDate ??
                DateOnly.FromDateTime(DateTime.Today);

            var report =
                await _repository.GetReport(
                    fromDate,
                    toDate);

            Log.Information(
                "Appointment report fetched. FromDate: {FromDate}, ToDate: {ToDate}, Count: {Count}",
                fromDate,
                toDate,
                report.Count);

            return report.Count == 0
                ? new List<AppointmentReportDto>()
                : report;
        }

        public async Task<List<AppointmentListDto>>
            GetDoctorSchedule(DateOnly date, int id)
        {
            var schedule =
                await _repository.GetDoctorSchedule(date, id);

            Log.Information(
                "Doctor schedule fetched. DoctorId: {DoctorId}, Date: {Date}, Count: {Count}",
                id,
                date,
                schedule.Count);

            return schedule.Count == 0
                ? new List<AppointmentListDto>()
                : schedule;
        }

        public async Task<List<AppointmentListDto>>
            GetPatientSchedule(DateOnly date, int id)
        {
            var schedule =
                await _repository.GetPatientSchedule(date, id);

            Log.Information(
                "Patient schedule fetched. PatientId: {PatientId}, Date: {Date}, Count: {Count}",
                id,
                date,
                schedule.Count);

            return schedule.Count == 0
                ? new List<AppointmentListDto>()
                : schedule;
        }

        public async Task<List<AppointmentListDto>>
            GetAppointmentByPatient(int id)
        {
            var appointments =
                await _repository.GetAppointmentByPatient(id);

            Log.Information(
                "Appointments fetched by patient. PatientId: {PatientId}, Count: {Count}",
                id,
                appointments.Count);

            return appointments.Count == 0
                ? new List<AppointmentListDto>()
                : appointments;
        }

        public async Task<List<AppointmentListDto>>
            GetAppointmentByDoctor(int id)
        {
            var appointments =
                await _repository.GetAppointmentByDoctor(id);

            Log.Information(
                "Appointments fetched by doctor. DoctorId: {DoctorId}, Count: {Count}",
                id,
                appointments.Count);

            return appointments.Count == 0
                ? new List<AppointmentListDto>()
                : appointments;
        }

        public async Task CancelAppointmentsByDoctorDate(
    int doctorId,
    DateOnly date)
        {
            try
            {
                await _repository.CancelAppointmentsByDoctorDate(
                    doctorId,
                    date);

                await _context.SaveChangesAsync();

                Log.Information(
                    "Appointments cancelled successfully for doctor date. DoctorId: {DoctorId}, Date: {Date}",
                    doctorId,
                    date);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException(
                    $"Failed to cancel appointments for doctor {doctorId} on {date}.",
                    ex);
            }
        }
        public async Task<AppointmentSummaryDto>
            GetSummaryAsync()
        {
            var summary =
                await _repository.GetSummaryAsync();

            Log.Information(
                "Appointment summary fetched successfully.");

            return summary;
        }

        public async Task<AppointmentSummaryDto>
            GetDashboardSummaryAsync()
        {
            var summary =
                await _repository.GetDashboardSummaryAsync();

            Log.Information(
                "Dashboard appointment summary fetched successfully.");

            return summary;
        }
    }
}