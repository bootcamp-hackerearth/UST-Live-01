using System.ComponentModel.DataAnnotations;

namespace Healthcare.Shared.DTOs.Doctor
{
    public class CreateLeaveDto
    {
        [Required]
        public DateOnly LeaveDate { get; set; }

        [MaxLength(500)]
        public string? Reason { get; set; }
    }
}
