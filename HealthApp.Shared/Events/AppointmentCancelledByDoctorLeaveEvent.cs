namespace HealthApp.Shared.Events
{
    public record AppointmentCancelledByDoctorLeaveEvent(
        int AppointmentId,
        int PatientId,
        string PatientUserId,
        string PatientName,
        int DoctorId,
        string DoctorName,
        DateTime ScheduledDate,
        string TimeSlot,
        DateTime LeaveStartDate,
        DateTime LeaveEndDate,
        string LeaveReason);
}