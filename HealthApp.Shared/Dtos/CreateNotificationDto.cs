public class CreateNotificationDto
{
    public string RecipientUserId { get; set; } = string.Empty;

    public string NotificationType { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public int? RelatedEntityId { get; set; }

    public string? RelatedEntityType { get; set; }
}