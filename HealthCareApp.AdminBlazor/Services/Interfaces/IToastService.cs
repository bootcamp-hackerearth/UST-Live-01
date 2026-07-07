using HealthCareApp.AdminBlazor.Services.Toast;

namespace HealthCareApp.AdminBlazor.Services.Interfaces
{
    public interface IToastService
    {
        event Action<ToastMessage>? OnShow;

        void ShowSuccess(string title, string message);

        void ShowError(string title, string message);

        void ShowWarning(string title, string message);

        void ShowInfo(string title, string message);
    }
}