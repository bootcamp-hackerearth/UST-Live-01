using HealthApp.AdminPortal.Models;
using HealthApp.AdminPortal.Services.Interface;

namespace HealthApp.AdminPortal.Services.Impl
{
    public class NotificationService : INotificationService
    {
        public event Action<ToastNotification>? OnShow;

        public void Success(string message)
        {
            Show(message, ToastType.Success);
        }

        public void Error(string message)
        {
            Show(message, ToastType.Error);
        }

        public void Warning(string message)
        {
            Show(message, ToastType.Warning);
        }

        public void Info(string message)
        {
            Show(message, ToastType.Info);
        }

        private void Show(string message, ToastType type)
        {
            OnShow?.Invoke(new ToastNotification
            {
                Message = message,
                Type = type
            });
        }
    }
}