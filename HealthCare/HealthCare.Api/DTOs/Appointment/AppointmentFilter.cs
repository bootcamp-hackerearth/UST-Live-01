namespace HealthCare.Api.DTOs.Appointment
{
    public class AppointmentFilter : PaginationParam
    {
        public string? Status { get; set; }
        public DateOnly? ScheduledDate { get; set; }
    }
}
