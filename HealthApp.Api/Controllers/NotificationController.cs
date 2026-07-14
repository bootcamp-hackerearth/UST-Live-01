using HealthApp.Api.Exceptions;
using HealthApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthApp.Api.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = "Patient")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(
            INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("my/unread-doctor-leave")]
        public async Task<IActionResult>
            GetMyUnreadDoctorLeaveNotifications(
                CancellationToken ct)
        {
            var userId = GetRequiredUserId();

            var notifications = await _notificationService
                .GetUnreadDoctorLeaveNotificationsAsync(
                    userId,
                    ct);

            return Ok(notifications);
        }

        [HttpPatch("{notificationId:int}/acknowledge")]
        public async Task<IActionResult> Acknowledge(
            int notificationId,
            CancellationToken ct)
        {
            var userId = GetRequiredUserId();

            await _notificationService
                .AcknowledgeNotificationAsync(
                    notificationId,
                    userId,
                    ct);

            return NoContent();
        }

        private string GetRequiredUserId()
        {
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ForbiddenAccessException(
                    "Patient user account is not linked to this request.");
            }

            return userId;
        }
    }
}
