using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.Shared.Dtos.Notifications
{
    public class CreateDoctorLeaveDto
    {
        [Required(ErrorMessage = "Doctor is required.")]
        public int DoctorId { get; set; }

        [Required(ErrorMessage = "Leave start date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Leave end date is required.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Leave reason is required.")]
        [StringLength(300, ErrorMessage = "Leave reason cannot exceed 300 characters.")]
        public string Reason { get; set; } = string.Empty;
    }
}