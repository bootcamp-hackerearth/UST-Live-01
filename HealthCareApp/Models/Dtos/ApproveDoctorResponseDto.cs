namespace HealthCareApp.Dtos
{
    public class ApproveDoctorResponseDto
    {
        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string TemporaryPassword { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }
}