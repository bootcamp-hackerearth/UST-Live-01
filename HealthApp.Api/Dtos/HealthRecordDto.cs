using System.ComponentModel.DataAnnotations;

namespace HealthApp.Api.Dtos
{
    public class HealthRecordDto
    {
        [Required]
        public int RecordId { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Patient name must be at least 3 characters long.")]
        [StringLength(50, ErrorMessage = "Patient name cannot exceed 50 characters.")]
        public string PatientName { get; set; } = string.Empty;

        [Required]
        [MinLength(3, ErrorMessage = "Doctor name must be at least 3 characters long.")]
        [StringLength(50, ErrorMessage = "Doctor name cannot exceed 50 characters.")]
        public string DoctorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Visit date is required.")]
        public DateTime VisitDate { get; set; }

        [Required(ErrorMessage = "Diagnosis is required.")]
        [MinLength(3, ErrorMessage = "Diagnosis must be at least 3 characters long.")]
        public string Diagnosis { get; set; } = string.Empty;

        [Required(ErrorMessage = "Prescription is required.")]
        [MinLength(3, ErrorMessage = "Prescription must be at least 3 characters long.")]
        public string Prescription { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }
    }
}
