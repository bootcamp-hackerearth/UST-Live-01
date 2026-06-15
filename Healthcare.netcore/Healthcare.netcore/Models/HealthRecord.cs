using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.Models
{
    public class HealthRecord
    {
        public int HealthRecordId { get; set; }

        public int AppointmentId { get; set; }

        [Required]
        public string Diagnosis { get; set; }

        [Required]
        public string Prescription { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Property
        public Appointment Appointment { get; set; }
    }
}