
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Api.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminHealthRecordController : ControllerBase
    {
        private readonly IHealthRecordService _healthRecordService;

        public AdminHealthRecordController(IHealthRecordService healthRecordService)
        {
            _healthRecordService = healthRecordService;
        }


        [HttpDelete("/records/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteHealthRecord(int id)
        {
            await _healthRecordService.DeleteAsync(id);
            return Ok();
        }
    }
}