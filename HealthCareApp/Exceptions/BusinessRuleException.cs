namespace HealthCareApp.Exceptions
{
    public class BusinessRuleException : HealthcareAppException
    {
        public BusinessRuleException(string message)
            : base(message)
        {
        }
    }
}