using System;
using System.ComponentModel.DataAnnotations;
using SharedClasses.Enums;

namespace SharedClasses.Dtos
{
    public class CreateDoctorDto
    {
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        [RegularExpression(@"^[A-Za-z .]+$", ErrorMessage = "Full name can contain only letters, spaces, and dots.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Specialisation is required.")]
        [Display(Name = "Specialisation")]
        public Specialisation Specialisation { get; set; }

        [Required(ErrorMessage = "Practice start date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Practice Start Date")]
        public DateTime PracticeStartDate { get; set; }

        [Required(ErrorMessage = "Consultation fee is required.")]
        [Range(0, 100000, ErrorMessage = "Consultation fee must be between 0 and 100000.")]
        [Display(Name = "Consultation Fee")]
        public decimal ConsultationFee { get; set; }
    }
}