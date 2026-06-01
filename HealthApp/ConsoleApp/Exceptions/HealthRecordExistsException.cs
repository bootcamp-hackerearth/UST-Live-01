namespace HealthApp.ConsoleApp.Exceptions
{
    public class HealthRecordExistsException : Exception
    {
        public HealthRecordExistsException(string message) : base(message)
        {
            
        }
    }
}