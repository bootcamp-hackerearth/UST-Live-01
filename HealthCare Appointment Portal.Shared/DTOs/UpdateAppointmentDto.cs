using HealthCare_Appointment_Portal.Utilities;
using System;
using System.ComponentModel.DataAnnotations;

namespace HealthCare_Appointment_Portal.DTOs.AppointmentDtos
{
    public class UpdateAppointmentDto
    {
        [Required]
        public int DoctorId
        {
            get;
            set;
        }

        [Required]
        public DateTime ScheduledDate
        {
            get;
            set;
        }

        [Required]
        [StringLength(
            ValidationLimits.TimeSlotLength)]
        public string TimeSlot
        {
            get;
            set;
        }
    }
}