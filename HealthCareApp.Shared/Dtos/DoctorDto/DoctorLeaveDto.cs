namespace HealthCareApp.Shared.Dtos.DoctorLeaves
{
    public class DoctorLeaveDto
    {
        public int DoctorLeaveId { get; set; }

        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public string StartDate { get; set; } = string.Empty;

        public string EndDate { get; set; } = string.Empty;

        public string Reason { get; set; } = string.Empty;

        public string CreatedDate { get; set; } = string.Empty;
    }
}