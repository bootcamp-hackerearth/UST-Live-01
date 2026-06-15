using HealthAxis.API.Enums;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.API.Dtos.DoctorDtos
{
    public class CreateDoctorDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public Specialization Specialisation { get; set; }

        [Required]
        public DateOnly PracticeStartDate { get; set; }

        [Required]
        public decimal ConsultationFee { get; set; }
    }
}