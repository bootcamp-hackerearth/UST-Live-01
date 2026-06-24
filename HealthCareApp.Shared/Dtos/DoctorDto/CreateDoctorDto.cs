using System;
using System.ComponentModel.DataAnnotations;
using HealthCareApp.Shared.Enums;

namespace HealthCareApp.Shared.Dtos.Doctors
{
    public class CreateDoctorDto
    {
        [Required(ErrorMessage = "Please enter the doctor's full name.")]
        [StringLength(
            100,
            MinimumLength = 2,
            ErrorMessage = "Doctor name must be between 2 and 100 characters.")]
        [RegularExpression(
            @"^[A-Za-z]+(?: [A-Za-z]+)*$",
            ErrorMessage = "Doctor name can contain only letters and single spaces between words.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter the doctor's email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [StringLength(
            150,
            ErrorMessage = "Email address must not exceed 150 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select the doctor's specialisation.")]
        public SpecialisationType Specialisation { get; set; }

        [Required(ErrorMessage = "Please enter the doctor's practice start date.")]
        [DataType(DataType.Date)]
        public DateTime PracticeStartDate { get; set; }

        [Required(ErrorMessage = "Please enter the consultation fee.")]
        [Range(
            1,
            100000,
            ErrorMessage = "Consultation fee must be between 1 and 100,000.")]
        public decimal ConsultationFee { get; set; }
    }
}