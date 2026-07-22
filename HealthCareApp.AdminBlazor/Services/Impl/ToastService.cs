using HealthCareApp.AdminBlazor.Services.Interfaces;
using HealthCareApp.AdminBlazor.Services.Toast;

namespace HealthCareApp.AdminBlazor.Services.Impl
{
    public class ToastService : IToastService
    {
        public event Action<ToastMessage>? OnShow;

        public void ShowSuccess(string title, string message)
        {
            ShowToast(title, message, ToastType.Success);
        }

        public void ShowError(string title, string message)
        {
            ShowToast(title, message, ToastType.Error);
        }

        public void ShowWarning(string title, string message)
        {
            ShowToast(title, message, ToastType.Warning);
        }

        public void ShowInfo(string title, string message)
        {
            ShowToast(title, message, ToastType.Info);
        }

        private void ShowToast(string title, string message, ToastType type)
        {
            var toastMessage = new ToastMessage
            {
                Title = title,

                Message = message,

                Type = type
            };

            OnShow?.Invoke(toastMessage);
        }
    }
}
