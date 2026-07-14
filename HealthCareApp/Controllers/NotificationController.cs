using HealthCareApp.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthCareApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationsController : ControllerBase
    {
        private const string InvalidUserTokenMessage = "Invalid user token.";

        private readonly INotificationService notificationService;

        private readonly ILogger<NotificationsController> logger;

        public NotificationsController(
            INotificationService notificationService,
            ILogger<NotificationsController> logger)
        {
            this.notificationService = notificationService;
            this.logger = logger;
        }

        [HttpGet("my-unread")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyUnreadNotifications()
        {
            var identityUserId = GetLoggedInUserId();

            var notifications = await notificationService.GetMyUnreadNotificationsForPatientAsync(
                identityUserId);

            return Ok(notifications);
        }

        [HttpPut("{notificationId:int}/read")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> MarkNotificationAsRead(
            [FromRoute] int notificationId)
        {
            var identityUserId = GetLoggedInUserId();

            var notification = await notificationService.MarkNotificationAsReadForPatientAsync(
                notificationId,
                identityUserId);

            LogPatientNotificationMarkedAsReadThroughApi(
                notification.NotificationId);

            return Ok(notification);
        }

        private void LogPatientNotificationMarkedAsReadThroughApi(
            int notificationId)
        {
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }

            logger.LogInformation(
                "Patient notification marked as read through API. NotificationId: {NotificationId}",
                notificationId);
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