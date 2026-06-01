using System;

namespace HealthApp.ConsoleApp.Exceptions
{
    public class DoctorAlreadyExistsException : Exception
    {
        public DoctorAlreadyExistsException(string message) : base(message)
        {
            Console.WriteLine("Doctor already exists: " + message);
        }
    }
}