namespace HealthCareApp.Exceptions
{
    public class ForbiddenAccessException : HealthcareAppException
    {
        public ForbiddenAccessException(string message)
            : base(message)
        {
        }
    }
}