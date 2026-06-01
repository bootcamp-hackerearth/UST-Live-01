namespace HealthApp.ConsoleApp.Exceptions
{
    public class PatientAlreadyExistsException : Exception
    {
        public PatientAlreadyExistsException(string message) : base(message)
        {
            
        }
    }
}