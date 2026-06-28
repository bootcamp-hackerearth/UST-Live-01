using HealthApp.AdminPortal.Auth;
using HealthApp.AdminPortal.Models;
using HealthApp.AdminPortal.Services.Interface;
using HealthApp.Shared.Dtos;
using System.Net.Http.Json;

namespace HealthApp.AdminPortal.Services.Impl
{
    public class AuthService : BaseApiService, IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly CustomAuthStateProvider _authStateProvider;

        public AuthService(
            HttpClient http,
            ITokenService tokenService,
            CustomAuthStateProvider authStateProvider)
            : base(http, tokenService)
        {
            _tokenService = tokenService;
            _authStateProvider = authStateProvider;
        }

        public async Task<ApiResult> Login(LoginDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/auth/login", dto);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await ReadErrorMessageAsync(response);
                return ApiResult.Failure(errorMessage);
            }

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

            if (result == null || string.IsNullOrWhiteSpace(result.AccessToken))
            {
                return ApiResult.Failure("Login failed. Token was not returned.");
            }

            await _tokenService.SetToken(result.AccessToken);

            _authStateProvider.NotifyUserAuthentication(result.AccessToken);

            return ApiResult.Success(result.Message ?? "Login successful.");
        }

        public async Task Logout()
        {
            await _tokenService.RemoveToken();

            _authStateProvider.NotifyUserLogout();
        }

        public async Task<ApiResult<RegisterDoctorResult>> RegisterDoctor(DoctorCreateDto dto)
        {
            await AddAuthHeaderAsync();

            var response = await _http.PostAsJsonAsync("api/auth/register/doctor", dto);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await ReadErrorMessageAsync(response);
                return ApiResult<RegisterDoctorResult>.Failure(errorMessage);
            }

            var result = await response.Content.ReadFromJsonAsync<RegisterDoctorResponse>();

            if (result == null)
            {
                return ApiResult<RegisterDoctorResult>.Failure("Doctor was created, but response was invalid.");
            }

            var data = new RegisterDoctorResult
            {
                Message = result.Message,
                UserId = result.UserId,
                TemporaryPassword = result.TemporaryPassword
            };

            return ApiResult<RegisterDoctorResult>.Success(
                data,
                result.Message ?? "Doctor registered successfully.");
        }

        public async Task<ApiResult> ChangePassword(ChangePasswordDto dto)
        {
            await AddAuthHeaderAsync();

            var response = await _http.PostAsJsonAsync("api/auth/change-password", dto);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await ReadErrorMessageAsync(response);
                return ApiResult.Failure(errorMessage);
            }

            return ApiResult.Success("Password changed successfully.");
        }

        private sealed class RegisterDoctorResponse
        {
            public string Message { get; set; } = string.Empty;
            public string UserId { get; set; } = string.Empty;
            public string TemporaryPassword { get; set; } = string.Empty;
        }
    }

    public class RegisterDoctorResult
    {
        public string Message { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string TemporaryPassword { get; set; } = string.Empty;
    }
}