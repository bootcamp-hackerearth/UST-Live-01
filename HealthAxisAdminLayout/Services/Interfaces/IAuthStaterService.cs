namespace HealthAxisAdminLayout.Services.Interfaces
{
    using HealthAxisAdminLayout.DTOs.Auth;
    

    
    
        public interface IAuthStateService
        {
            string? Token { get; }

            string? Email { get; }

            string? Role { get; }

            int? ReferenceId { get; }

            bool IsFirstLogin { get; }

            bool IsLoggedIn { get; }

            bool IsAdmin { get; }

            void SetLogin(AuthResponseDTO response);

            void Logout();
        }
    }
