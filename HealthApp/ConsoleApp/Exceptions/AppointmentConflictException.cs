namespace HealthApp.ConsoleApp.Exceptions
{
    public class AppointmentConflictException : Exception
    {
        public AppointmentConflictException(string message) : base(message)
        {
            Console.WriteLine("Appointment conflict: " + message);
        }
    }
}