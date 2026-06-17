namespace HealthCareApp.Exceptions
{
    public class AppointmentRuleException : BusinessRuleException
    {
        public AppointmentRuleException(string message)
            : base(message)
        {
        }
    }
}