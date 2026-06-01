namespace HealthApp.ConsoleApp.Exceptions
{
    public class AppointmentNotCompletedException : Exception
    {
        public AppointmentNotCompletedException(string message) : base(message)
        {
            
        }
    }
}