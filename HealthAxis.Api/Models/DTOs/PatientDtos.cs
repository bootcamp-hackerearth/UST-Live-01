using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Api.Models.Dtos
{
    public class PatientDto
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? InsuranceID { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdatePatientDto
    {
        [Required, RegularExpression(@"[A-Z][A-Za-z\s]+"), MinLength(2)]
        public string PatientName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required, RegularExpression("(Male|Female|Transgender|Other)")]
        public string Gender { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }

        public string? InsuranceID { get; set; }
    }
}