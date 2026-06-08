using System.ComponentModel.DataAnnotations;

namespace HealthCare_Appointment_Portal.DTOs.AppointmentDtos
{
    public class UpdateAppointmentStatusDto
    {
        [Required]
        public string Status
        {
            get;
            set;
        }

        public string CancellationReason
        {
            get;
            set;
        }
    }
}