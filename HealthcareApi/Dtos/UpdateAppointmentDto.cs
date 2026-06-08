using System;
using System.ComponentModel.DataAnnotations;

namespace HealthcareApi.Dtos
{
    public class UpdateAppointmentDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int PatientId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int DoctorId { get; set; }

        [Required]
        public DateTime ScheduledDate { get; set; }

        [Required]
        [Range(1, 10)]
        public int SlotNumber { get; set; }
    }
}