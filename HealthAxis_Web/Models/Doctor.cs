using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Api.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public string Specialisation { get; set; }

        [Required]
        public int YearsOfExperience { get; set; }

        [Required]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; } = true;
    }
}