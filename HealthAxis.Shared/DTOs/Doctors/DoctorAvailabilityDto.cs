namespace HealthAxis.API.DTOs.Doctors
{
    public class DoctorAvailabilityDto
    {
        public int DoctorId { get; set; }

        public DateTime Date { get; set; }

        public string TimeSlot { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }
    }
}
