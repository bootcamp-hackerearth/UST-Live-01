using System.ComponentModel.DataAnnotations;
using S3_HealthAxis.Shared.Enums;
namespace S3_HealthAxisApi.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Patient name is required.")]
        [StringLength(40, ErrorMessage = "Patient name cannot exceed 40 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public InsuranceStatus InsuranceStatus { get; set; }

        [StringLength(50)]
        public string? InsuranceNumber { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public ICollection<HealthRecord> HealthRecords { get; set; } = new List<HealthRecord>();
    }
}