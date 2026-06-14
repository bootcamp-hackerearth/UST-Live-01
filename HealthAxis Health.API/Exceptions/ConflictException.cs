namespace HealthAxisHealth.API.Exceptions
{
    public class ConflictException :
        Exception
    {
        public ConflictException()
            : base("A conflict occurred while processing the request.")
        {
        }

        public ConflictException(
            string message)
            : base(message)
        {
        }
    }
}