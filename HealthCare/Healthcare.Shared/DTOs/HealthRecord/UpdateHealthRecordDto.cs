using System.ComponentModel.DataAnnotations;

namespace Healthcare.Shared.DTOs.HealthRecord
{
    public class UpdateHealthRecordDto
    {
        [Required]
        [MaxLength(500)]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Diagnosis must contain only letters and numbers.")]
        public string Diagnosis { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Prescription must contain only letters and numbers.")]
        public string Prescription { get; set; } = null!;

        [MaxLength(1000)]
        [RegularExpression(@"^[A-Za-z0-9\s]+$", ErrorMessage = "Notes must contain only letters and numbers.")]
        public string? Notes { get; set; }
    }
}
