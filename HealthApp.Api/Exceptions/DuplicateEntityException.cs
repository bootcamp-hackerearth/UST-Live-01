namespace HealthApp.Api.Exceptions
{
    public class DuplicateEntityException : Exception
    {
        public DuplicateEntityException(string message)
            : base(message)
        {
        }
    }
}
