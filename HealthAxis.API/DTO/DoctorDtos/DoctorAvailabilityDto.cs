namespace HealthAxis.API.DTO.DoctorDtos
{
    public class DoctorAvailabilityDto
    {
        public int DoctorId { get; set; }

        public DateTime Date { get; set; }

        public string TimeSlot { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}