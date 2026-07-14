using HealthCareApp.Services.Interface;
using HealthCareApp.Shared.Dtos.DoctorLeaves;
using HealthCareApp.Shared.Dtos.Notifications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthCareApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DoctorLeavesController : ControllerBase
    {
        private const string InvalidUserTokenMessage = "Invalid logged-in user.";

        private readonly IDoctorLeaveService doctorLeaveService;

        private readonly ILogger<DoctorLeavesController> logger;

        public DoctorLeavesController(
            IDoctorLeaveService doctorLeaveService,
            ILogger<DoctorLeavesController> logger)
        {
            this.doctorLeaveService = doctorLeaveService;
            this.logger = logger;
        }

        [HttpPost("my-leaves")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<DoctorLeaveDto>> CreateMyDoctorLeaveAsync(
            [FromBody] CreateMyDoctorLeaveDto dto)
        {
            var identityUserId = GetLoggedInUserId();

            var doctorLeave = await doctorLeaveService.CreateMyDoctorLeaveAsync(
                dto,
                identityUserId);

            LogDoctorLeaveCreatedThroughApi(doctorLeave);

            return Ok(doctorLeave);
        }

        [HttpGet("my-leaves")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<List<DoctorLeaveDto>>> GetMyDoctorLeavesAsync()
        {
            var identityUserId = GetLoggedInUserId();

            var doctorLeaves = await doctorLeaveService.GetMyDoctorLeavesAsync(
                identityUserId);

            return Ok(doctorLeaves);
        }

        [HttpGet("doctor/{doctorId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<DoctorLeaveDto>>> GetDoctorLeavesByDoctorIdAsync(
            [FromRoute] int doctorId)
        {
            var doctorLeaves = await doctorLeaveService.GetDoctorLeavesByDoctorIdAsync(
                doctorId);

            return Ok(doctorLeaves);
        }

        [HttpGet("doctor/{doctorId:int}/on-leave")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<object>> IsDoctorOnLeaveAsync(
            [FromRoute] int doctorId,
            [FromQuery] DateTime date)
        {
            var isOnLeave = await doctorLeaveService.IsDoctorOnLeaveAsync(
                doctorId,
                date);

            return Ok(new
            {
                doctorId,
                date = date.ToString("yyyy-MM-dd"),
                isOnLeave
            });
        }

        private void LogDoctorLeaveCreatedThroughApi(
            DoctorLeaveDto doctorLeave)
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                "Doctor leave created by logged-in doctor through API. DoctorLeaveId: {DoctorLeaveId}, DoctorId: {DoctorId}, StartDate: {StartDate}, EndDate: {EndDate}",
                doctorLeave.DoctorLeaveId,
                doctorLeave.DoctorId,
                doctorLeave.StartDate,
                doctorLeave.EndDate);
        }

        private string GetLoggedInUserId()
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                throw new UnauthorizedAccessException(InvalidUserTokenMessage);
            }

            return identityUserId;
        }
    }
}