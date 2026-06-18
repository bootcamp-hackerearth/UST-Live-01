namespace S3_HealthAxisApi.DTOs.Admin
{
    public class UserManagementDto
    {
        public int UserId { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public int? ReferenceId { get; set; }
    }
}