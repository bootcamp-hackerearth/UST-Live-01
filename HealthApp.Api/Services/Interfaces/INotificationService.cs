using HealthApp.Shared.Dtos;

namespace HealthApp.Api.Services.Interfaces
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(CreateNotificationDto dto);
    }
}