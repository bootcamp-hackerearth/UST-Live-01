namespace HealthCareApp.Options
{
    public class NotificationCleanupOptions
    {
        public int RetentionDays { get; set; } = 30;

        public int IntervalHours { get; set; } = 24;
    }
}