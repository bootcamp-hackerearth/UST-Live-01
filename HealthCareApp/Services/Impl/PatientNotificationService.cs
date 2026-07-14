using HealthCareApp.Data;
using HealthCareApp.Exceptions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services.Interface;
using HealthCareApp.Shared.Dtos.PatientNotifications;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApp.Services.Impl
{
    public class PatientNotificationService : IPatientNotificationService
    {
        private const string PatientNotificationEntityName = "Patient Notification";

        private readonly HealthAxisDbContext context;
        private readonly IPatientRepository patientRepository;

        public PatientNotificationService(
            HealthAxisDbContext context,
            IPatientRepository patientRepository)
        {
            this.context = context;
            this.patientRepository = patientRepository;
        }

        public async Task<List<PatientNotificationDto>> GetMyUnreadNotificationsAsync(
            string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                throw new BusinessRuleException("Invalid logged-in user.");
            }

            var patient = await patientRepository.GetByIdentityUserIdAsync(identityUserId);

            if (patient is null)
            {
                throw new EntityNotFoundException("Patient profile for logged-in user", 0);
            }

            return await context.PatientNotifications
                .AsNoTracking()
                .Where(notification =>
                    notification.PatientId == patient.PatientId &&
                    !notification.IsRead)
                .OrderByDescending(notification => notification.CreatedDateUtc)
                .Select(notification => new PatientNotificationDto
                {
                    PatientNotificationId = notification.PatientNotificationId,
                    PatientId = notification.PatientId,
                    DoctorId = notification.DoctorId,
                    AppointmentId = notification.AppointmentId,
                    Title = notification.Title,
                    Message = notification.Message,
                    NotificationType = notification.NotificationType,
                    IsRead = notification.IsRead,
                    CreatedDateUtc = notification.CreatedDateUtc
                })
                .ToListAsync();
        }

        public async Task<PatientNotificationDto> MarkAsReadAsync(
            int patientNotificationId,
            string identityUserId)
        {
            if (patientNotificationId <= 0)
            {
                throw new BusinessRuleException("Please provide a valid notification reference.");
            }

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                throw new BusinessRuleException("Invalid logged-in user.");
            }

            var patient = await patientRepository.GetByIdentityUserIdAsync(identityUserId);

            if (patient is null)
            {
                throw new EntityNotFoundException("Patient profile for logged-in user", 0);
            }

            var notification = await context.PatientNotifications
                .FirstOrDefaultAsync(item =>
                    item.PatientNotificationId == patientNotificationId &&
                    item.PatientId == patient.PatientId);

            if (notification is null)
            {
                throw new EntityNotFoundException(
                    PatientNotificationEntityName,
                    patientNotificationId);
            }

            notification.IsRead = true;

            await context.SaveChangesAsync();

            return new PatientNotificationDto
            {
                PatientNotificationId = notification.PatientNotificationId,
                PatientId = notification.PatientId,
                DoctorId = notification.DoctorId,
                AppointmentId = notification.AppointmentId,
                Title = notification.Title,
                Message = notification.Message,
                NotificationType = notification.NotificationType,
                IsRead = notification.IsRead,
                CreatedDateUtc = notification.CreatedDateUtc
            };
        }

        public async Task CreateNotificationAsync(
            int patientId,
            int? doctorId,
            int? appointmentId,
            string title,
            string message,
            string notificationType)
        {
            if (patientId <= 0)
            {
                throw new BusinessRuleException("Please provide a valid patient reference.");
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new BusinessRuleException("Notification title is required.");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new BusinessRuleException("Notification message is required.");
            }

            if (string.IsNullOrWhiteSpace(notificationType))
            {
                throw new BusinessRuleException("Notification type is required.");
            }

            var notification = new PatientNotification
            {
                PatientId = patientId,
                DoctorId = doctorId,
                AppointmentId = appointmentId,
                Title = title.Trim(),
                Message = message.Trim(),
                NotificationType = notificationType.Trim(),
                IsRead = false,
                CreatedDateUtc = DateTime.UtcNow
            };

            context.PatientNotifications.Add(notification);

            await context.SaveChangesAsync();
        }
    }
}