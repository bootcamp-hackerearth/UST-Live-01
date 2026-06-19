namespace HealthApp.Api.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public string EntityName { get; private set; }

        public int EntityId { get; private set; }

        public EntityNotFoundException(string entityName, int entityId)
            : base($"{entityName} with ID {entityId} was not found.")
        {
            EntityName = entityName;
            EntityId = entityId;
        }
    }
}
