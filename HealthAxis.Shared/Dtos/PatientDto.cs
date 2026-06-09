using System;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.Dtos
{
    public class PatientDto
    {
        public int PatientId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string InsuranceID { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}