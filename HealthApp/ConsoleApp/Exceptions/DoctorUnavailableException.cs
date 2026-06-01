namespace HealthApp.ConsoleApp.Exceptions
{
    public class DoctorUnavailableException : Exception
    {
        public DoctorUnavailableException(string message) : base(message)
        {
            Console.WriteLine("Doctor unavailable: " + message);
        }
    }
    
}