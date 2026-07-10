namespace HealthAxis.Shared.DTO.DoctorDtos
{
    public class DoctorAvailabilityDto
    {
        public int DoctorId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime Date { get; set; }

        public string Message { get; set; } = string.Empty;

        public List<string> AvailableSlots { get; set; } = new();
    }
}