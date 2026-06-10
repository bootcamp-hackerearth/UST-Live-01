using System;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.Dtos
{
    public class BookAppointmentDto
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public Specialisation Specialisation { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime ScheduledDate { get; set; }

        [Required]
        public string TimeSlot { get; set; }
    }
}