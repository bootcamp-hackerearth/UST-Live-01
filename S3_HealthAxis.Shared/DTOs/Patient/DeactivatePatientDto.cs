using System.ComponentModel.DataAnnotations;

namespace S3_HealthAxis.Shared.DTOs.Patient
{
    public class UpdatePatientStatusDto
    {
        [Required]
        public bool IsActive { get; set; }
    }
}