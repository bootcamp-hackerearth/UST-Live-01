using HealthAxis.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.Dtos.DoctorDtos
{
    public class UpdateDoctorDto
    {
        [Required]
        public Specialization Specialisation { get; set; }

        [Required]
        public DateOnly PracticeStartDate { get; set; }

        [Required]
        public decimal ConsultationFee { get; set; }

        public bool IsAvailable { get; set; }
    }
}