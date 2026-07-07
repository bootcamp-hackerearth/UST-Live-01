namespace HealthCareApp.Helpers
{
    public static class ConsoleHighlightHelper
    {
        public static void WriteAppointmentEventBox(
            string title,
            int appointmentId,
            string patientName,
            int doctorId,
            DateTime scheduledDate,
            string timeSlot)
        {
            var previousForegroundColor = Console.ForegroundColor;
            var previousBackgroundColor = Console.BackgroundColor;

            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Yellow;

            Console.WriteLine();
            Console.WriteLine("====================================================================================================");
            Console.WriteLine($"| {title,-96} |");
            Console.WriteLine("====================================================================================================");
            Console.WriteLine($"| Appointment Id : {appointmentId,-80} |");
            Console.WriteLine($"| Patient Name   : {patientName,-80} |");
            Console.WriteLine($"| Doctor Id      : {doctorId,-80} |");
            Console.WriteLine($"| Date           : {scheduledDate:yyyy-MM-dd,-80} |");
            Console.WriteLine($"| Time Slot      : {timeSlot,-80} |");
            Console.WriteLine("====================================================================================================");
            Console.WriteLine();

            Console.ForegroundColor = previousForegroundColor;
            Console.BackgroundColor = previousBackgroundColor;
        }

        public static void WriteNotificationBox(
            int appointmentId,
            int doctorId,
            string message)
        {
            var previousForegroundColor = Console.ForegroundColor;
            var previousBackgroundColor = Console.BackgroundColor;

            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGreen;

            Console.WriteLine();
            Console.WriteLine("====================================================================================================");
            Console.WriteLine("| APPOINTMENT BOOKED EVENT CONSUMED - NOTIFICATION CREATED                                          |");
            Console.WriteLine("====================================================================================================");
            Console.WriteLine($"| Appointment Id : {appointmentId,-80} |");
            Console.WriteLine($"| Doctor Id      : {doctorId,-80} |");
            Console.WriteLine($"| Message        : {TrimForConsole(message, 80),-80} |");
            Console.WriteLine("====================================================================================================");
            Console.WriteLine();

            Console.ForegroundColor = previousForegroundColor;
            Console.BackgroundColor = previousBackgroundColor;
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