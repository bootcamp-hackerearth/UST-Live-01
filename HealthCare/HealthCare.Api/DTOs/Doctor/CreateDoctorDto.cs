using System.ComponentModel.DataAnnotations;

namespace HealthCare.Api.DTOs.Doctor
{
    public class CreateDoctorDto
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


    }
}
