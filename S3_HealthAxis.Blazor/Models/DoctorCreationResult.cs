namespace S3_HealthAxis.Blazor.Models
{
    public class DoctorCreationResult
    {
        public int DoctorId { get; set; }

        public string Email { get; set; } = string.Empty;

        public string TemporaryPassword { get; set; } = string.Empty;
    }
}