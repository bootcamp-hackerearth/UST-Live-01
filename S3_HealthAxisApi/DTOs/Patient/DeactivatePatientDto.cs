using System.ComponentModel.DataAnnotations;

namespace S3_HealthAxisApi.DTOs.Patient
{
    public class UpdatePatientStatusDto
    {
        [Required]
        public bool IsActive { get; set; }
    }
}