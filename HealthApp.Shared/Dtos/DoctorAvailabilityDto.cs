namespace HealthApp.Shared.Dtos
{
    public class DoctorAvailabilityDto
    {
        public int DoctorId { get; set; }

        public DateOnly Date { get; set; }

        public bool IsDoctorOnLeave { get; set; }

        public string Message { get; set; } = string.Empty;

        public List<DoctorAvailabilitySlotDto> Slots { get; set; } = new();
    }

    public class DoctorAvailabilitySlotDto
    {
        public string TimeSlot { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}