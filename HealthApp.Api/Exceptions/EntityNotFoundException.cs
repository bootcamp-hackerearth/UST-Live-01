namespace HealthApp.Api.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string entityName, object id)
            : base($"{entityName} with id '{id}' was not found.")
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }
    }
}
