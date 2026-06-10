using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.Dtos
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public Specialisation Specialisation { get; set; }

        [Required]
        [Range(0, 50)]
        public int YearsOfExperience { get; set; }

        [Required]
        [Range(0, 100000)]
        public decimal ConsultationFee { get; set; }

        public bool IsActive { get; set; } = true;
    }
}