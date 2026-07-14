using HealthCareApp.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientNotificationsController : ControllerBase
    {
        private const string InvalidUserTokenMessage = "Invalid user token.";

        private readonly IPatientNotificationService patientNotificationService;

        public PatientNotificationsController(
            IPatientNotificationService patientNotificationService)
        {
            this.patientNotificationService = patientNotificationService;
        }

        [HttpGet("my-unread")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Patient")]
        public async Task<IActionResult> GetMyUnreadNotifications()
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized(new
                {
                    Message = InvalidUserTokenMessage
                });
            }

            var notifications =
                await patientNotificationService.GetMyUnreadNotificationsAsync(
                    identityUserId);

            return Ok(notifications);
        }

        [HttpPut("{patientNotificationId:int}/read")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Patient")]
        public async Task<IActionResult> MarkAsRead(
            [FromRoute] int patientNotificationId)
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized(new
                {
                    Message = InvalidUserTokenMessage
                });
            }

            var notification =
                await patientNotificationService.MarkAsReadAsync(
                    patientNotificationId,
                    identityUserId);

            return Ok(notification);
        }
    }
}