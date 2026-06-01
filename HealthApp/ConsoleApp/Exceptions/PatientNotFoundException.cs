namespace HealthApp.ConsoleApp.Exceptions
{
    public class PatientNotFoundException : Exception
    {
        public PatientNotFoundException(string message) : base(message)
        {
            Console.WriteLine("Patient not found: " + message);
        }
    }
}