namespace HealthApp.API.Exceptions;

public class ConflictException : HealthcareAppException
{
    public ConflictException(string message) : base(message) { }
}
