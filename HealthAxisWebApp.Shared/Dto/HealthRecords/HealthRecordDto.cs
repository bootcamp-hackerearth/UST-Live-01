using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthAxisApplicn.Dto.HealthRecords
{
    public class HealthRecordDto
    {
        public int HealthRecordId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime VisitDate { get; set; }

        public string Diagnosis { get; set; } = string.Empty;
        public string Prescription { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
    public class UpdateHealthRecordDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Diagnosis must be at least 3 characters")]
        public string Diagnosis { get; set; } = string.Empty;

        [Required]
        [MinLength(3, ErrorMessage = "Prescription must be at least 3 characters")]
        public string Prescription { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }
    }
}
