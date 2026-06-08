using System.ComponentModel.DataAnnotations;
using HealthCare_Appointment_Portal.Enums;

namespace HealthCare_Appointment_Portal.Models
{

    public class User
    {

        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(10)]
         public string UserCode { get; set; } = string.Empty;

        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(256)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public Role Role { get; set; }

        [Required]
        public int ReferenceId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}