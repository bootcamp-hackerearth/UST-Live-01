using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisCore_Api.Controllers
{
    [ApiController]
    [Route("api/admin-handoff")]
    public class AdminHandoffController(
        IAdminHandoffService service
    ) : ControllerBase
    {
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CreateAdminHandoffResponseDto>> Create(
            CancellationToken ct)
        {
            return Ok(await service.CreateAsync(User, ct));
        }

        [HttpPost("exchange")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Exchange(
            ExchangeAdminHandoffRequestDto request,
            CancellationToken ct)
        {
            return Ok(await service.ExchangeAsync(request, ct));
        }
    }
}