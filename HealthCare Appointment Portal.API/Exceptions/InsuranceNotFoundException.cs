using System;

namespace HealthCare_Appointment_Portal.Exceptions
{
    public class
        InsuranceNotFoundException
        : Exception
    {
        public InsuranceNotFoundException()
            : base(
                "Insurance record not found.")
        {
        }
    }
}