using HealthCareApp.Data;
using HealthCareApp.Exceptions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services.Interface;
using HealthCareApp.Shared.Constants;
using HealthCareApp.Shared.Dtos.DoctorLeaves;
using HealthCareApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Serilog.Core;

namespace HealthCareApp.Services.Impl
{
    public class DoctorLeaveService : IDoctorLeaveService
    {
        private const string DoctorEntityName = "Doctor";

        private readonly HealthAxisDbContext _context;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<DoctorLeaveService> _logger;


        public DoctorLeaveService(
            HealthAxisDbContext context,
            IDoctorRepository doctorRepository,
            ICacheService cacheService,
            ILogger<DoctorLeaveService> logger)
        {
            _context = context;
            _doctorRepository = doctorRepository;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<DoctorLeaveDto> CreateMyLeaveAsync(
            string identityUserId,
            CreateDoctorLeaveRequest request)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                throw new BusinessRuleException("Invalid logged-in user.");
            }

            var doctor = await _doctorRepository.GetByIdentityUserIdAsync(identityUserId);

            if (doctor is null)
            {
                throw new EntityNotFoundException("Doctor profile for logged-in user", 0);
            }

            return await CreateLeaveAsync(doctor.DoctorId, request);
        }

        public async Task<List<DoctorLeaveDto>> GetMyLeavesAsync(
            string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                throw new BusinessRuleException("Invalid logged-in user.");
            }

            var doctor = await _doctorRepository.GetByIdentityUserIdAsync(identityUserId);

            if (doctor is null)
            {
                throw new EntityNotFoundException("Doctor profile for logged-in user", 0);
            }

            return await GetDoctorLeavesAsync(doctor.DoctorId);
        }

        public async Task<List<DoctorLeaveDto>> GetDoctorLeavesAsync(
            int doctorId)
        {
            ValidateDoctorId(doctorId);

            var doctorExists = await _context.Doctors
                .AnyAsync(doctor => doctor.DoctorId == doctorId);

            if (!doctorExists)
            {
                throw new EntityNotFoundException(DoctorEntityName, doctorId);
            }

            return await _context.DoctorLeaves
                .AsNoTracking()
                .Include(leave => leave.Doctor)
                .Where(leave => leave.DoctorId == doctorId)
                .OrderByDescending(leave => leave.CreatedDateUtc)
                .ThenByDescending(leave => leave.StartDate)
                .Select(leave => new DoctorLeaveDto
                {
                    DoctorLeaveId = leave.DoctorLeaveId,
                    DoctorId = leave.DoctorId,
                    DoctorName = leave.Doctor.DoctorName,
                    StartDate = leave.StartDate,
                    EndDate = leave.EndDate,
                    Reason = leave.Reason,
                    CreatedDateUtc = leave.CreatedDateUtc
                })
                .ToListAsync();
        }

        public async Task<DoctorLeaveStatusDto> GetDoctorLeaveStatusAsync(
            int doctorId,
            DateOnly date)
        {
            ValidateDoctorId(doctorId);

            var leave = await _context.DoctorLeaves
                .AsNoTracking()
                .Where(item =>
                    item.DoctorId == doctorId &&
                    date >= item.StartDate &&
                    date <= item.EndDate)
                .OrderByDescending(item => item.CreatedDateUtc)
                .FirstOrDefaultAsync();

            if (leave is null)
            {
                return new DoctorLeaveStatusDto
                {
                    DoctorId = doctorId,
                    Date = date,
                    IsDoctorOnLeave = false,
                    Message = string.Empty
                };
            }

            return new DoctorLeaveStatusDto
            {
                DoctorId = doctorId,
                Date = date,
                IsDoctorOnLeave = true,
                Message =
                    $"Doctor is on leave from {leave.StartDate:dd MMM yyyy} to {leave.EndDate:dd MMM yyyy}. Reason: {leave.Reason}"
            };
        }

        public async Task<bool> IsDoctorOnLeaveAsync(
            int doctorId,
            DateOnly date)
        {
            ValidateDoctorId(doctorId);

            return await _context.DoctorLeaves
                .AnyAsync(leave =>
                    leave.DoctorId == doctorId &&
                    date >= leave.StartDate &&
                    date <= leave.EndDate);
        }

        private async Task<DoctorLeaveDto> CreateLeaveAsync(
            int doctorId,
            CreateDoctorLeaveRequest request)
        {
            ValidateDoctorId(doctorId);

            await ValidateLeaveRequestAsync(doctorId, request);

            var doctor = await _context.Doctors
                .FirstOrDefaultAsync(item => item.DoctorId == doctorId);

            if (doctor is null)
            {
                throw new EntityNotFoundException(DoctorEntityName, doctorId);
            }

            var leave = new DoctorLeave
            {
                DoctorId = doctorId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Reason = request.Reason.Trim(),
                CreatedDateUtc = DateTime.UtcNow
            };

            _context.DoctorLeaves.Add(leave);

            await _context.SaveChangesAsync();

            await RemoveAvailabilityCacheForLeaveDatesAsync(
                doctorId,
                leave.StartDate,
                leave.EndDate);

            await CancelAffectedAppointmentsAndNotifyPatientsAsync(doctor, leave.StartDate, leave.EndDate, leave.Reason);

            return new DoctorLeaveDto
            {
                DoctorLeaveId = leave.DoctorLeaveId,
                DoctorId = leave.DoctorId,
                DoctorName = doctor.DoctorName,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                Reason = leave.Reason,
                CreatedDateUtc = leave.CreatedDateUtc
            };
        }

        private async Task ValidateLeaveRequestAsync(
            int doctorId,
            CreateDoctorLeaveRequest request)
        {
            if (request is null)
            {
                throw new BusinessRuleException("Leave details are required.");
            }

            var today = DateOnly.FromDateTime(DateTime.Today);

            if (request.StartDate < today)
            {
                throw new BusinessRuleException("Leave start date cannot be in the past.");
            }

            if (request.EndDate < request.StartDate)
            {
                throw new BusinessRuleException("Leave end date cannot be before start date.");
            }

            if (string.IsNullOrWhiteSpace(request.Reason))
            {
                throw new BusinessRuleException("Leave reason is required.");
            }

            if (request.Reason.Trim().Length > 500)
            {
                throw new BusinessRuleException("Leave reason cannot exceed 500 characters.");
            }

            bool overlapExists = await _context.DoctorLeaves
                .AnyAsync(leave =>
                    leave.DoctorId == doctorId &&
                    request.StartDate <= leave.EndDate &&
                    request.EndDate >= leave.StartDate);

            if (overlapExists)
            {
                throw new BusinessRuleException(
                    "The selected leave range overlaps with an existing leave.");
            }
        }

        private async Task RemoveAvailabilityCacheForLeaveDatesAsync(
    int doctorId,
    DateOnly startDate,
    DateOnly endDate)
        {
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                string cacheKey = $"doctors:{doctorId}:availability:{date:yyyy-MM-dd}";

                try
                {
                    await _cacheService.RemoveAsync(cacheKey);

                    if (_logger.IsEnabled(LogLevel.Information))
                    {
                        _logger.LogInformation(
                            "Removed availability cache for doctor {DoctorId} on {Date}.",
                            doctorId,
                            date);
                    }
                }
                catch (Exception ex)
                {
                    if (_logger.IsEnabled(LogLevel.Warning))
                    {
                        _logger.LogWarning(
                            ex,
                            "Failed to remove availability cache for doctor {DoctorId} on {Date}. Leave creation will continue.",
                            doctorId,
                            date);
                    }
                }
            }
        }

        private static void ValidateDoctorId(int doctorId)
        {
            if (doctorId <= 0)
            {
                throw new BusinessRuleException("Please provide a valid doctor reference.");
            }
        }



        private async Task CancelAffectedAppointmentsAndNotifyPatientsAsync(
     Doctor doctor,
     DateOnly startDate,
     DateOnly endDate,
     string leaveReason)
        {
            if (doctor is null)
            {
                LogDoctorNullWarning();

                return;
            }

            try
            {
                var affectedAppointments = await GetAffectedAppointmentsAsync(
                    doctor.DoctorId,
                    startDate,
                    endDate);

                if (affectedAppointments.Count == 0)
                {
                    LogNoAffectedAppointments(doctor.DoctorId);

                    return;
                }

                var notifications = CancelAppointmentsAndCreateNotifications(
                    affectedAppointments,
                    doctor,
                    startDate,
                    endDate,
                    leaveReason);

                _context.PatientNotifications.AddRange(notifications);

                await _context.SaveChangesAsync();

                LogCancellationSuccess(
                    affectedAppointments.Count,
                    notifications.Count,
                    doctor.DoctorId);
            }
            catch (Exception ex)
            {
                LogCancellationFailure(ex, doctor.DoctorId);
            }
        }


        private async Task<List<Appointment>> GetAffectedAppointmentsAsync(
    int doctorId,
    DateOnly startDate,
    DateOnly endDate)
        {
            var startDateTime = startDate.ToDateTime(TimeOnly.MinValue);

            var endDateTime = endDate.ToDateTime(TimeOnly.MaxValue);

            return await _context.Appointments
                .Where(appointment =>
                    appointment.DoctorId == doctorId &&
                    appointment.ScheduledDate >= startDateTime &&
                    appointment.ScheduledDate <= endDateTime &&
                    (
                        appointment.Status == AppointmentStatus.Pending ||
                        appointment.Status == AppointmentStatus.Confirmed
                    ))
                .ToListAsync();
        }

        private List<PatientNotification> CancelAppointmentsAndCreateNotifications(
            List<Appointment> affectedAppointments,
            Doctor doctor,
            DateOnly startDate,
            DateOnly endDate,
            string leaveReason)
        {
            var doctorName = GetDoctorDisplayName(doctor);

            var notifications = new List<PatientNotification>();

            foreach (var appointment in affectedAppointments)
            {
                appointment.Status = AppointmentStatus.Cancelled;

                appointment.CancellationReason = GetCancellationReason(
                    startDate,
                    endDate,
                    leaveReason);

                notifications.Add(
                    CreateDoctorLeaveNotification(
                        appointment,
                        doctor.DoctorId,
                        doctorName));
            }

            return notifications;
        }

        private static string GetDoctorDisplayName(Doctor doctor)
        {
            return string.IsNullOrWhiteSpace(doctor.DoctorName)
                ? "your doctor"
                : doctor.DoctorName;
        }

        private static string GetCancellationReason(
            DateOnly startDate,
            DateOnly endDate,
            string leaveReason)
        {
            return
                $"Doctor unavailable due to leave from {startDate:dd MMM yyyy} to {endDate:dd MMM yyyy}. Reason: {leaveReason}";
        }

        private static PatientNotification CreateDoctorLeaveNotification(
            Appointment appointment,
            int doctorId,
            string doctorName)
        {
            var formattedDate = appointment.ScheduledDate.ToString("dd MMM yyyy");

            var timeSlot = string.IsNullOrWhiteSpace(appointment.TimeSlot)
                ? "the scheduled time"
                : appointment.TimeSlot;

            return new PatientNotification
            {
                PatientId = appointment.PatientId,
                DoctorId = doctorId,
                AppointmentId = appointment.AppointmentId,
                Title = "Appointment Cancelled - Doctor Unavailable",
                Message =
                    $"Dr. {doctorName} is unavailable on {formattedDate}. Your appointment at {timeSlot} has been cancelled. Please use Rebook Now to select another date or slot.",
                NotificationType = NotificationTypes.DoctorLeave,
                IsRead = false,
                CreatedDateUtc = DateTime.UtcNow
            };
        }

        private void LogDoctorNullWarning()
        {
            if (!_logger.IsEnabled(LogLevel.Warning))
            {
                return;
            }

            _logger.LogWarning(
                "Affected appointment cancellation skipped because doctor object was null.");
        }

        private void LogNoAffectedAppointments(int doctorId)
        {
            if (!_logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "No pending or confirmed appointments found for doctor leave. DoctorId: {DoctorId}",
                doctorId);
        }

        private void LogCancellationSuccess(
            int appointmentCount,
            int notificationCount,
            int doctorId)
        {
            if (!_logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            _logger.LogInformation(
                "Cancelled {AppointmentCount} appointment(s) and created {NotificationCount} patient notification(s) for DoctorId: {DoctorId}",
                appointmentCount,
                notificationCount,
                doctorId);
        }

        private void LogCancellationFailure(
            Exception exception,
            int doctorId)
        {
            if (!_logger.IsEnabled(LogLevel.Warning))
            {
                return;
            }

            _logger.LogWarning(
                exception,
                "Failed to cancel affected appointments or create notifications for doctor leave. DoctorId: {DoctorId}",
                doctorId);
        }
    }
}