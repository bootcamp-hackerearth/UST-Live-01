namespace HealthApp.AdminPortal.Models
{
    public class ToastNotification
    {
        public string Message { get; set; } = string.Empty;
        public ToastType Type { get; set; } = ToastType.Info;
    }

    public enum ToastType
    {
        Success,
        Error,
        Warning,
        Info
    }
}