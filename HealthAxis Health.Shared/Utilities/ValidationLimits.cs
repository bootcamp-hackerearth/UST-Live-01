namespace HealthAxisHealth.Shared.Utilities
{
    public static class ValidationLimits
    {
        #region Patient

        public const int FullNameLength = 100;

        public const int EmailLength = 100;

        public const int PhoneNumberLength = 10;

        public const int MinimumAge = 0;

        public const int MaximumAge = 120;

        #endregion

        #region Doctor

        public const int DoctorNameLength = 100;

        public const int SpecialisationLength = 100;

        public const int MinExperience = 0;

        public const int MaxExperience = 50;

        public const string MinConsultationFee = "100";

        public const string MaxConsultationFee = "10000";

        #endregion

        #region Appointment

        public const int TimeSlotLength = 20;

        public const int CancellationReasonLength = 500;

        #endregion

        #region HealthRecord

        public const int DiagnosisLength = 500;

        public const int PrescriptionLength = 500;

        public const int NotesLength = 1000;

        #endregion

        #region Authentication

        public const int PasswordMinLength = 6;

        public const int PasswordMaxLength = 100;

        public const int RefreshTokenLength = 500;

        #endregion

        #region Common

        public const int StatusLength = 50;

        public const int NameLength = 100;

        public const int DescriptionLength = 1000;

        #endregion
    }
}
