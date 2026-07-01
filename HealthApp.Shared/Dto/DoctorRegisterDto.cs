using System.ComponentModel.DataAnnotations;

namespace HealthApp.Shared.Dto
{
    public class DoctorRegisterDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Doctor name is required")]
        [StringLength(200, MinimumLength = 3,
            ErrorMessage = "Doctor name must be between 3 and 200 characters")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialisation is required")]
        public string Specialisation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Practice start date is required")]
        [DataType(DataType.Date)]
        public DateTime? PracticeStartDate { get; set; }

        [Required(ErrorMessage = "Consultation fee is required")]
        [Range(1, 10000000,
            ErrorMessage = "Consultation fee must be greater than 0")]
        public decimal? ConsultationFee { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[0-9]{10}$",
            ErrorMessage = "Phone number must be exactly 10 digits")]
        public string DoctorPhoneNumber { get; set; } = string.Empty;

        public bool? IsActive { get; set; } = true;
    }
}