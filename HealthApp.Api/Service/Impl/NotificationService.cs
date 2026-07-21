using HealthApp.Api.Exceptions;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Service.Interface;
using HealthApp.Shared.Dto;

namespace HealthApp.Api.Service.Impl
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(
            INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<NotificationDto> CreateAsync(NotificationCreateDto dto)
        {
            if (dto == null)
                throw new BusinessRuleException("Notification data is required.");

            if (string.IsNullOrWhiteSpace(dto.UserId))
                throw new BusinessRuleException("Notification user id is required.");

            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new BusinessRuleException("Notification title is required.");

            if (string.IsNullOrWhiteSpace(dto.Message))
                throw new BusinessRuleException("Notification message is required.");

            if (string.IsNullOrWhiteSpace(dto.EventType))
                throw new BusinessRuleException("Notification event type is required.");

            var notification = new Notification
            {
                NotificationId = string.IsNullOrWhiteSpace(dto.NotificationId)
                    ? Guid.NewGuid().ToString()
                    : dto.NotificationId,

                UserId = dto.UserId,
                Title = dto.Title.Trim(),
                Message = dto.Message.Trim(),
                EventType = dto.EventType.Trim(),
                IsRead = false,
                CreatedAt = dto.CreatedAt == default
                    ? DateTime.UtcNow
                    : dto.CreatedAt
            };

            var saved = await _notificationRepository.AddAsync(notification);

            return Map(saved);
        }

        public async Task<List<NotificationDto>> GetMyNotificationsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new BusinessRuleException("Invalid user.");

            var notifications = await _notificationRepository
                .GetByUserIdAsync(userId);

            return notifications.Select(Map).ToList();
        }

        public async Task<List<NotificationDto>> GetMyUnreadNotificationsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new BusinessRuleException("Invalid user.");

            var notifications = await _notificationRepository
                .GetUnreadByUserIdAsync(userId);

            return notifications.Select(Map).ToList();
        }

        public async Task<NotificationDto> MarkAsReadAsync(string notificationId)
        {
            if (string.IsNullOrWhiteSpace(notificationId))
                throw new BusinessRuleException("Invalid notification id.");

            var notification = await _notificationRepository
                .MarkAsReadAsync(notificationId);

            if (notification == null)
                throw new EntityNotFoundException("Notification", 0);

            return Map(notification);
        }

        private static NotificationDto Map(Notification notification)
        {
            return new NotificationDto
            {
                NotificationId = notification.NotificationId,
                UserId = notification.UserId,
                Title = notification.Title,
                Message = notification.Message,
                EventType = notification.EventType,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt
            };
        }

        public async Task<int> CleanupOldReadNotificationsAsync(int olderThanDays)
        {
            if (olderThanDays <= 0)
                throw new BusinessRuleException("Cleanup days must be greater than zero.");

            var cutoffDate = DateTime.UtcNow.AddDays(-olderThanDays);

            return await _notificationRepository
                .DeleteReadNotificationsOlderThanAsync(cutoffDate);
        }
    }
}