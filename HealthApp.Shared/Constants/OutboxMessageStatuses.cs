namespace HealthApp.Shared.Constants;

public static class OutboxMessageStatuses
{
    public const string Pending = "Pending";
    public const string Processing = "Processing";
    public const string Published = "Published";
    public const string Failed = "Failed";
}