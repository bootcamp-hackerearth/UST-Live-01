using System;
using System.ComponentModel.DataAnnotations;
using HealthCareApp.Shared.Enums;
using HealthCareApp.Shared.Dtos.Auth;

namespace HealthCareApp.Shared.Dtos.Doctors
{
    public class CreateDoctorDto
    {
        [Required(ErrorMessage = "Please enter the doctor's full name.")]
        [StringLength(100, ErrorMessage = "Doctor name must not exceed 100 characters.")]
        public  string FullName { get; set; }

        [Required(ErrorMessage = "Please enter the doctor's email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select the doctor's specialisation.")]
        public SpecialisationType Specialisation { get; set; }

        [Required(ErrorMessage = "Please enter the doctor's practice start date.")]
        [DataType(DataType.Date)]
        public DateTime PracticeStartDate { get; set; }

        [Required(ErrorMessage = "Please enter the consultation fee.")]
        [Range(0, 100000, ErrorMessage = "Consultation fee must be between 0 and 100,000.")]
        public decimal ConsultationFee { get; set; }
    }
}