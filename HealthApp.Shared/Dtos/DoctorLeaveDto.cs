using System.ComponentModel.DataAnnotations;

namespace HealthApp.Shared.Dtos
{
    public class DoctorLeaveCreateDto
    {
        [Required(ErrorMessage = "Leave start date is required.")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "Leave end date is required.")]
        public DateOnly EndDate { get; set; }

        [Required(ErrorMessage = "Leave reason is required.")]
        [MinLength(3, ErrorMessage = "Leave reason must be at least 3 characters long.")]
        [StringLength(500, ErrorMessage = "Leave reason cannot exceed 500 characters.")]
        public string Reason { get; set; } = string.Empty;
    }

    public class DoctorLeaveDto
    {
        [Required]
        public int DoctorLeaveId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Doctor name must be at least 3 characters long.")]
        [StringLength(50, ErrorMessage = "Doctor name cannot exceed 50 characters.")]
        public string DoctorName { get; set; } = string.Empty;

        [Required]
        public DateOnly StartDate { get; set; }

        [Required]
        public DateOnly EndDate { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Leave reason must be at least 3 characters long.")]
        [StringLength(500, ErrorMessage = "Leave reason cannot exceed 500 characters.")]
        public string Reason { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAtUtc { get; set; }
    }

    public class DoctorLeaveCreationResultDto
    {
        public DoctorLeaveDto Leave { get; set; } = new();

        public int CancelledAppointmentCount { get; set; }

        public List<int> CancelledAppointmentIds { get; set; } = new();
    }
}