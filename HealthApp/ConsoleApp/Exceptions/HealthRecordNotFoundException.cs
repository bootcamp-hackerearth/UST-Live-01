namespace HealthApp.ConsoleApp.Exceptions
{
    public class HealthRecordNotFoundException : Exception
    {
        public HealthRecordNotFoundException(string message) : base(message)
        {
            Console.WriteLine("Health record not found: " + message);

        }
    }
}