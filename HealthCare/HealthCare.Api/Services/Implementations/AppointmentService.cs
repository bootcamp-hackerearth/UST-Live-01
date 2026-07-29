using AutoMapper;
using HealthCare.Api.Data;
using HealthCare.Api.Events;
using HealthCare.Api.Models;
using HealthCare.Api.Repositories.Interfaces;
using HealthCare.Api.Services.Interfaces;
using HealthCare.Shared.DTOs;
using HealthCare.Shared.DTOs.Appointment;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq.Expressions;

namespace HealthCare.Api.Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;
        private readonly IDoctorService _doctorService;
        private readonly HealthCareDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(IAppointmentRepository repository, IDoctorService doctorService, HealthCareDbContext context, IMapper mapper, IPublishEndpoint publishEndpoint, ILogger<AppointmentService> logger)
        {
            _repository = repository;
            _doctorService = doctorService;
            _context = context;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        private const string AppointmentNotFoundMessage = "Appointment not found.";

        private const string PendingStatus = "Pending";
        private const string ConfirmedStatus = "Confirmed";
        private const string CancelledStatus = "Cancelled";
        private const string CompletedStatus = "Completed";

        public async Task<AppointmentListDto?> GetByIdAsync(int id)
        {
            var appointment = await _repository.GetByIdAsync(id);

            if (appointment is null)
                throw new InvalidOperationException(AppointmentNotFoundMessage);

            return _mapper.Map<AppointmentListDto>(appointment);
        }

        public async Task<PagedResult<AppointmentListDto>> GetAllAsync(AppointmentFilter filter)
        {
           
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

            Func<IQueryable<Appointment>, IOrderedQueryable<Appointment>> orderBy =
                q => q.OrderBy(a => a.ScheduledDate);

            var pagedResult = await _repository.GetAllAsync(
                filter.PageNumber,
                filter.PageSize,
                predicate,
                orderBy
            );

            return new PagedResult<AppointmentListDto>
            {
                Items = _mapper.Map<IEnumerable<AppointmentListDto>>(pagedResult.Items),
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount
            };
        }

        public async Task AddAsync(CreateAppointmentDto dto, int patientId)
        {
            if (dto.ScheduledDate < DateOnly.FromDateTime(DateTime.Today))
                throw new InvalidOperationException("Cannot book an appointment for a past date.");

            await IsAvailable(dto.ScheduledDate, dto.DoctorId, dto.TimeSlot);

            var appointment = _mapper.Map<Appointment>(dto);
            appointment.PatientId = patientId;

            try
            {
                await _repository.AddAsync(appointment);
                await _context.SaveChangesAsync();

                if (_logger.IsEnabled(LogLevel.Information))
                { 
                    _logger.LogInformation(
                    "Appointment booked successfully. AppointmentId={AppointmentId}, PatientId={PatientId}, DoctorId={DoctorId}",
                    appointment.AppointmentId,
                    patientId,
                    appointment.DoctorId);
                    }

                var patient = await _context.Patients
                    .FirstAsync(p => p.PatientId == patientId);

                var appointmentEvent = new AppointmentBookedEvent
                {
                    AppointmentId = appointment.AppointmentId,
                    PatientName = patient.FullName,
                    DoctorId = appointment.DoctorId,
                    ScheduledDate = appointment.ScheduledDate,
                    TimeSlot = appointment.TimeSlot
                };

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(
                    "Publishing AppointmentBookedEvent. AppointmentId={AppointmentId}",
                    appointment.AppointmentId);
                }
                await _publishEndpoint.Publish(appointmentEvent);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to book the appointment.", ex);
            }
        }

        public async Task UpdateAsync(int id, UpdateAppointmentDto dto)
        {
            var appointment = await _repository.GetByIdAsync(id);

            if (appointment is null)
                throw new InvalidOperationException(AppointmentNotFoundMessage);

            _mapper.Map(dto, appointment);
            await _repository.UpdateAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(int id, UpdateAppointmentDto dto)
        {
            var appointment = await _repository.GetByIdAsync(id);

            if (appointment is null)
                throw new InvalidOperationException(AppointmentNotFoundMessage);

            // VALID STATUS LIST
            var validStatuses = new[]
             {
                PendingStatus,
                ConfirmedStatus,
                CancelledStatus,
                CompletedStatus
            };

            // CHECK INVALID STATUS
            if (!validStatuses.Contains(dto.Status, StringComparer.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Invalid status value");
            }

            // BUSINESS RULE: Cancellation needs reason
            if (dto.Status == CancelledStatus && string.IsNullOrWhiteSpace(dto.CancellationReason))
            {
                throw new InvalidOperationException("Cancellation reason is required when status is Cancelled");
            }

            // CLEAN ASSIGNMENT
            appointment.Status = dto.Status;
            appointment.CancellationReason =
                 dto.Status == CancelledStatus
                     ? dto.CancellationReason
                     : null;


            await _repository.UpdateAsync(appointment);
            await _context.SaveChangesAsync();

        }


        public async Task DeleteAsync(int id)
        {
            var appointment = await _repository.GetByIdAsync(id);

            if (appointment is null)
                throw new InvalidOperationException(AppointmentNotFoundMessage);
            try
            {
                await _repository.DeleteAsync(id);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("Failed to delete appointment. It may be referenced by existing health records.", ex);
            }
        }

        public async Task<List<string>> AvailableTimeSlots(DateOnly date, int doctorId)
        {
            if (date < DateOnly.FromDateTime(DateTime.Today))
                throw new InvalidOperationException("Cannot check availability for a past date.");

            var allSlots = await _doctorService.GetSlots(doctorId);
            var bookedSlots = await _repository.BookedTimeSlots(date, doctorId);

            var freeSlots = allSlots
     .Where(slot =>
         !bookedSlots.Any(b =>
             b.Trim().StartsWith(
                 slot.Trim().Substring(0, 4),
                 StringComparison.OrdinalIgnoreCase)
         )
     )
     .ToList();


            return freeSlots;
        }

        public async Task<bool> IsAvailable(DateOnly date, int doctorId, string timeSlot)
        {
            var bookedSlots = await _repository.BookedTimeSlots(date, doctorId);

            var exists = bookedSlots.Any(b =>
                 string.Equals(
                     b.Trim(),
                     timeSlot.Trim(),
                     StringComparison.OrdinalIgnoreCase));

            if (exists)
                throw new InvalidOperationException("This time slot is already booked.");

            return true;
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

            return appointments
               .Where(a =>
                a.Status == PendingStatus ||
                a.Status == ConfirmedStatus)
                .OrderBy(a => a.ScheduledDate)  
                .ToList();
        }
        public async Task<List<AppointmentListDto>> GetAppointmentByDoctor(int id)
        {
            
            var appointments = await _repository.GetAppointmentByDoctor(id);

            
            var today = DateOnly.FromDateTime(DateTime.Today);

            
            var result = appointments
                .Where(a =>
                    a.ScheduledDate >= today &&  
                    (
                        a.Status.Equals(PendingStatus, StringComparison.OrdinalIgnoreCase) ||
                        a.Status.Equals(ConfirmedStatus, StringComparison.OrdinalIgnoreCase)
                    )
                )
                .OrderBy(a => a.ScheduledDate)   
                .ThenBy(a => a.TimeSlot)         
                .ToList();

            return result;
        }

        public async Task CancelAppointmentsByDoctorDate(int doctorId, DateOnly date)
        {
            await _repository.CancelAppointmentsByDoctorDate(doctorId, date);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AppointmentReportDto>> GetDailyReport()

        {

            var report = await _repository.GetDailyReport();

            return report.Count == 0 ? new List<AppointmentReportDto>() : report;

        }

        public async Task<List<AppointmentReportDto>> GetReportByDateRange(
    DateOnly startDate,
    DateOnly endDate)
        {
            var report = await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.ScheduledDate >= startDate &&
                            a.ScheduledDate <= endDate)
                .GroupBy(a => a.ScheduledDate)
                .Select(g => new AppointmentReportDto
                {
                    Date = g.Key,

                    PendingCount = g.Count(a => a.Status == PendingStatus),
                    ConfirmedCount = g.Count(a => a.Status == ConfirmedStatus),
                    CancelledCount = g.Count(a => a.Status == CancelledStatus),
                    CompletedCount = g.Count(a => a.Status == CompletedStatus),

                    Revenue = g
                        .Where(a => a.Status == CompletedStatus)
                        .Sum(a => (decimal?)a.Doctor.ConsultationFee) ?? 0
                })
                .OrderBy(r => r.Date)
                .ToListAsync();

            return report;
        }

        public async Task<AppointmentSummaryDto> GetSummaryAsync()
        {
            var fromDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-30));
            var toDate = DateOnly.FromDateTime(DateTime.Today);

            var result = await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.ScheduledDate >= fromDate && a.ScheduledDate <= toDate)
                .GroupBy(a => 1)
                .Select(g => new AppointmentSummaryDto
                {
                    PendingCount = g.Count(a => a.Status == PendingStatus),
                    ConfirmedCount = g.Count(a => a.Status == ConfirmedStatus),
                    CancelledCount = g.Count(a => a.Status == CancelledStatus),
                    CompletedCount = g.Count(a => a.Status == CompletedStatus),

                    TotalRevenue = g
                        .Where(a => a.Status == CompletedStatus)
                        .Sum(a => a.Doctor.ConsultationFee)
                })
                .FirstOrDefaultAsync();

            return result ?? new AppointmentSummaryDto();
        }

    }
}