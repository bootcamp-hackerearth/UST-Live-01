namespace HealthApp.Api.Exceptions
{
    public class ForbiddenAccessException(string message) : Exception(message)
    {}
}
