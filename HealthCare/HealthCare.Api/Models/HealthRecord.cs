using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare.Api.Models
{
    public class HealthRecord
    {
        [Key]
        public int RecordId { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [Required]
        public DateTime VisitDate { get; set; }

        [Required]
        [MaxLength(500)]
        public string? Diagnosis { get; set; }

        [Required]
        [MaxLength(500)]
        public string? Prescription { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; }

        // Navigation
        [ForeignKey("AppointmentId")]
        public virtual Appointment? Appointment { get; set; }
    }
}
