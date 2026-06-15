using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAxisCore_Api.Models
{
    public class HealthRecord
    {
        [Key]
        public int HealthRecordId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; } = null!;

        public int? DoctorId { get; set; }

        [ForeignKey(nameof(DoctorId))]
        public Doctor? Doctor { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        [ForeignKey(nameof(AppointmentId))]
        public Appointment Appointment { get; set; } = null!;

        [Required]
        public DateTime VisitDate { get; set; }

        [Required]
        [MaxLength(500)]
        public string Diagnosis { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string Prescription { get; set; } = null!;

        [MaxLength(1000)]
        public string? Notes { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}