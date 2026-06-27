using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Api.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required]
        [RegularExpression(@"[A-Z][A-za-z\s]+", ErrorMessage = "Name should contain only Alphabets")]
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
        public string PhoneNumber { get; set; }

        public string? InsuranceID { get; set; }
        public bool IsActive { get; set; }
    }
}