using HealthCareApp.Shared.Enums;
using System.ComponentModel.DataAnnotations;


namespace HealthCareApp.Shared.Dtos.Patients
{
    public class PatientRegisterDto
    {
        [Required(ErrorMessage = "Please enter the patient's full name.")]
        [StringLength(100, ErrorMessage = "Patient name must not exceed 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the patient's date of birth.")]
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31",
            ErrorMessage = "Date of birth must be on or after 01 Jan 1900.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please select the patient's gender.")]
        public GenderType Gender { get; set; }

        [Required(ErrorMessage = "Please enter the patient's phone number.")]
        [StringLength(10, ErrorMessage = "Phone number must not exceed 10 characters.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the patient's email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the patient's insurance ID.")]
        [StringLength(30, ErrorMessage = "Insurance ID must not exceed 30 characters.")]
        public string InsuranceId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a password.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm the password.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}