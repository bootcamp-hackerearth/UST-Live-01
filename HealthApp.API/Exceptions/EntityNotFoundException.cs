namespace HealthApp.API.Exceptions;

public class EntityNotFoundException : HealthcareAppException
{
    public string EntityName { get; }
    public object EntityId { get; }
    public EntityNotFoundException(string entityName, object entityId) : base($"{entityName} with ID {entityId} was not found.")
    { EntityName = entityName; EntityId = entityId; }
}
