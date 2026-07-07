namespace HealthCareApp.Exceptions
{
    public abstract class HealthcareAppException : Exception
    {
        protected HealthcareAppException(string message)
            : base(message)
        {
        }

        protected HealthcareAppException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}