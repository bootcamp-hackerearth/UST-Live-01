namespace HealthCareApp.Shared.Dtos.DoctorLeaves
{
    public class DoctorLeaveDto
    {
        public int DoctorLeaveId { get; set; }

        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public string Reason { get; set; } = string.Empty;

        public DateTime CreatedDateUtc { get; set; }
    }
}