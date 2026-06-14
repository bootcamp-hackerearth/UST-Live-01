using System.ComponentModel.DataAnnotations;

namespace HealthApp.API.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        [Required]
        [RegularExpression(@"[A-Z][a-zA-Z\s]{2,}", ErrorMessage = "Name must start with a capital letter and contain only letters")]
        public string PatientName { get; set; }
        
        [Required]
        public DateTime DateOfBirth { get; set; }
        
        [Required]
        [RegularExpression("(Male|Female|Other)", ErrorMessage = "Invalid gender")]
        public string Gender { get; set; }
        
        [EmailAddress]
        public string? Email { get; set; }
       
        [Required]
        [RegularExpression(@"[6-9]\d{9}", ErrorMessage = "Phone number must be 10 digits")]
        public string PhoneNumber { get; set; }
        public string? InsuranceId { get; set; }
    }
}
