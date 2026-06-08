using System.Diagnostics.CodeAnalysis;

namespace HealthCare_Appointment_Portal.Utilities
{

    [ExcludeFromCodeCoverage]
    public static class RegexPatterns
    {

        public const string FullName = @"^[a-zA-Z\s]+$";

        public const string PhoneNumber = @"^\d{10}$";
    }
}