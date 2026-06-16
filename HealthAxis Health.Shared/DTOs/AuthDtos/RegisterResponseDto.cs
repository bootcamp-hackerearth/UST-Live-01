namespace HealthAxisHealth.Shared.DTOs.AuthDtos
{
    public class RegisterResponseDto
    {
        public string Status { get; set; } = "success";

        public string Message { get; set; } =
            "User registered successfully.";

        public RegisterDataDto Data { get; set; } =
            new RegisterDataDto();
    }

    public class RegisterDataDto
    {
        public RegisterUserDto User { get; set; } =
            new RegisterUserDto();

        public RegisterTokenDto Tokens { get; set; } =
            new RegisterTokenDto();
    }

    public class RegisterUserDto
    {
        public int Id { get; set; }

        public string Email { get; set; } =
            string.Empty;

        public string FullName { get; set; } =
            string.Empty;

        public DateTime CreatedAt { get; set; }
    }

    public class RegisterTokenDto
    {
        public string AccessToken { get; set; } =
            string.Empty;

        public string RefreshToken { get; set; } =
            string.Empty;

        public int ExpiresIn { get; set; } = 3600;
    }
}