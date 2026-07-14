namespace HealthCareApp.Helpers
{
    public static class ConsoleHighlightHelper
    {
        private static readonly object ConsoleLock = new();

        public static void WriteAppointmentEventBox(
            string title,
            int appointmentId,
            string patientName,
            int doctorId,
            DateTime scheduledDate,
            string timeSlot)
        {
            WriteHighlightedLine(
                $"{title} | AppointmentId: {appointmentId} | Patient: {patientName} | DoctorId: {doctorId} | Date: {scheduledDate:yyyy-MM-dd} | TimeSlot: {timeSlot}",
                ConsoleColor.Yellow);
        }

        public static void WriteNotificationBox(
            int appointmentId,
            int doctorId,
            string message)
        {
            WriteHighlightedLine(
                $"APPOINTMENT NOTIFICATION CREATED | AppointmentId: {appointmentId} | DoctorId: {doctorId} | Message: {message}",
                ConsoleColor.Green);
        }

        public static void WriteCacheBox(
            string title,
            string cacheKey,
            string status,
            ConsoleColor backgroundColor)
        {
            WriteHighlightedLine(
                $"{title} | CacheStatus: {status} | CacheKey: {cacheKey}",
                backgroundColor);
        }

        private static void WriteHighlightedLine(
            string message,
            ConsoleColor foregroundColor)
        {
            lock (ConsoleLock)
            {
                var previousForegroundColor = Console.ForegroundColor;

                try
                {
                    Console.ForegroundColor = foregroundColor;
                    Console.WriteLine(TrimForConsole(message, 240));
                }
                finally
                {
                    Console.ForegroundColor = previousForegroundColor;
                }
            }
        }

        private static string TrimForConsole(string value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }

            return value.Length <= maxLength
                ? value
                : value[..(maxLength - 3)] + "...";
        }
    }
}
