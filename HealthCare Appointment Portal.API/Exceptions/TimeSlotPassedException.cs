using System;

namespace HealthCare_Appointment_Portal.Exceptions
{
    public class TimeSlotPassedException : Exception
    {
        public TimeSlotPassedException()
            : base("The selected time slot has already passed. Please choose a future time slot.")
        {
        }

        public TimeSlotPassedException(string message)
            : base(message)
        {
        }
    }
}