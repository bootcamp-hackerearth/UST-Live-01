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
            lock (ConsoleLock)
            {
                WriteBox(
                    title,
                    new[]
                    {
                        ("Appointment Id", appointmentId.ToString()),
                        ("Patient Name", patientName),
                        ("Doctor Id", doctorId.ToString()),
                        ("Date", scheduledDate.ToString("yyyy-MM-dd")),
                        ("Time Slot", timeSlot)
                    },
                    ConsoleColor.Black,
                    ConsoleColor.Yellow);
            }
        }

        public static void WriteNotificationBox(
            int appointmentId,
            int doctorId,
            string message)
        {
            lock (ConsoleLock)
            {
                WriteBox(
                    "APPOINTMENT BOOKED EVENT CONSUMED - NOTIFICATION CREATED",
                    new[]
                    {
                        ("Appointment Id", appointmentId.ToString()),
                        ("Doctor Id", doctorId.ToString()),
                        ("Message", message)
                    },
                    ConsoleColor.Black,
                    ConsoleColor.Green);
            }
        }

        public static void WriteCacheBox(
            string title,
            string cacheKey,
            string status,
            ConsoleColor backgroundColor)
        {
            lock (ConsoleLock)
            {
                WriteBox(
                    title,
                    new[]
                    {
                        ("Cache Status", status),
                        ("Cache Key", cacheKey),
                        ("Time", DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"))
                    },
                    ConsoleColor.White,
                    backgroundColor);
            }
        }

        private static void WriteBox(
            string title,
            IEnumerable<(string Label, string Value)> rows,
            ConsoleColor foregroundColor,
            ConsoleColor backgroundColor)
        {
            var previousForegroundColor = Console.ForegroundColor;
            var previousBackgroundColor = Console.BackgroundColor;

            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;

            const int boxWidth = 100;
            const int labelWidth = 16;
            const int valueWidth = 76;

            Console.WriteLine();
            Console.WriteLine(new string('=', boxWidth));
            Console.WriteLine($"| {TrimForConsole(title, boxWidth - 4),-(boxWidth - 4)} |");
            Console.WriteLine(new string('=', boxWidth));

            foreach (var row in rows)
            {
                Console.WriteLine(
                    $"| {TrimForConsole(row.Label, labelWidth),-labelWidth} : {TrimForConsole(row.Value, valueWidth),-valueWidth} |");
            }

            Console.WriteLine(new string('=', boxWidth));
            Console.WriteLine();

            Console.ForegroundColor = previousForegroundColor;
            Console.BackgroundColor = previousBackgroundColor;
            Console.ResetColor();
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