using HealthApp.AdminPortal.Models;

namespace HealthApp.AdminPortal.Services.Interface
{

    public interface INotificationService
    {
        event Action<ToastNotification>? OnShow;

        void Success(string message);

        void Error(string message);

        void Warning(string message);

        void Info(string message);
    }

}
