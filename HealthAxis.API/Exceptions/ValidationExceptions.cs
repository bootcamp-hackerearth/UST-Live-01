namespace HealthAxis.API.Exceptions
{
    public class ValidationExceptions : Exception
    {
        public ValidationExceptions(string message) : base(message)
        {
        }
    }
}