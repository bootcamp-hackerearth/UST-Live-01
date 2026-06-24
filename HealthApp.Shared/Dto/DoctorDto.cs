using System.ComponentModel.DataAnnotations;

namespace HealthApp.Shared.Dto
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string? FullName { get; set; }

        [Required]
        public string? Specialisation { get; set; }


        [Required]
        public DateTime? PracticeStartDate { get; set; }


        [Required]
        [Range(0, 10000000, ErrorMessage = "Invalid consultation fee specified.")]
        public decimal? ConsultationFee { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [Phone(ErrorMessage = "Phone Number should contain ten digits")]
        public string DoctorPhoneNumber { get; set; } = string.Empty;

        public bool? IsActive { get; set; }


        [MaxLength(450)]
        public string? IdentityUserId { get; set; }

    }
}