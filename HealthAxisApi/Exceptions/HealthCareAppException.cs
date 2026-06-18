namespace HealthAxisCore_Api.Exceptions
{
    public abstract class HealthCareAppException : Exception
    {
        public int StatusCode { get; }

        protected HealthCareAppException(string message, int statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}