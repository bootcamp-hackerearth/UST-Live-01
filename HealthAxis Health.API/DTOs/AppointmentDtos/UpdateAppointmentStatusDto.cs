using HealthAxisHealth.API.Enums;
using HealthAxisHealth.API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace HealthAxisHealth.API.DTOs.AppointmentDtos
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
