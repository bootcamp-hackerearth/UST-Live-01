using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthApp.Api.Models
{

    [Table("HealthRecords")]
    public class HealthRecord
    {
        [Key]
        public int RecordId { get; set; }

        [Required(ErrorMessage = "Patient is required.")]
        public int PatientId { get; set; }

        public Patient Patient { get; set; } = default!;

        [Required(ErrorMessage = "Doctor is required.")]
        public int DoctorId { get; set; }

        public Doctor Doctor { get; set; } = default!;

        [Required(ErrorMessage = "Visit date is required.")]
        public DateTime VisitDate { get; set; }

        [Required(ErrorMessage = "Diagnosis is required.")]
        [MinLength(3, ErrorMessage = "Diagnosis must be at least 3 characters long.")]
        public string Diagnosis { get; set; } = string.Empty;

        [Required(ErrorMessage = "Prescription is required.")]
        [MinLength(3, ErrorMessage = "Prescription must be at least 3 characters long.")]
        public string Prescription { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

}
