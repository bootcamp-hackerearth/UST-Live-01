using System;

namespace HealthApp.ConsoleApp.Exceptions
{
    public class DoctorNotFoundException : Exception
    {
        public DoctorNotFoundException(string message) : base(message)
        {
        }
    }
}