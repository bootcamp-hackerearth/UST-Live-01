namespace HealthAxisAdminLayout.Services.Implmentations
{
    using HealthAxisAdminLayout.DTOs.Auth;
    using HealthAxisAdminLayout.Services.Interfaces;
    

    public class AuthStateService : IAuthStateService
    {
        public string? Token { get; private set; }

        public string? Email { get; private set; }

        public string? Role { get; private set; }

        public int? ReferenceId { get; private set; }

        public bool IsFirstLogin { get; private set; }

        public bool IsLoggedIn => !string.IsNullOrWhiteSpace(Token);

        public bool IsAdmin => Role == "Admin";

        public void SetLogin(AuthResponseDTO response)
        {
            Token = response.Token;
            Email = response.Email;
            Role = response.Role;
            ReferenceId = response.ReferenceId;
            IsFirstLogin = response.IsFirstLogin;
        }

        public void Logout()
        {
            Token = null;
            Email = null;
            Role = null;
            ReferenceId = null;
            IsFirstLogin = false;
        }
    }
}



