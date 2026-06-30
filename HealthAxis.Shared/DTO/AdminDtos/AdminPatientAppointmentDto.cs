namespace HealthAxis.Shared.DTO.AdminDtos
{
    public sealed class AdminPatientAppointmentDto
    {
        public int AppointmentId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public string Specialisation { get; set; } = string.Empty;

        public DateTime ScheduledDate { get; set; }

        public string TimeSlot { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
    }
}