using HealthAxis.Shared.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTOs.Patient
{
    public class CreatePatientDto
    {
        [Required(ErrorMessage = "Patient name is required")]
        [MinLength(2, ErrorMessage = "Patient name must be at least 2 characters")]
        [MaxLength(100, ErrorMessage = "Patient name cannot exceed 100 characters")]
        public string PatientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public GenderType Gender { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; } = string.Empty;

        public string? InsuranceID { get; set; }
    }
}