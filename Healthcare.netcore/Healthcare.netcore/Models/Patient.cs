using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        public DateTime DOB { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        public bool IsActive { get; set; } = true;
    }
}