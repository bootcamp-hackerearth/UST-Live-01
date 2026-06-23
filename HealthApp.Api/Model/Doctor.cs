using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthApp.Api.Model
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Specialisation { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "date")]
        public DateTime PracticeStartDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ConsultationFee { get; set; }

        [Required]
        [Phone(ErrorMessage = "Phone Number should contain ten digits")]
        public string DoctorPhoneNumber { get; set; } = string.Empty;

        public bool? IsActive { get; set; }


        [MaxLength(450)]
        public string? IdentityUserId { get; set; }


    }
}