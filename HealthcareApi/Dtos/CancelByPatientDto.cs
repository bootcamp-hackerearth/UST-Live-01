using System.ComponentModel.DataAnnotations;

namespace HealthcareApi.Dtos
{
    public class CancelByPatientDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int PatientId { get; set; }

        [Required]
        [StringLength(500)]
        public string Reason { get; set; }
    }
}