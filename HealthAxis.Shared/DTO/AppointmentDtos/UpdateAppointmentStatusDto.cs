using HealthAxis.Shared.Enums;

namespace HealthAxis.Shared.DTO.AppointmentDtos
{
    public class UpdateAppointmentStatusDto
    {
        public AppointmentStatus Status { get; set; }

        public string? CancellationReason { get; set; }
    }
}