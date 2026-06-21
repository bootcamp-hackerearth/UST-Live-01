using System;
using System.ComponentModel.DataAnnotations;
using HealthCareApp.Enums;

namespace HealthCareApp.Dtos
{
    public class UpdateDoctorDto
    {
        [Required(ErrorMessage = "Please enter the doctor's full name.")]
        [StringLength(100, ErrorMessage = "Doctor name must not exceed 100 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please select the doctor's specialisation.")]
        public SpecialisationType Specialisation { get; set; }

        [Required(ErrorMessage = "Please enter the doctor's practice start date.")]
        [DataType(DataType.Date)]
        public DateTime PracticeStartDate { get; set; }

        [Required(ErrorMessage = "Please enter the consultation fee.")]
        [Range(0, 100000, ErrorMessage = "Consultation fee must be between 0 and 100,000.")]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}