using System.ComponentModel.DataAnnotations;

namespace HealthApp.Shared.Dtos
{
    public class HealthRecordCreateDto
    {
        [Required(ErrorMessage = "PatientId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "PatientId must be a valid positive number.")]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "DoctorId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "DoctorId must be a valid positive number.")]
        public int DoctorId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "AppointmentId must be a valid positive number.")]
        public int? AppointmentId { get; set; }

        [Required(ErrorMessage = "Visit date is required.")]
        public DateOnly? VisitDate { get; set; }

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