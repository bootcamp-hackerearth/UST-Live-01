using HealthAxisHealth.Shared.Enums;
using HealthAxisHealth.Shared.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxisHealth.Shared.DTOs.AppointmentDtos
{
    [ExcludeFromCodeCoverage]
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
