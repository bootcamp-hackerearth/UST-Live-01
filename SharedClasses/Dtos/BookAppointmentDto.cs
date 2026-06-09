using System;
using System.ComponentModel.DataAnnotations;

namespace SharedClasses.Dtos
{
    public class BookAppointmentDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int PatientId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int DoctorId { get; set; }

        [Required]
        public DateTime ScheduledDate { get; set; }
    }
}