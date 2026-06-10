using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HealthCare.Shared.DTOs.Patient
{
    public class UpdatePatient
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

        public string InsuranceId { get; set; }
    }
}