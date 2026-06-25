using HealthApp.AdminPortal.Models;
using HealthApp.AdminPortal.Services.Impl;
using HealthApp.Shared.Dtos;

namespace HealthApp.AdminPortal.Services.Interface
{
    public interface IAuthService
    {
        Task<ApiResult> Login(LoginDto dto);

        Task Logout();

        Task<ApiResult<RegisterDoctorResult>> RegisterDoctor(DoctorCreateDto dto);

        Task<ApiResult> ChangePassword(ChangePasswordDto dto);
    }
}