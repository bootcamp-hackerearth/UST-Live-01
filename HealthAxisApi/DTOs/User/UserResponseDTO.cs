using HealthAxisCore_Api.Enums;

namespace HealthAxisCore_Api.DTOs.User
{
    public class UserResponseDTO
    {
        public int UserId { get; set; }

        public string Email { get; set; } = null!;

        public UserRole Role { get; set; }

        public int? ReferenceId { get; set; }
    }
}
