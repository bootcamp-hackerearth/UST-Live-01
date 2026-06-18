namespace HealthAxis.API.DTOs.Appointments
{
    public class AppointmentReportDto
    {
        public int TotalAppointments { get; set; }

        public int ScheduledAppointments { get; set; }

        public int ConfirmedAppointments { get; set; }

        public int CompletedAppointments { get; set; }

        public int CancelledAppointments { get; set; }
    }
}
