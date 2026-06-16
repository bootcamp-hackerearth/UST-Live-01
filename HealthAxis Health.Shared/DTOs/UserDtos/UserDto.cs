using HealthAxisHealth.Shared.Enums;


namespace HealthAxisHealth.Shared.DTOs.UserDtos
{
    public class UserDto
    {
        public int UserId { get; set; }

        public string Email { get; set; } = string.Empty;

        public UserRole Role { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        // Display name for UI
        public string? FullName { get; set; }
    }


}
