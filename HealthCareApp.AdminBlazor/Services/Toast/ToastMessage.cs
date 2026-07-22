namespace HealthCareApp.AdminBlazor.Services.Toast
{
    public class ToastMessage
    {
        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public ToastType Type { get; set; }

        public int DurationInMilliseconds { get; set; } = 3500;
    }
}
