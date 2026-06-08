namespace HealthcareApi.Exceptions
{
    public class HealthRecordRuleException : BusinessRuleException
    {
        public HealthRecordRuleException(string message)
            : base(message)
        {
        }
    }
}