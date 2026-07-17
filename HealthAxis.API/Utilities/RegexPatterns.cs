namespace HealthAxis.API.Utilities
{
    public static class RegexPatterns
    {
        public const string FullName = @"^[a-zA-Z\s]+$";

        public const string PhoneNumber = @"^\d{10}$";
    }
}