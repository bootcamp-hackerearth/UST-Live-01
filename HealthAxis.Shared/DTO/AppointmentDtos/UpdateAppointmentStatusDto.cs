using HealthAxis.Shared.Enums;
using HealthAxis.Shared.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HealthAxis.Shared.DTO.AppointmentDtos
{
    public class UpdateAppointmentStatusDto
    {
        [Required]
        public AppointmentStatus Status { get; set; }

        [StringLength(ValidationLimits.CancellationReasonLength)]
        public string? CancellationReason { get; set; }
    }
}