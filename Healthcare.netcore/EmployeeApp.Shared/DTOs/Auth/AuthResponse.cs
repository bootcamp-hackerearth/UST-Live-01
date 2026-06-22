namespace HealthAxis.Shared.DTOs.Auth;

public class AuthResponse
{
    public bool Success { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string AccessToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public int ExpiresIn { get; set; }

    public bool RequiresPasswordChange { get; set; } = false;
}