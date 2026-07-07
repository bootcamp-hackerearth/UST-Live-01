namespace HealthCareApp.Shared.Dtos.Appointments
{
    public class AppointmentFilterOptionsDto
    {
        public List<AppointmentFilterPersonDto> Patients { get; set; } = new();

        public List<AppointmentFilterPersonDto> Doctors { get; set; } = new();
    }
}