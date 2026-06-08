using System;

namespace HealthCare_Appointment_Portal.Exceptions
{
    public class
        DuplicatePolicyNumberException
        : Exception
    {
        public DuplicatePolicyNumberException()
            : base(
                "Policy number already exists.")
        {
        }
    }
}