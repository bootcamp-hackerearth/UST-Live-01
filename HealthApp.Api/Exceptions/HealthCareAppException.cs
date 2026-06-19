namespace HealthApp.Api.Exceptions
{
    public class HealthCareAppException :Exception
    {
        protected HealthCareAppException(string message)
            : base(message)
        {
        }

        protected HealthCareAppException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
