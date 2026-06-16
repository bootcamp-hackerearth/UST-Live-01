using HealthAxisHealth.Shared.Enums;
using HealthAxisHealth.Shared.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HealthAxisHealth.Shared.DTOs.AppointmentDtos
{
    public class UpdateAppointmentStatusDto
    {
        #region Properties

        [Required(
            ErrorMessage = ValidationMessages.AppointmentStatusRequired)]
        public AppointmentStatus Status { get; set; }

        [StringLength(
            ValidationLimits.CancellationReasonLength)]
        public string? CancellationReason { get; set; }

        #endregion
    }
}
