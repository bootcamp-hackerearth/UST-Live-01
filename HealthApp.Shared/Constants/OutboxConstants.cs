namespace HealthApp.Shared.Constants;

public static class OutboxConstants
{
    public const string AppointmentBookedEventType =
        "AppointmentBookedEvent";

    public const int ProcessingIntervalSeconds = 10;

    public const int BatchSize = 20;

    public const int MaximumRetryCount = 10;

    public const int MaximumErrorMessageLength = 2000;
}