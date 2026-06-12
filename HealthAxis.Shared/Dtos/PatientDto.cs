using System;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.Dtos
{
    public class PatientDto
    {
        public int PatientId { get; set; }

        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;

                if (DateOfBirth > today.AddYears(-age))
                    age--;

                return age;
            }
        }

        public string Gender { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string InsuranceId { get; set; }

        public bool IsActive { get; set; }

        public int UpcomingAppointments { get; set; }
    }

    public class CreatePatientDto
    {
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name should contain only letters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [RegularExpression(@"^$|^INS\d{4}$", ErrorMessage = "Format must be INSXXXX (4 digits)")]
        public string InsuranceId { get; set; }

        public bool IsActive { get; set; }
    }

    public class UpdatePatientDto
    {
        public int PatientId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name should contain only letters")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Phone number must be 10 digits")]
        public string PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        [RegularExpression(@"^$|^INS\d{4}$", ErrorMessage = "Format must be INSXXXX (4 digits)")]
        public string InsuranceId { get; set; }

        public bool IsActive { get; set; }
    }
}
