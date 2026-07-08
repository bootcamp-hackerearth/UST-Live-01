namespace HealthApp.Shared.Events
{
    public record AppointmentBookedEvent(
        int AppointmentId,
        int PatientId,
        string PatientName,
        int DoctorId,
        string DoctorName,
        DateTime ScheduledDate,
        string TimeSlot);
}