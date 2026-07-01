using System.ComponentModel.DataAnnotations;

namespace HealthApp.Shared.Dto
{
    public class HealthRecordDto
    {
        public int RecordId { get; set; }

        [Required(ErrorMessage = "Patient is required")]
        public int? PatientId { get; set; }

        [Required(ErrorMessage = "Doctor is required")]
        public int? DoctorId { get; set; }

        [Required(ErrorMessage = "Patient name is required")]
        public string PatientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Doctor name is required")]
        public string DoctorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Visit date is required")]
        [DataType(DataType.Date)]
        public DateTime? VisitDate { get; set; }

        [Required(ErrorMessage = "Diagnosis is required")]
        [StringLength(500,
            ErrorMessage = "Diagnosis cannot exceed 500 characters")]
        public string? Diagnosis { get; set; }

        [Required(ErrorMessage = "Prescription is required")]
        [StringLength(500,
            ErrorMessage = "Prescription cannot exceed 500 characters")]
        public string? Prescription { get; set; }

        [StringLength(1000,
            ErrorMessage = "Notes cannot exceed 1000 characters")]
        public string? Notes { get; set; }
    }
}