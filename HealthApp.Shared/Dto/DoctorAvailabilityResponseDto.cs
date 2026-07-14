namespace HealthApp.Shared.Dto
{
    public class DoctorAvailabilityResponseDto
    {
        public int DoctorId { get; set; }

        public DateTime Date { get; set; }

        public bool IsDoctorOnLeave { get; set; }

        public string Message { get; set; } = string.Empty;

        public List<DoctorSlotDto> Slots { get; set; } = new();
    }

    public class DoctorSlotDto
    {
        public string TimeSlot { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}