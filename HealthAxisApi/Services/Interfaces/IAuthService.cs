using HealthAxisCore_Api.DTOs.User;

public interface IAuthService
{
    Task<AuthResponseDTO> RegisterAsync(RegisterDTO request);

    Task<AuthResponseDTO> LoginAsync(LoginDTO request);

    Task ChangePasswordAsync(ChangePasswordDTO request); // ✅ NEW
}