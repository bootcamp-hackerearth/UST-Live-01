using System.ComponentModel.DataAnnotations;

namespace HealthAxisApplicn.Dto.Admin
{
    public class CreateDoctorDto
    {
        [Required, StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required, Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string Specialisation { get; set; } = string.Empty;

        [Range(0, 60)]
        public int ExperienceYears { get; set; }

        [Range(0, 100000)]
        public decimal ConsultationFee { get; set; }

        [Required, RegularExpression(@"^([01]\d|2[0-3]):([0-5]\d)$")]
        public string AvailableFrom { get; set; } = "09:00";

        [Required, RegularExpression(@"^([01]\d|2[0-3]):([0-5]\d)$")]
        public string AvailableTo { get; set; } = "17:00";
    }
}
