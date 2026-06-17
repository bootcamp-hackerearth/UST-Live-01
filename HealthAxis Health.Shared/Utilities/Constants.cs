namespace HealthAxisHealth.Shared.Utilities
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public static class Constants
    {
        #region Common

        public const string PatientNotFound = "Patient not found.";
        public const string DoctorNotFound = "Doctor not found.";
        public const string AppointmentNotFound = "Appointment not found.";
        public const string HealthRecordNotFound = "Health record not found.";
        public const string UserNotFound = "User not found.";
        public const string AssociatedUserNotFound = "Associated user not found.";

        public const string EmailAlreadyRegistered =
            "Email is already registered.";

        public const string EmailAlreadyExists =
            "Email already exists.";

        public const string InvalidCredentials =
            "Invalid credentials.";

        public const string InvalidRefreshToken =
            "Invalid refresh token.";

        public const string UserAccountInactive =
            "User account is inactive.";

        #endregion

        #region Appointment

        public const string AppointmentPastDate =
            "Appointments cannot be booked for past dates.";

        public const string AppointmentFutureLimit =
            "Appointments can only be booked up to 6 months in advance.";

        public const string InvalidTimeSlotFormat =
            "Invalid time slot format.";

        public const string DoctorUnavailable =
            "Doctor is not available for appointments.";

        public const string TimeSlotPassed =
            "The selected time slot has already passed.";

        public const string TimeSlotUnavailable =
            "Selected time slot is not available.";

        public const string AppointmentAlreadyConfirmed =
            "Appointment is already confirmed.";

        public const string AppointmentAlreadyCancelled =
            "Appointment is already cancelled.";

        public const string AppointmentAlreadyCompleted =
            "Appointment is already completed.";

        public const string CompletedCannotBeCancelled =
            "Completed appointments cannot be cancelled.";

        public const string CancelledCannotBeConfirmed =
            "Cancelled appointments cannot be confirmed.";

        public const string CancelledCannotBeCompleted =
            "Cancelled appointments cannot be completed.";

        public const string CompletedCannotBeModified =
            "Completed appointments cannot be modified.";

        public const string PendingMustBeConfirmed =
            "Pending appointments must be confirmed before completion.";

        public const string CancellationReasonRequired =
            "Cancellation reason is required.";

        public const string CompletedCannotBeDeleted =
            "Completed appointments cannot be deleted.";

        public const string ConfirmedCannotBeDeleted =
            "Confirmed appointments cannot be deleted.";

        #endregion

        #region Health Record

        public const string HealthRecordAlreadyExists =
            "A health record already exists for this appointment.";

        public const string CancelledAppointmentHealthRecord =
            "Cannot create a health record for a cancelled appointment.";

        public const string ConfirmedAppointmentRequired =
            "Health records can only be created for confirmed appointments.";

        public const string AppointmentDateRequired =
            "Health records can only be created on or after the appointment date.";

        #endregion

        #region Admin

        public const string DoctorHasUpcomingAppointments =
            "Cannot deactivate a doctor with upcoming appointments.";

        #endregion

        #region Authentication

        public const string UserRegisteredSuccessfully =
            "User registered successfully.";

        public const string LoggedOutSuccessfully =
            "Logged out successfully.";

        #endregion

        #region Responses

        public const string AppointmentBookedSuccessfully =
            "Appointment booked successfully.";

        public const string AppointmentStatusUpdatedSuccessfully =
            "Appointment status updated successfully.";

        public const string AppointmentDeletedSuccessfully =
            "Appointment deleted successfully.";

        public const string DoctorCreatedSuccessfully =
            "Doctor created successfully.";

        public const string DoctorUpdatedSuccessfully =
            "Doctor updated successfully.";

        public const string PatientUpdatedSuccessfully =
            "Patient updated successfully.";

        public const string HealthRecordCreatedSuccessfully =
            "Health record created successfully.";

        #endregion
    }
}
