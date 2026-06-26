namespace HealthAxis.API.Utilities
{
    public static class RegexPatterns
    {
        public const string FullName =
            @"^[A-Za-z]+(?:[ .'’-][A-Za-z]+)*$";

        public const string PhoneNumber =
            @"^[6-9]\d{9}$";

        public const string StrictEmail =
            @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
    }
}