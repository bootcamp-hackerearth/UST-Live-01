
using System;
using System.ComponentModel.DataAnnotations;
using SharedClasses.Enums;

namespace SharedClasses.Dtos
{
    public class PatientDto
    {
        public int PatientId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
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

        public DateTime CreatedDate { get; set; }
    }
}