namespace HealthAxisCore_Api.Models.Dtos
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }

    public class AppointmentReportDto
    {
        public DateTime Date { get; set; }
        public int Confirmed { get; set; }
        public int Cancelled { get; set; }
        public int Completed { get; set; }
    }
}
