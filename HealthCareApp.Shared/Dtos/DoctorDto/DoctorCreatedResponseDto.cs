
namespace HealthCareApp.Shared.Dtos.Doctors
{
    public class DoctorCreatedResponseDto
    {
        public int DoctorId { get; set; }

        public string DoctorName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string TemporaryPassword { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }
}