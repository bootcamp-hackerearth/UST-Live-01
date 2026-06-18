namespace HealthCare.Api.DTOs.Doctor
{
    public class CreateLeaveResultDto
    {
        public List<DateOnly> SkippedDates { get; set; } = new();
        public List<DateOnly> CreatedWithCancelledAppointments { get; set; } = new();
    }
}
