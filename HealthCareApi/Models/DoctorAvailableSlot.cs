namespace HealthCareApi.Models
{
    public class DoctorAvailableSlot
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string TimeSlot { get; set; }

        public virtual Doctor Doctor { get; set; }
    }
}