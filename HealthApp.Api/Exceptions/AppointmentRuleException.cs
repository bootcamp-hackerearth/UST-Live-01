namespace HealthApp.Api.Exceptions
{
    public class AppointmentRuleException : Exception
    {
        public AppointmentRuleException(string message)
            : base(message)
        {
        }
    }
}
