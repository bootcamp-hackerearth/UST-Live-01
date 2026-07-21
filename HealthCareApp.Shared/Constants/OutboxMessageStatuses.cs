namespace HealthCareApp.Shared.Constants
{
    public static class OutboxMessageStatuses
    {
        public const string Pending = "Pending";

        public const string Published = "Published";

        public const string Failed = "Failed";
    }
}