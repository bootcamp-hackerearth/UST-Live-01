namespace HealthCareApp.Shared.Dtos.DoctorLeaves
{
    public class DoctorLeaveStatusDto
    {
        public int DoctorId { get; set; }

        public DateOnly Date { get; set; }

        public bool IsDoctorOnLeave { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}