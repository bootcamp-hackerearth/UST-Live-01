namespace HealthApp.API.Exceptions;

public class PasswordChangeRequiredException : BusinessRuleException
{
    public PasswordChangeRequiredException(string message)
        : base(message)
    {
    }
}