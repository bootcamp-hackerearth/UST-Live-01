using System;
using System.ComponentModel.DataAnnotations;
using SharedClasses.Enums;

namespace SharedClasses.Dtos
{
    public class PatientDto
    {
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Please enter the patient's full name.")]
        [StringLength(100, ErrorMessage = "Patient name must not exceed 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter the patient's date of birth.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please select the patient's gender.")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Please enter the patient's phone number.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must contain exactly 10 digits.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter the patient's email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter the patient's insurance ID.")]
        [StringLength(30, ErrorMessage = "Insurance ID must not exceed 30 characters.")]
        public string InsuranceId { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
