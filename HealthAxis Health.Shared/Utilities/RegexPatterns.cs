namespace HealthAxisHealth.Shared.Utilities
{
    public static class RegexPatterns
    {
        #region Patient

        public const string FullName =
            @"^[a-zA-Z\s]+$";

        public const string PhoneNumber =
            @"^[6-9]\d{9}$";

        #endregion

        #region Authentication

        public const string Password =
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,100}$";

        #endregion
    }
}
