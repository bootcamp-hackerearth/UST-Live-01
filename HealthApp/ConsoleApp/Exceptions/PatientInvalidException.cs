using System;

namespace HealthApp.ConsoleApp.Exceptions
{
    public class PatientInvalidException : Exception
    {
        public PatientInvalidException() : base("Patient data is invalid: " )
        {} 
    }
}