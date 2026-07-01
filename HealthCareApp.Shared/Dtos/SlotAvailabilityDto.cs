namespace HealthCareApp.Shared.Dtos.Doctors
{
    public class SlotAvailabilityDto
    {
        public string TimeSlot { get; set; } = string.Empty;

        public bool IsBooked { get; set; }
    }
}