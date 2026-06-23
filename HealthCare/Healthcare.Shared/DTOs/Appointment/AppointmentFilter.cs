namespace Healthcare.Shared.DTOs.Appointment
{
    public class AppointmentFilter : PaginationParam
    {
        public string? Status { get; set; }
        public DateOnly? ScheduledDate { get; set; }
    }
}
