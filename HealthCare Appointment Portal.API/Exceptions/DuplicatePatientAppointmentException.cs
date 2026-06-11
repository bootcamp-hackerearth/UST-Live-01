using System;

namespace HealthCare_Appointment_Portal.Exceptions
{
    public class DuplicatePatientAppointmentException
        : Exception
    {
        public DuplicatePatientAppointmentException()
            : base(
                "Patient already has an appointment scheduled for this date.")
        {
        }
    }
}