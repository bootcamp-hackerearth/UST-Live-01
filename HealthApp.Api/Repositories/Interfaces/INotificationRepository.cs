using HealthApp.Api.Models;

namespace HealthApp.Api.Repositories.Interfaces;

public interface INotificationRepository
{
    Task AddAsync(
        Notification notification,
        CancellationToken ct = default);

    Task<IReadOnlyList<Notification>> GetUnreadDoctorLeaveAsync(
        string recipientUserId,
        CancellationToken ct = default);

    Task<Notification?> GetByIdAndRecipientAsync(
        int notificationId,
        string recipientUserId,
        CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}