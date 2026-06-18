using HealthAxis.API.Enums;
using HealthAxis.API.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace HealthAxis.API.DTOs.Appointments
{
    public class AppointmentStatusUpdateDto
    {
        [Required(ErrorMessage = Helpers.AppointmentStatusRequired)]
        public AppointmentStatus Status { get; set; }

        [StringLength(ValidationLimits.CancellationReasonLength)]
        public string CancellationReason { get; set; } = string.Empty;
    }
}

