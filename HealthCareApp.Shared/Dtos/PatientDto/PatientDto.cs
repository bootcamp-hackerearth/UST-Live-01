using HealthCareApp.Shared.Enums;
using System.ComponentModel.DataAnnotations;


namespace HealthCareApp.Shared.Dtos.Patients
{
    public class PatientDto
    {
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Please enter the patient's full name.")]
        [StringLength(100, ErrorMessage = "Patient name must not exceed 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        public string DateOfBirth { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select the patient's gender.")]
        public GenderType Gender { get; set; }

        [Required(ErrorMessage = "Please enter the patient's phone number.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must contain exactly 10 digits.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the patient's email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the patient's insurance ID.")]
        [StringLength(30, ErrorMessage = "Insurance ID must not exceed 30 characters.")]
        public string InsuranceId { get; set; } = string.Empty;

        public string CreatedDate { get; set; } = string.Empty;
    }
}