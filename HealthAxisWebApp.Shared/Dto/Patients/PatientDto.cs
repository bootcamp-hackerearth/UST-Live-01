using System.ComponentModel.DataAnnotations;

namespace HealthAxisApplicn.Dto.Patients
{
    public class PatientDto
    {
        public int PatientId { get; set; }
        [Required]
        [RegularExpression(@"[A-Z][A-Za-z\s]+", ErrorMessage = "Name should only contain alphabets")]
        [MinLength(2)]
        public string PatientName { get; set; } = string.Empty;
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [RegularExpression("(Male|Female|Transgender|Other)")]
        public string Gender { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [Phone]
        public string PhoneNo { get; set; } = string.Empty;
        public string? InsuranceID { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }

    public class CreatePatientDto
    {
        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[A-Z][A-Za-z\s]+$")]
        public string PatientName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression("^(Male|Female|Transgender|Other)$")]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string PhoneNo { get; set; } = string.Empty;

        [RegularExpression(@"^$|^INS\d{4}$")]
        public string? InsuranceID { get; set; }
    }
}
