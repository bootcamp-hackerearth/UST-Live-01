using System.ComponentModel.DataAnnotations;

namespace S3_HealthAxisApi.Models
{
    public class HealthRecord
    {
        [Key]
        public int HealthRecordId { get; set; }

        [Required]
        public int AppointmentId { get; set; }

        public Appointment Appointment { get; set; } = null!;

        [Required]
        public int PatientId { get; set; }

        public Patient Patient { get; set; } = null!;

        [Required]
        public int DoctorId { get; set; }

        public Doctor Doctor { get; set; } = null!;

        [Required(ErrorMessage = "Diagnosis is required.")]
        [StringLength(1000)]
        public string Diagnosis { get; set; } = string.Empty;

        [Required(ErrorMessage = "Prescription is required.")]
        [StringLength(2000)]
        public string Prescription { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? Notes { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}