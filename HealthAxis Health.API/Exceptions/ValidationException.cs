namespace HealthAxisHealth.API.Exceptions
{
    public class ValidationException :
        Exception
    {
        public ValidationException()
            : base("One or more validation errors occurred.")
        {
        }

        public ValidationException(
            string message)
            : base(message)
        {
        }
    }
}