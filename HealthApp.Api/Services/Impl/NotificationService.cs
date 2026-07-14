using HealthApp.Api.Data;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Services.Impl
{
    public class NotificationService : INotificationService
    {
        private readonly HealthAppDbContext _context;

        public NotificationService(HealthAppDbContext context)
        {
            _context = context;
        }

        public async Task CreateNotificationAsync(
            CreateNotificationDto dto,
            CancellationToken ct = default)
        {
            if (dto == null)
            {
                throw new InvalidRequestException(
                    "Notification data is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.RecipientUserId))
            {
                throw new InvalidRequestException(
                    "Notification recipient is required.");
            }

            var notification = new Notification
            {
                RecipientUserId = dto.RecipientUserId,
                NotificationType = dto.NotificationType,
                Title = dto.Title,
                Message = dto.Message,
                RelatedEntityId = dto.RelatedEntityId,
                RelatedEntityType = dto.RelatedEntityType,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Notifications.AddAsync(notification, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task<IReadOnlyList<NotificationDto>>
            GetUnreadDoctorLeaveNotificationsAsync(
                string recipientUserId,
                CancellationToken ct = default)
        {
            ValidateRecipientUserId(recipientUserId);

            return await _context.Notifications
                .AsNoTracking()
                .Where(notification =>
                    notification.RecipientUserId == recipientUserId &&
                    notification.NotificationType == "DoctorLeave" &&
                    !notification.IsRead)
                .OrderBy(notification => notification.CreatedAt)
                .Select(notification => new NotificationDto
                {
                    NotificationId = notification.NotificationId,
                    NotificationType = notification.NotificationType,
                    Title = notification.Title,
                    Message = notification.Message,
                    RelatedEntityId = notification.RelatedEntityId,
                    RelatedEntityType = notification.RelatedEntityType,
                    IsRead = notification.IsRead,
                    CreatedAt = notification.CreatedAt
                })
                .ToListAsync(ct);
        }

        public async Task AcknowledgeNotificationAsync(
            int notificationId,
            string recipientUserId,
            CancellationToken ct = default)
        {
            if (notificationId <= 0)
            {
                throw new InvalidRequestException(
                    "Valid notification id is required.");
            }

            ValidateRecipientUserId(recipientUserId);

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(item =>
                    item.NotificationId == notificationId &&
                    item.RecipientUserId == recipientUserId,
                    ct);

            if (notification == null)
            {
                throw new EntityNotFoundException(
                    "Notification",
                    notificationId);
            }

            if (notification.IsRead)
            {
                return;
            }

            notification.IsRead = true;
            await _context.SaveChangesAsync(ct);
        }

        private static void ValidateRecipientUserId(
            string recipientUserId)
        {
            if (string.IsNullOrWhiteSpace(recipientUserId))
            {
                throw new InvalidRequestException(
                    "Authenticated user id is required.");
            }
        }
    }
}