namespace HealthApp.ConsoleApp.Exceptions
{
    public class AppointmentNotFoundException : Exception
    {
        public AppointmentNotFoundException(string message) : base(message)
        {
            
        }
    }
}