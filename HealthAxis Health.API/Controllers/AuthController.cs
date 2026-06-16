using HealthAxisHealth.Shared.DTOs.AuthDtos;
using HealthAxisHealth.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisHealth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController :
        ControllerBase
    {
        #region Fields

        private readonly IAuthService
            _authService;

        #endregion

        #region Constructor

        public AuthController(
            IAuthService authService)
        {
            _authService =
                authService;
        }

        #endregion

        #region Endpoints

        [HttpPost("register")]
        public async Task<IActionResult>
            Register(
                RegisterDto dto)
        {
            RegisterResponseDto response =
                await _authService
                    .RegisterAsync(dto);

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult>
            Login(
                LoginDto dto)
        {
            LoginResponseDto response =
                await _authService
                    .LoginAsync(dto);

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult>
            RefreshToken(
                RefreshTokenDto dto)
        {
            LoginResponseDto response =
                await _authService
                    .RefreshTokenAsync(dto);

            return Ok(response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(
             [FromBody] RefreshTokenDto dto)
        {
            await _authService.LogoutAsync(
                dto.RefreshToken);

            return Ok(new
            {
                message = "Logged out successfully."
            });
        }

        #endregion
    }
}
