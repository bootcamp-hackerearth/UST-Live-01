using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Healthaxis2.Models
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PatientId { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only alphabets allowed")]
        public string PatientName { get; set; }

        // ✅ DOB VALIDATION
        [Required]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Patient), nameof(ValidateDOB))]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [RegularExpression(@"^(Male|Female|Transgender|Others)$")]
        public string Gender { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{10}$")]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [RegularExpression(@"^INS[0-9]{4}$")]
        public string InsuranceId { get; set; }

        public DateTime RegisteredDate { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }

        // ✅ CUSTOM VALIDATION METHOD
        public static ValidationResult ValidateDOB(DateTime dob, ValidationContext context)
        {
            if (dob > DateTime.Today)
                return new ValidationResult("Date of Birth cannot be in the future");

            return ValidationResult.Success;
        }
    }
}