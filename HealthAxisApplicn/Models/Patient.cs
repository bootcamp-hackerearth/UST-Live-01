using System.ComponentModel.DataAnnotations;

namespace HealthAxisApplicn.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z\s]{2,50}$", ErrorMessage = "Name should contain only alphabets and spaces")]
        [MinLength(2)]
        public string PatientName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [RegularExpression(@"^(Male|Female|Transgender|Other)$", ErrorMessage = "Invalid gender")]
        public string Gender { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^\+\d{10,15}$", ErrorMessage = "Enter valid number with country code (e.g. +919876543210)")]
        public string PhoneNo { get; set; }
        
        [RegularExpression(@"^$|^INS\d{4}$")]
        public string? InsuranceID { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
