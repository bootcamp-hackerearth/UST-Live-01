namespace HealthAxis.API.Exceptions
{
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message): base(message)
        {
        }
    }
}