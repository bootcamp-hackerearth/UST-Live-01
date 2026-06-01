using System;

namespace HealthApp.ConsoleApp.Exceptions
{
    public class SpecialisationNotFoundException : Exception
    {
        public SpecialisationNotFoundException(string message) : base(message)
        {
            Console.WriteLine("Specialisation not found: " + message);
        }
    }
}