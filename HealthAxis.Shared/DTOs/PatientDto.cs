using HealthAxis.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTOs
{
    public class PatientDto
    {
        public int PatientId { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(50, ErrorMessage = "Full name cannot exceed 50 characters.")]
        [RegularExpression(
            @"^[A-Za-z ]+$",
            ErrorMessage = "Full name should contain only letters and spaces.")]
        public string FullName { get; set; }

        [Display(Name ="Date Of Birth")]
        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public GenderEnum Gender { get; set; }

        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(
            @"^[0-9]{10}$",
            ErrorMessage = "Phone number must contain exactly 10 digits.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [StringLength(50, ErrorMessage = "Email cannot exceed 50 characters.")]
        public string Email { get; set; }

        [Display(Name ="Insurance ID")]
        [StringLength(10, ErrorMessage = "Insurance ID cannot exceed 10 characters.")]
        public string InsuranceID { get; set; }

        public DateTime CreatedDate { get; set; }

        public int AppointmentCount { get; set; }

        public bool IsActive { get; set; }
    }
}