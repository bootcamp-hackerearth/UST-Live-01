using HealthAxis.API.Enums;
using System.Data;

namespace HealthAxis.API.DTOs.Users
{
    public class UserReadDto
    {
        public int UserId { get; set; }

        public string UserCode { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public Role Role { get; set; }

        public int ReferenceId { get; set; }

        public bool IsActive { get; set; }
    }
}
