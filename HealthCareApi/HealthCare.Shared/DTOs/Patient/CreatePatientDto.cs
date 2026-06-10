using System;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Shared.DTOs.Patient
{
    public class CreatePatientDto
    {


        [Required(ErrorMessage = "Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "DOB required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Select gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Phone required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email required")]
        public string Email { get; set; }


        [StringLength(50)]
        public string InsuranceId { get; set; }
    }
}