using HealthApp.Api.Data;
using HealthApp.Api.Models;
using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Dtos;

namespace HealthApp.Api.Services.Impl
{
    public class NotificationService : INotificationService
    {
        private readonly HealthAppDbContext _context;

        public NotificationService(HealthAppDbContext context)
        {
            _context = context;
        }

        public async Task CreateNotificationAsync(CreateNotificationDto dto)
        {
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

            await _context.Notifications.AddAsync(notification);

            await _context.SaveChangesAsync();
        }
    }
}