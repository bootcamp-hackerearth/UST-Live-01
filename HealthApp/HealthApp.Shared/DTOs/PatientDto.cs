using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HealthApp.Shared.DTOs
{
    public class PatientDto
    {
        public int PatientId { get; set; }


        [Required(ErrorMessage = "Name is required")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Name cannot contain numbers")]

        public string FullName { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }


        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }


        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone must be exactly 10 digits")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email must contain @ and .")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Insurance is required")]
        public string InsuranceId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}