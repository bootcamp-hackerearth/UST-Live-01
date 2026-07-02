using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTO.AdminDtos
{
    public sealed class AdminUpdateAppointmentStatusDto
    {
        [Required(ErrorMessage = "Appointment status is required.")]
        public string Status { get; set; } = string.Empty;
    }
}