using HealthCareApp.Data;
using HealthCareApp.Exceptions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services.Interface;
using HealthCareApp.Shared.Dtos.DoctorLeaves;
using HealthCareApp.Shared.Dtos.Notifications;
using HealthCareApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApp.Services.Impl
{
    public class DoctorLeaveService : IDoctorLeaveService
    {
        private const string DoctorEntityName = "Doctor";
        private const string DateFormat = "yyyy-MM-dd";

        private const string DoctorLeaveCancellationReason =
            "Doctor is unavailable due to leave. Please rebook another appointment. Sorry for the inconvenience.";

        private readonly IDoctorLeaveRepository doctorLeaveRepository;
        private readonly IDoctorRepository doctorRepository;
        private readonly ICacheService cacheService;
        private readonly HealthAxisDbContext dbContext;
        private readonly ILogger<DoctorLeaveService> logger;

        public DoctorLeaveService(
            IDoctorLeaveRepository doctorLeaveRepository,
            IDoctorRepository doctorRepository,
            ICacheService cacheService,
            HealthAxisDbContext dbContext,
            ILogger<DoctorLeaveService> logger)
        {
            this.doctorLeaveRepository = doctorLeaveRepository;
            this.doctorRepository = doctorRepository;
            this.cacheService = cacheService;
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task<DoctorLeaveDto> CreateMyDoctorLeaveAsync(
            CreateMyDoctorLeaveDto dto,
            string identityUserId)
        {
            if (dto is null)
            {
                throw new BusinessRuleException("Doctor leave details are required.");
            }

            var doctor = await GetLoggedInDoctorAsync(identityUserId);

            ValidateLeaveDates(
                dto.StartDate,
                dto.EndDate);

            var startDate = dto.StartDate.Date;
            var endDate = dto.EndDate.Date;

            var hasOverlappingLeave = await doctorLeaveRepository.HasOverlappingLeaveAsync(
                doctor.DoctorId,
                startDate,
                endDate);

            if (hasOverlappingLeave)
            {
                throw new ConflictException(
                    "You already have leave scheduled during the selected date range.");
            }

            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                var doctorLeave = new DoctorLeave
                {
                    DoctorId = doctor.DoctorId,
                    StartDate = startDate,
                    EndDate = endDate,
                    Reason = dto.Reason.Trim(),
                    CreatedDate = DateTime.Now
                };

                await dbContext.DoctorLeaves.AddAsync(doctorLeave);

                int affectedAppointmentCount =
                    await CancelAffectedAppointmentsAndCreatePatientNotificationsAsync(
                        doctor,
                        startDate,
                        endDate);

                await dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                await TryRemoveAvailabilityCacheForLeaveDatesAsync(
                    doctor.DoctorId,
                    startDate,
                    endDate);

                LogDoctorLeaveCreated(
                    doctor.DoctorId,
                    startDate,
                    endDate,
                    affectedAppointmentCount);

                return MapToDto(
                    doctorLeave,
                    doctor.DoctorName);
            }
            catch
            {
                await transaction.RollbackAsync();

                throw;
            }
        }

        public async Task<List<DoctorLeaveDto>> GetMyDoctorLeavesAsync(
            string identityUserId)
        {
            var doctor = await GetLoggedInDoctorAsync(identityUserId);

            var doctorLeaves = await doctorLeaveRepository.GetLeavesByDoctorIdAsync(
                doctor.DoctorId);

            return doctorLeaves
                .OrderByDescending(doctorLeave => doctorLeave.StartDate)
                .Select(doctorLeave => MapToDto(
                    doctorLeave,
                    doctor.DoctorName))
                .ToList();
        }

        public async Task<List<DoctorLeaveDto>> GetDoctorLeavesByDoctorIdAsync(
            int doctorId)
        {
            ValidateDoctorId(doctorId);

            var doctor = await doctorRepository.GetByIdAsync(doctorId);

            if (doctor is null)
            {
                throw new EntityNotFoundException(
                    DoctorEntityName,
                    doctorId);
            }

            var doctorLeaves = await doctorLeaveRepository.GetLeavesByDoctorIdAsync(
                doctorId);

            return doctorLeaves
                .OrderByDescending(doctorLeave => doctorLeave.StartDate)
                .Select(doctorLeave => MapToDto(
                    doctorLeave,
                    doctor.DoctorName))
                .ToList();
        }

        public async Task<bool> IsDoctorOnLeaveAsync(
            int doctorId,
            DateTime date)
        {
            ValidateDoctorId(doctorId);

            return await doctorLeaveRepository.IsDoctorOnLeaveAsync(
                doctorId,
                date.Date);
        }

        private async Task<int> CancelAffectedAppointmentsAndCreatePatientNotificationsAsync(
            Doctor doctor,
            DateTime leaveStartDate,
            DateTime leaveEndDate)
         {
            var nextDateAfterLeaveEnd = leaveEndDate.Date.AddDays(1);

            var affectedAppointments = await dbContext.Appointments
                .Where(appointment =>
                    appointment.DoctorId == doctor.DoctorId &&
                    appointment.ScheduledDate >= leaveStartDate.Date &&
                    appointment.ScheduledDate < nextDateAfterLeaveEnd &&
                    (
                        appointment.Status == AppointmentStatus.Pending ||
                        appointment.Status == AppointmentStatus.Confirmed
                    ))
                .ToListAsync();

            if (affectedAppointments.Count == 0)
            {
                LogNoAffectedAppointments(
                    doctor.DoctorId,
                    leaveStartDate,
                    leaveEndDate);

                return 0;
            }

            var notifications = new List<Notification>();

            foreach (var appointment in affectedAppointments)
            {
                appointment.Status = AppointmentStatus.Cancelled;
                appointment.CancellationReason = DoctorLeaveCancellationReason;

                string notificationMessage =
                    $"Your appointment with Dr. {doctor.DoctorName} on " +
                    $"{appointment.ScheduledDate:yyyy-MM-dd} at {appointment.TimeSlot} " +
                    "was cancelled because the doctor is unavailable due to leave. " +
                    "Sorry for the inconvenience. Please rebook with another doctor or choose another date.";

                notifications.Add(new Notification
                {
                    PatientId = appointment.PatientId,
                    DoctorId = appointment.DoctorId,
                    AppointmentId = appointment.AppointmentId,
                    Title = "Appointment cancelled - doctor unavailable",
                    Message = notificationMessage,
                    NotificationType = NotificationType.DoctorLeaveRebook,
                    IsRead = false,
                    CreatedDate = DateTime.Now
                });

                LogAppointmentCancelledDueToLeave(appointment);
            }

            await dbContext.Notifications.AddRangeAsync(notifications);

            LogPatientNotificationsPrepared(
                doctor.DoctorId,
                notifications.Count);

            return affectedAppointments.Count;
        }

        private async Task<Doctor> GetLoggedInDoctorAsync(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                throw new BusinessRuleException("Invalid logged-in doctor.");
            }

            var doctor = await doctorRepository.GetByIdentityUserIdAsync(identityUserId);

            if (doctor is null)
            {
                throw new EntityNotFoundException(
                    "Doctor profile for logged-in user",
                    0);
            }

            return doctor;
        }

        private async Task TryRemoveAvailabilityCacheForLeaveDatesAsync(
            int doctorId,
            DateTime startDate,
            DateTime endDate)
        {
            try
            {
                await RemoveAvailabilityCacheForLeaveDatesAsync(
                    doctorId,
                    startDate,
                    endDate);
            }
            catch (Exception ex)
            {
                LogAvailabilityCacheRemovalFailed(
                    ex,
                    doctorId,
                    startDate,
                    endDate);
            }
        }

        private async Task RemoveAvailabilityCacheForLeaveDatesAsync(
            int doctorId,
            DateTime startDate,
            DateTime endDate)
        {
            var currentDate = startDate.Date;

            while (currentDate <= endDate.Date)
            {
                var cacheKey = BuildDoctorAvailabilityCacheKey(
                    doctorId,
                    currentDate);

                await cacheService.RemoveAsync(cacheKey);

                LogAvailabilityCacheRemoved(cacheKey);

                currentDate = currentDate.AddDays(1);
            }
        }

        private void LogDoctorLeaveCreated(
            int doctorId,
            DateTime startDate,
            DateTime endDate,
            int affectedAppointmentCount)
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                "Doctor leave created successfully. DoctorId: {DoctorId}, StartDate: {StartDate}, EndDate: {EndDate}, AffectedAppointments: {AffectedAppointments}",
                doctorId,
                FormatDate(startDate),
                FormatDate(endDate),
                affectedAppointmentCount);
        }

        private void LogNoAffectedAppointments(
            int doctorId,
            DateTime startDate,
            DateTime endDate)
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                "No pending or confirmed appointments affected by doctor leave. DoctorId: {DoctorId}, StartDate: {StartDate}, EndDate: {EndDate}",
                doctorId,
                FormatDate(startDate),
                FormatDate(endDate));
        }

        private void LogAppointmentCancelledDueToLeave(
            Appointment appointment)
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                "Appointment cancelled due to doctor leave. AppointmentId: {AppointmentId}, PatientId: {PatientId}, DoctorId: {DoctorId}, ScheduledDate: {ScheduledDate}, TimeSlot: {TimeSlot}",
                appointment.AppointmentId,
                appointment.PatientId,
                appointment.DoctorId,
                FormatDate(appointment.ScheduledDate),
                appointment.TimeSlot);
        }

        private void LogPatientNotificationsPrepared(
            int doctorId,
            int notificationCount)
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                "Patient notifications prepared for doctor leave. DoctorId: {DoctorId}, NotificationCount: {NotificationCount}",
                doctorId,
                notificationCount);
        }

        private void LogAvailabilityCacheRemovalFailed(
            Exception exception,
            int doctorId,
            DateTime startDate,
            DateTime endDate)
        {
            if (!logger.IsEnabled(LogLevel.Warning))
            {
                return;
            }

            logger.LogWarning(
                exception,
                "Doctor leave was created, but availability cache removal failed. DoctorId: {DoctorId}, StartDate: {StartDate}, EndDate: {EndDate}",
                doctorId,
                FormatDate(startDate),
                FormatDate(endDate));
        }

        private void LogAvailabilityCacheRemoved(string cacheKey)
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                "Doctor availability cache removed due to doctor leave. CacheKey: {CacheKey}",
                cacheKey);
        }

        private static string BuildDoctorAvailabilityCacheKey(
            int doctorId,
            DateTime date)
        {
            return $"doctors:{doctorId}:availability:{FormatDate(date)}";
        }

        private static string FormatDate(DateTime date)
        {
            return date.ToString(DateFormat);
        }

        private static DoctorLeaveDto MapToDto(
            DoctorLeave doctorLeave,
            string doctorName)
        {
            return new DoctorLeaveDto
            {
                DoctorLeaveId = doctorLeave.DoctorLeaveId,
                DoctorId = doctorLeave.DoctorId,
                DoctorName = doctorName,
                StartDate = FormatDate(doctorLeave.StartDate),
                EndDate = FormatDate(doctorLeave.EndDate),
                Reason = doctorLeave.Reason,
                CreatedDate = FormatDate(doctorLeave.CreatedDate)
            };
        }

        private static void ValidateDoctorId(int doctorId)
        {
            if (doctorId <= 0)
            {
                throw new BusinessRuleException("Please provide a valid doctor reference.");
            }
        }

        private static void ValidateLeaveDates(
            DateTime startDate,
            DateTime endDate)
        {
            if (startDate.Date < DateTime.Today)
            {
                throw new BusinessRuleException("Leave start date cannot be in the past.");
            }

            if (endDate.Date < startDate.Date)
            {
                throw new BusinessRuleException("Leave end date cannot be before start date.");
            }
        }
    }
}