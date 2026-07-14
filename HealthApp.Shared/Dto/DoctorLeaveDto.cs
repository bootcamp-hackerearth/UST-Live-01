namespace HealthApp.Shared.Dto
{
    public class DoctorLeaveDto
    {
        public int DoctorLeaveId { get; set; }

        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Reason { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
    }
}