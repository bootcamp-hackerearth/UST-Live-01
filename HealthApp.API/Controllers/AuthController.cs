using HealthApp.Shared.DTOs;
using HealthApp.API.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterRequestDto dto)
    {
        var response = await authService.RegisterAsync(dto);
        response.Message = "Registered successfully.";
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginRequestDto dto)
    {
        var response = await authService.LoginAsync(dto);
        response.Message = "Login successful.";
        return Ok(response);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthResponseDto>> Refresh(RefreshTokenRequestDto dto)
    {
        var response = await authService.RefreshTokenAsync(dto);
        response.Message = "Token refreshed successfully.";
        return Ok(response);
    }

    [HttpPost("change-password")]
    public async Task<ActionResult> ChangePassword(ChangePasswordDto dto)
    {
        await authService.ChangePasswordAsync(dto);
        return Ok(new
        {
            message = "Password changed successfully. Please login again."
        });
    }
}