namespace HealthApp.Shared.Dtos
{
    public class AdminDoctorLeaveDto
    {
        public int DoctorLeaveId { get; set; }

        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public string Reason { get; set; } = string.Empty;

        public DateTime CreatedAtUtc { get; set; }

        public string Status { get; set; } = string.Empty;

        public bool IsSingleDayLeave { get; set; }
    }
}
