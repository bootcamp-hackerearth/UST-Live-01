using HealthApp.AdminBlazor.Models;

namespace HealthApp.AdminBlazor.Services;

public class AdminAuthStateService
{
    public bool IsLoggedIn { get; private set; }
    public string AdminEmail { get; private set; } = string.Empty;
    public string? DemoToken { get; private set; }

    public AdminLoginResultDto Login(LoginDto dto)
    {
        if (dto.Email.Equals("admin@healthapp.com", StringComparison.OrdinalIgnoreCase)
            && dto.Password == "Admin@123")
        {
            IsLoggedIn = true;
            AdminEmail = dto.Email;
            DemoToken = "rough-blazor-demo-admin-token";

            return new AdminLoginResultDto
            {
                IsSuccess = true,
                Message = "Login successful.",
                DemoToken = DemoToken
            };
        }

        return new AdminLoginResultDto
        {
            IsSuccess = false,
            Message = "Invalid admin credentials."
        };
    }

    public void Logout()
    {
        IsLoggedIn = false;
        AdminEmail = string.Empty;
        DemoToken = null;
    }
}
