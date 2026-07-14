using System.ComponentModel.DataAnnotations;

namespace HealthCareApp.Shared.Dtos.DoctorLeaves
{
    public class CreateDoctorLeaveRequest
    {
        [Required(ErrorMessage = "Leave start date is required.")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "Leave end date is required.")]
        public DateOnly EndDate { get; set; }

        [Required(ErrorMessage = "Leave reason is required.")]
        [StringLength(500, ErrorMessage = "Leave reason cannot exceed 500 characters.")]
        public string Reason { get; set; } = string.Empty;
    }
}