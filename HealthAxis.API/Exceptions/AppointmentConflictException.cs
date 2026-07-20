namespace HealthAxis.API.Exceptions
{
    public sealed class AppointmentConflictException : Exception
    {
        public AppointmentConflictException(string message) : base(message)
        {
        }

        public AppointmentConflictException(string message,Exception innerException): base(message, innerException)
        {
        }
    }
}