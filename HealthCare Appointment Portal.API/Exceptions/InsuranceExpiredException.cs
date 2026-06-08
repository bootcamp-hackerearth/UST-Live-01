using System;

namespace HealthCare_Appointment_Portal.Exceptions
{
    public class
        InsuranceExpiredException
        : Exception
    {
        public InsuranceExpiredException()
            : base(
                "Insurance policy has expired.")
        {
        }
    }
}