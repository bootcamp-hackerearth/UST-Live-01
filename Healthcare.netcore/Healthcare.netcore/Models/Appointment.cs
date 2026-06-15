using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }

        public int DoctorId { get; set; }
        public int PatientId { get; set; }

        public DateTime ScheduledDate { get; set; }

        [Required]
        public string TimeSlot { get; set; }

        [Required]
        public string Status { get; set; }

        // Navigation Properties
        public Doctor Doctor { get; set; }

        public Patient Patient { get; set; }
    }
}