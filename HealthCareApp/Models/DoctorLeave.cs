namespace HealthCareApp.Models
{
    public class DoctorLeave
    {
        public int DoctorLeaveId { get; set; }

        public int DoctorId { get; set; }

        public Doctor Doctor { get; set; } = null!;

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public string Reason { get; set; } = string.Empty;

        public DateTime CreatedDateUtc { get; set; }
    }
}
