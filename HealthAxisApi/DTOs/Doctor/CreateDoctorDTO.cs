using HealthAxisCore_Api.Enums;
using System.ComponentModel.DataAnnotations;

namespace HealthAxisCore_Api.DTOs.Doctor
{
    public class CreateDoctorDTO
    {
        [Required]
        public string DoctorName { get; set; } = null!;

        [Required]
        public SpecialisationType Specialisation { get; set; }

        public int YearsOfExperience { get; set; }

        public int ConsultationFee { get; set; }

        public bool IsActive { get; set; }
    }
}