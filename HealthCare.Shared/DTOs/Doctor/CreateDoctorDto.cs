using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Shared.DTOs.Doctor
{
    public class CreateDoctorDto
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [StringLength(50)]
        public string Specialisation { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Range(0, 60)]
        public int YearsOfExperience { get; set; }

        [Required]
        [Range(0, 100000)]
        public decimal ConsultationFee { get; set; }

        [Required]
        public List<string> TimeSlots { get; set; }
    }
}
