
using System;
using System.ComponentModel.DataAnnotations;
using SharedClasses.Enums;

namespace SharedClasses.Dtos
{
    public class UpdatePatientDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31",
    ErrorMessage = "Date of birth cannot be before 01 Jan 1900.")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(30)]
        public string InsuranceId { get; set; }
    }
}