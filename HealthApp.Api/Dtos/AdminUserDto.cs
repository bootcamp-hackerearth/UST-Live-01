namespace HealthApp.Api.Dtos
{
    public class AdminUserDto
    {
        public string Id { get; set; } = string.Empty;

        public string? Email { get; set; }

        public int? PatientId { get; set; }

        public int? DoctorId { get; set; }

        public IList<string> Roles { get; set; } = new List<string>();
    }
}