using System.ComponentModel.DataAnnotations;

namespace HealthApp.Shared.Dto
{
    public class PatientUpdateDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[0-9]{10}$",
            ErrorMessage = "Phone number must be exactly 10 digits")]
        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }

        public string? InsuranceId { get; set; }
    }
}