using HealthCareApp.Data;
using HealthCareApp.Exceptions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services.Interface;
using HealthCareApp.Shared.Dtos.Notifications;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApp.Services.Impl
{
    public class NotificationService : INotificationService
    {
        private readonly HealthAxisDbContext dbContext;

        private readonly IPatientRepository patientRepository;

        private readonly ILogger<NotificationService> logger;

        public NotificationService(
            HealthAxisDbContext dbContext,
            IPatientRepository patientRepository,
            ILogger<NotificationService> logger)
        {
            this.dbContext = dbContext;
            this.patientRepository = patientRepository;
            this.logger = logger;
        }

        public async Task<List<NotificationDto>> GetMyUnreadNotificationsForPatientAsync(
            string identityUserId)
        {
            var patient = await GetLoggedInPatientAsync(identityUserId);

            var notifications = await dbContext.Notifications
                .AsNoTracking()
                .Where(notification =>
                    notification.PatientId == patient.PatientId &&
                    !notification.IsRead)
                .OrderByDescending(notification => notification.CreatedDate)
                .ToListAsync();

            return notifications
                .Select(MapToDto)
                .ToList();
        }

        public async Task<NotificationDto> MarkNotificationAsReadForPatientAsync(
            int notificationId,
            string identityUserId)
        {
            if (notificationId <= 0)
            {
                throw new BusinessRuleException("Please provide a valid notification reference.");
            }

            var patient = await GetLoggedInPatientAsync(identityUserId);

            var notification = await dbContext.Notifications
                .FirstOrDefaultAsync(existingNotification =>
                    existingNotification.NotificationId == notificationId);

            if (notification is null)
            {
                throw new EntityNotFoundException("Notification", notificationId);
            }

            if (notification.PatientId != patient.PatientId)
            {
                throw new ForbiddenAccessException(
                    "Patients can update only their own notifications.");
            }

            notification.IsRead = true;

            await dbContext.SaveChangesAsync();

            logger.LogInformation(
                "Patient notification marked as read. NotificationId: {NotificationId}, PatientId: {PatientId}",
                notification.NotificationId,
                patient.PatientId);

            return MapToDto(notification);
        }

        private async Task<Patient> GetLoggedInPatientAsync(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                throw new BusinessRuleException("Invalid logged-in patient.");
            }

            var patient = await patientRepository.GetByIdentityUserIdAsync(identityUserId);

            if (patient is null)
            {
                throw new EntityNotFoundException(
                    "Patient profile for logged-in user",
                    0);
            }

            return patient;
        }

        private static NotificationDto MapToDto(Notification notification)
        {
            return new NotificationDto
            {
                NotificationId = notification.NotificationId,
                PatientId = notification.PatientId,
                DoctorId = notification.DoctorId,
                AppointmentId = notification.AppointmentId,
                Title = notification.Title,
                Message = notification.Message,
                NotificationType = notification.NotificationType.ToString(),
                IsRead = notification.IsRead,
                CreatedDate = notification.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }
    }
}