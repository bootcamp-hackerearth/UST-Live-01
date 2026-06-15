using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare.Api.Models
{
    public class HealthRecord
    {

        [Key]
        public int RecordId { get; set; }

        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

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

        public DateTimeOffset CreatedDate { get; set; }

        // Navigation
        [ForeignKey("AppointmentId")]
        public Appointment Appointment { get; set; }=null!;

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; } = null!;

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; } =null!;
    }
}
