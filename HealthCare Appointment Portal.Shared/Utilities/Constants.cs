using System.Diagnostics.CodeAnalysis;

namespace HealthCare_Appointment_Portal.Utilities
{

    [ExcludeFromCodeCoverage]
    public static class Constants
    {
        #region Validation Messages

        public const string FullNameRequired = "Full Name is required.";

        public const string InvalidFullNameFormat = "Full Name must contain only alphabets and spaces.";

        public const string DateOfBirthRequired = "Date Of Birth is required.";

        public const string DateOfBirthCannotBeFuture = "Date Of Birth cannot be in the future.";

        public const string GenderRequired = "Gender is required.";

        public const string PhoneNumberRequired = "Phone Number is required.";

        public const string InvalidPhoneNumberFormat =  "Phone Number must contain exactly 10 digits.";

        public const string EmailRequired = "Email is required.";

        public const string InvalidEmailFormat = "Please enter a valid email address.";

        public const string SpecialisationRequired = "Specialisation is required.";

        public const string InvalidExperienceRange = "Years Of Experience must be between 0 and 50.";

        public const string InvalidConsultationFee = "Consultation Fee must be between 0 and 10000.";

        public const string PatientRequired = "Patient details are required.";

        public const string DoctorRequired = "Doctor details are required.";

        public const string ScheduledDateRequired = "Scheduled Date is required.";

        public const string TimeSlotRequired = "Time Slot is required.";

        public const string AppointmentStatusRequired = "Appointment Status is required.";

        public const string VisitDateRequired = "Visit Date is required.";

        public const string DiagnosisRequired = "Diagnosis is required.";

        public const string PrescriptionRequired = "Prescription is required.";

        public const string ProviderNameRequired = "Provider Name is required.";

        public const string PolicyNumberRequired = "Policy Number is required.";

        public const string ExpiryDateRequired = "Expiry Date is required.";

        public const string InsuranceStatusRequired = "Insurance Status is required.";

        public const string PasswordRequired = "Password is required.";

        public const string UserRoleRequired = "User Role is required.";

        public const string ReferenceIdRequired = "Reference Id is required.";

        #endregion

        #region Duplicate Validation Messages

        public const string EmailAlreadyExists =  "Email already exists.";

        public const string PolicyNumberAlreadyExists = "Policy Number already exists.";

        public const string TimeSlotAlreadyBooked = "Selected time slot is already booked.";

        public const string HealthRecordAlreadyExists = "Health record already exists for this appointment.";

        #endregion

        #region Business Rule Messages

        public const string DoctorInactive = "Appointment cannot be booked with an inactive doctor.";

        public const string PastDateNotAllowed = "Past dates are not allowed.";

        public const string InvalidTimeSlot = "Invalid time slot selected.";

        public const string CompletedHealthRecord = "Health record can be added only for completed appointments.";

        #endregion

        #region Cancellation Reasons

        public const string DoctorRemovedFromSystem = "Doctor removed from system.";

        public const string PatientRemovedFromSystem = "Patient removed from system.";

        #endregion

        #region Exception Messages

        public const string ConfirmOnlyPending = "Only pending appointments can be confirmed.";

        public const string CancelOnlyPendingOrConfirmed = "Only pending or confirmed appointments can be cancelled.";

        public const string CompleteOnlyConfirmed = "Only confirmed appointments can be completed.";

        #endregion

        #region MVC Constants

        public const string ErrorKey =
            "Error";

        public const string SuccessKey =
            "Success";

        public const string ReferenceIdKey =
            "ReferenceId";

        public const string LoginAction =
            "Login";

        public const string DetailsAction =
            "Details";

        public const string IndexAction =
            "Index";

        public const string DashboardAction =
            "Dashboard";

        public const string CreateAction =
            "Create";

        public const string MyProfileAction =
            "MyProfile";

        public const string UserController =
            "User";

        public const string PatientController =
            "Patient";

        public const string DoctorController =
            "Doctor";

        public const string HealthRecordController =
            "HealthRecord";

        public const string EditMyProfileAction =
            "EditMyProfile";

        #endregion
    }
}
