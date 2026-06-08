using System.ComponentModel.DataAnnotations;

namespace HealthcareApi.Dtos
{
    public class UpdateHealthRecordDto
    {
        [Required]
        [StringLength(500)]
        public string Diagnosis { get; set; }

        [Required]
        [StringLength(500)]
        public string Prescription { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }
    }
}