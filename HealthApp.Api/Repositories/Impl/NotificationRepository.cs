using HealthApp.Api.Data;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Repositories.Impl;

public class NotificationRepository : INotificationRepository
{
    private readonly HealthAppDbContext _context;

    public NotificationRepository(HealthAppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Notification notification,
        CancellationToken ct = default)
    {
        await _context.Notifications.AddAsync(notification, ct);
    }

    public async Task<IReadOnlyList<Notification>>
        GetUnreadDoctorLeaveAsync(
            string recipientUserId,
            CancellationToken ct = default)
    {
        return await _context.Notifications
            .AsNoTracking()
            .Where(notification =>
                notification.RecipientUserId == recipientUserId &&
                notification.NotificationType == "DoctorLeave" &&
                !notification.IsRead)
            .OrderBy(notification => notification.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<Notification?> GetByIdAndRecipientAsync(
        int notificationId,
        string recipientUserId,
        CancellationToken ct = default)
    {
        return await _context.Notifications.FirstOrDefaultAsync(
            notification =>
                notification.NotificationId == notificationId &&
                notification.RecipientUserId == recipientUserId,
            ct);
    }

    public async Task SaveChangesAsync(
        CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}
