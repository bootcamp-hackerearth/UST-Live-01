namespace HealthAxisHealth.Shared.Utilities
{

    public static class ValidationMessages
    {

        #region Patient

        public const string FullNameRequired =
            "Full name is required.";

        public const string InvalidFullNameFormat =
            "Full name can contain only letters and spaces.";

        public const string DateOfBirthRequired =
            "Date of birth is required.";

        public const string DateOfBirthCannotBeFuture =
            "Date of birth cannot be in the future.";

        public const string InvalidAge =
            "Age cannot be greater than 120 years.";

        public const string GenderRequired =
            "Gender is required.";

        public const string PhoneNumberRequired =
            "Phone number is required.";

        public const string InvalidPhoneNumberFormat =
            "Enter a valid 10-digit mobile number.";

        public const string EmailRequired =
            "Email is required.";

        public const string InvalidEmailFormat =
            "Invalid email format.";

        #endregion

        #region Doctor

        public const string DoctorNameRequired =
            "Doctor name is required.";

        public const string InvalidDoctorNameFormat =
            "Doctor name can contain only letters and spaces.";

        public const string SpecialisationRequired =
            "Specialisation is required.";

        public const string ExperienceRequired =
            "Years of experience is required.";

        public const string InvalidExperienceRange =
            "Years of experience must be between 0 and 50.";

        public const string ConsultationFeeRequired =
            "Consultation fee is required.";

        public const string InvalidConsultationFee =
            "Consultation fee must be between 100 and 10000.";

        #endregion

        #region Appointment

        public const string AppointmentDateRequired =
            "Appointment date is required.";

        public const string AppointmentDateInvalid =
            "Past dates are not allowed.";

        public const string ScheduledDateCannotBePast =
            "Scheduled date cannot be in the past.";

        public const string TimeSlotRequired =
            "Time slot is required.";

        public const string InvalidTimeSlot =
            "Invalid time slot.";

        public const string DoctorRequired =
            "Doctor is required.";

        public const string PatientRequired =
            "Patient is required.";

        public const string SlotAlreadyBooked =
            "Selected slot is already booked.";

        public const string AppointmentStatusRequired =
            "Appointment status is required.";

        public const string CancellationReasonRequired =
            "Cancellation reason is required.";

        #endregion

        #region Health Record

        public const string AppointmentRequired =
            "Appointment is required.";

        public const string DiagnosisRequired =
            "Diagnosis is required.";

        public const string PrescriptionRequired =
            "Prescription is required.";

        public const string NotesRequired =
            "Notes are required.";

        public const string VisitDateRequired =
            "Visit date is required.";

        #endregion

        #region Authentication

        public const string PasswordRequired =
            "Password is required.";

        public const string InvalidPasswordFormat =
            "Password must contain at least one uppercase letter, one lowercase letter and one digit.";

        public const string InvalidCredentials =
            "Invalid email or password.";

        public const string UserAlreadyExists =
            "User already exists.";

        public const string Unauthorized =
            "Unauthorized access.";

        public const string RefreshTokenRequired =
            "Refresh token is required.";

        public const string RoleRequired =
            "User role is required.";

        #endregion

        #region Common

        public const string RecordNotFound =
            "Record not found.";

        public const string InvalidId =
            "Invalid identifier.";

        public const string InvalidOperation =
            "Invalid operation.";

        public const string ActiveStatusRequired =
            "Active status is required.";

        public const string DataRequired =
            "Required data is missing.";

        #endregion
    }
}
