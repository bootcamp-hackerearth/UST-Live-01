namespace HealthApp.AdminPortal.Services
{
    public class AuthRedirectState
    {
        public string? Message { get; set; }
        public void SetMessage(string message)
        {
            Message = message;
        }
        public string? ConsumeMessage()
        {
            var message = Message;
            Message = null;
            return message;
        }
    }
}