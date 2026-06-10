using System;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Api.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        public string InsuranceId { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}