using System.Collections.Generic;

namespace HealthCareApp.Constants
{
    public static class TimeSlots
    {
        public static IReadOnlyList<string> Slots { get; } = new List<string>
        {
            // Morning
            "09:00 AM - 09:30 AM",
            "09:30 AM - 10:00 AM",
            "10:00 AM - 10:30 AM",
            "10:30 AM - 11:00 AM",
            "11:00 AM - 11:30 AM",
            "11:30 AM - 12:00 PM",
            "12:00 PM - 12:30 PM",

            // Afternoon
            "02:00 PM - 02:30 PM",
            "02:30 PM - 03:00 PM",
            "03:00 PM - 03:30 PM",
            "03:30 PM - 04:00 PM",
            "04:00 PM - 04:30 PM",
            "04:30 PM - 05:00 PM",
            "05:00 PM - 05:30 PM",
            "05:30 PM - 06:00 PM"
        }.AsReadOnly();
    }
}