using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Api.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required]
        [RegularExpression(@"[A-Z][A-Za-z\s]+", ErrorMessage = "Name should contain only Alphabets")]
        [MinLength(2)]
        public required string PatientName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression("(Male|Female|Transgender|Other)")]
        public required string Gender { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

        public string? InsuranceID { get; set; }
        public bool IsActive { get; set; }
    }
}