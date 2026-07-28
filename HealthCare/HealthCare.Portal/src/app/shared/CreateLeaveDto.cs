using System.ComponentModel.DataAnnotations;

namespace HealthCare.Api.DTOs.Doctor
{
    public class CreateLeaveDto
    {

        [Required]
        public required DateOnly LeaveDate { get; set; }

        [MaxLength(500)]
        public string? Reason { get; set; }

    }
}