using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.Dtos
{
    public class UpdateHealthRecordDto
    {
        [Required(ErrorMessage = "Please enter the updated diagnosis details.")]
        [StringLength(500, ErrorMessage = "Diagnosis details must not exceed 500 characters.")]
        public required string Diagnosis { get; set; }

        [Required(ErrorMessage = "Please enter the updated prescription details.")]
        [StringLength(500, ErrorMessage = "Prescription details must not exceed 500 characters.")]
        public required string Prescription { get; set; }

        [StringLength(1000, ErrorMessage = "Additional notes must not exceed 1000 characters.")]
        public required string Notes { get; set; }


        [Required]
        public DateTime VisitDate { get; set; }

    }
}