using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTO.HealthRecordDtos
{
    public class UpdateHealthRecordDto
    {
        [Required(ErrorMessage = "Diagnosis is required")]
        [StringLength(500, ErrorMessage = "Diagnosis cannot exceed 500 characters")]
        public string Diagnosis { get; set; } = string.Empty;

        [Required(ErrorMessage = "Prescription is required")]
        [StringLength(500, ErrorMessage = "Prescription cannot exceed 500 characters")]
        public string Prescription { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Notes cannot exceed 1000 characters")]
        public string Notes { get; set; } = string.Empty;
    }
}