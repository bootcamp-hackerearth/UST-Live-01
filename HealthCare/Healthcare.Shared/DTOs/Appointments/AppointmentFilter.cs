namespace Healthcare.Shared.DTOs.Appointments
{
    public class AppointmentFilter : PaginationParam
    {
        public string? Status { get; set; }
        public DateOnly? ScheduledDate { get; set; }
    }
}
