using HospitalManagementAPI.Model;
using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;

namespace HealthApp.Api.Model
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required]
        [Range(3,200)]
        public string? FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression("^(Male|Female|Other)$", ErrorMessage = "Invalid gender specified.")]
        public string? Gender { get; set; }

        [MaxLength(20)]
        [Required]
        public string? PhoneNumber { get; set; }

        [Required]

        [MaxLength(450)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(100)]
        public string? InsuranceId { get; set; }

        public DateTime? CreatedDate { get; set; }

    }
}