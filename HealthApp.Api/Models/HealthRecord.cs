using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthApp.Api.Models
{

    [Table("HealthRecords")]
    public class HealthRecord
    {
        [Key]
        public int RecordId { get; set; }

        [Required]
        public int? PatientId { get; set; }

        public Patient Patient { get; set; }

        [Required]
        public int? DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int? AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        [Required]
        public DateTime VisitDate { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Diagnosis must be at least 3 characters long.")]
        public string Diagnosis { get; set; } = string.Empty;

        [Required]
        [MinLength(3, ErrorMessage = "Prescription must be at least 3 characters long.")]
        public string Prescription { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

}
