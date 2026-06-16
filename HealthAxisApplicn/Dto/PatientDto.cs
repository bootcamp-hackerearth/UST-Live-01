using System.ComponentModel.DataAnnotations;

namespace HealthAxisApplicn.Dto
{
    public class PatientDto
    {
        public int PatientId { get; set; }
        [Required]
        [RegularExpression(@"[A-Z][A-Za-z\s]+", ErrorMessage = "Name should only contain alphabets")]
        [MinLength(2)]
        public string PatientName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [RegularExpression("(Male|Female|Transgender|Other)")]
        public string Gender { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string PhoneNo { get; set; }
        public string? InsuranceID { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
