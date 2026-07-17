using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Dtos;

namespace HealthApp.Api.Services.Impl;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(
        INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
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

        await _notificationRepository.AddAsync(notification, ct);
        await _notificationRepository.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<NotificationDto>>
        GetUnreadDoctorLeaveNotificationsAsync(
            string recipientUserId,
            CancellationToken ct = default)
    {
        ValidateRecipientUserId(recipientUserId);

        var notifications = await _notificationRepository
            .GetUnreadDoctorLeaveAsync(recipientUserId, ct);

        return notifications
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
            .ToList();
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

        var notification = await _notificationRepository
            .GetByIdAndRecipientAsync(
                notificationId,
                recipientUserId,
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
        await _notificationRepository.SaveChangesAsync(ct);
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
