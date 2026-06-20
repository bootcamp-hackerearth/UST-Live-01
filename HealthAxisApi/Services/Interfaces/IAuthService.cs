using HealthAxisCore_Api.DTOs.Patient;
using HealthAxisCore_Api.DTOs.User;

namespace HealthAxisCore_Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO request);

        Task<AuthResponseDTO> LoginAsync(LoginDTO request);

        Task ChangePasswordAsync(ChangePasswordDTO request); 
    }
}