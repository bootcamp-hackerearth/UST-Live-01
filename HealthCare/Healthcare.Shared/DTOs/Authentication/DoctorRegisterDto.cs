using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Healthcare.Shared.DTOs.Authentication
{
    public class DoctorRegisterDto
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Specialisation { get; set; } = null!;

        [Range(0, 60)]
        public int YearsOfExperience { get; set; }

        [Range(0.01, 100000)]
        public decimal ConsultationFee { get; set; }

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        public string ?ConfirmPassword { get; set; }

        [Required]
        public List<string> ?TimeSlots { get; set; }

    }
}
