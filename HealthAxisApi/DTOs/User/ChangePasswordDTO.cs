namespace HealthAxisCore_Api.DTOs.User
{
    public class ChangePasswordDTO
    {
        public string Email { get; set; } = null!;

        public string OldPassword { get; set; } = null!;

        public string NewPassword { get; set; } = null!;
    }
}
