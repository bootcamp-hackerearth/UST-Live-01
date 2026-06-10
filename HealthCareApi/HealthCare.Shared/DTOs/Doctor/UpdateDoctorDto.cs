using System.ComponentModel.DataAnnotations;

namespace HealthCare.Shared.DTOs.Doctor
{
    public class UpdateDoctorDto
    {
        [Required]
        public int DoctorId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [StringLength(50)]
        public string Specialisation { get; set; }

        [Range(0, 60)]
        public int YearsOfExperience { get; set; }

        [Range(0, 100000)]
        public decimal ConsultationFee { get; set; }
    }
}