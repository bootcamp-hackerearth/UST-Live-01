using HealthAxis.API.Enums;

namespace HealthAxis.API.DTO.AppointmentDtos
{
    public class UpdateAppointmentStatusDto
    {
        public AppointmentStatus Status { get; set; }

        public string? CancellationReason { get; set; }
    }
}