namespace HealthAxis.Shared.DTO.AdminDtos
{
    public sealed class AdminUserDto
    {
        public string UserId { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime? CreatedDate { get; set; }
    }
}