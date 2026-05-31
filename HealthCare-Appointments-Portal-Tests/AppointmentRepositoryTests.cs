using HealthCare_Appointments_Portal.Data;
using HealthCare_Appointments_Portal.Enums;
using HealthCare_Appointments_Portal.Models;
using HealthCare_Appointments_Portal.Repositories;

namespace HealthCare_Appointments_Portal.Tests
{
    public class AppointmentRepositoryTests
    {
        private readonly DataStore _dataStore;

        private readonly AppointmentRepository _repository;

        public AppointmentRepositoryTests()
        {
            _dataStore = new DataStore();

            _repository =
                new AppointmentRepository(
                    _dataStore);
        }

        // Add Appointment Success
        [Fact]
        public void AddAppointment_ValidAppointment_ShouldAddAppointment()
        {
            // Arrange
            Appointment appointment = CreateAppointment();

            // Act
            _repository.AddAppointment(
                appointment);

            // Assert
            Assert.Single(
                _dataStore.Appointments);

            Assert.Equal(
                appointment.AppointmentId,
                _dataStore
                    .Appointments[0]
                    .AppointmentId);
        }

        // Get Appointment By Existing Id
        [Fact]
        public void GetAppointmentById_ExistingId_ShouldReturnAppointment()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            _dataStore
                .Appointments
                .Add(appointment);

            // Act
            Appointment? result =
                _repository.GetAppointmentById(
                    appointment.AppointmentId);

            // Assert
            Assert.NotNull(
                result);

            Assert.Equal(
                appointment.AppointmentId,
                result?.AppointmentId);
        }

        // Get Appointment By Invalid Id
        [Fact]
        public void GetAppointmentById_InvalidId_ShouldReturnNull()
        {
            // Act
            Appointment? result =
                _repository.GetAppointmentById(
                    43);

            // Assert
            Assert.Null(
                result);
        }

        // Get All Appointments
        [Fact]
        public void GetAllAppointments_ShouldReturnAllAppointments()
        {
            // Arrange
            _dataStore.Appointments.AddRange(
            [
                CreateAppointment(),
                CreateAppointment()
            ]);

            // Act
            List<Appointment> result =
                _repository.GetAllAppointments();

            // Assert
            Assert.Equal(
                2,
                result.Count);
        }

        // Get All Appointments Empty
        [Fact]
        public void GetAllAppointments_EmptyList_ShouldReturnEmpty()
        {
            // Act
            List<Appointment> result =
                _repository.GetAllAppointments();

            // Assert
            Assert.Empty(
                result);
        }

        // Update Existing Appointment
        [Fact]
        public void UpdateAppointment_ExistingAppointment_ShouldUpdateDetails()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            _dataStore
                .Appointments
                .Add(appointment);

            Patient updatedPatient = new()
            {
                FullName =
                    "Updated Patient",

                DateOfBirth =
                    new DateOnly(
                        2001,
                        4,
                        22),

                Gender =
                    Gender.Male,

                PhoneNumber =
                    "9999999999",

                Email =
                    "updated@gmail.com",

                InsuranceId =
                    "INS101"
            };

            Doctor updatedDoctor = new()
            {
                FullName =
                    "Updated Doctor",

                Specialisation =
                    Specialisation.Neurology,

                YearsOfExperience =
                    10,

                ConsultationFee =
                    2000,

                IsActive =
                    true
            };

            Appointment updatedAppointment = new()
            {
                AppointmentId =
                    appointment.AppointmentId,

                Patient =
                    updatedPatient,

                Doctor =
                    updatedDoctor,

                ScheduledDate =
                    new DateOnly(
                        2026,
                        5,
                        20),

                TimeSlot =
                    new TimeOnly(
                        11,
                        30),

                Status =
                    AppointmentStatus.Confirmed,

                CancellationReason =
                    "Updated"
            };

            // Act
            _repository.UpdateAppointment(
                updatedAppointment);

            // Assert
            Appointment? result =
                _repository.GetAppointmentById(
                    appointment.AppointmentId);

            Assert.NotNull(
                result);

            Assert.Equal(
                "Updated Patient",
                result?.Patient.FullName);

            Assert.Equal(
                "Updated Doctor",
                result?.Doctor.FullName);

            Assert.Equal(
                AppointmentStatus.Confirmed,
                result?.Status);
        }

        // Partial Update Appointment
        [Fact]
        public void UpdateAppointment_PartialUpdate_ShouldUpdateOnlyProvidedFields()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            _dataStore
                .Appointments
                .Add(appointment);

            Appointment updatedAppointment = new()
            {
                AppointmentId =
                    appointment.AppointmentId,

                Patient =
                    appointment.Patient,

                Doctor =
                    appointment.Doctor,

                Status =
                    AppointmentStatus.Completed
            };

            // Act
            _repository.UpdateAppointment(
                updatedAppointment);

            // Assert
            Appointment? result =
                _repository.GetAppointmentById(
                    appointment.AppointmentId);

            Assert.NotNull(
                result);

            // Updated Field
            Assert.Equal(
                AppointmentStatus.Completed,
                result?.Status);

            // Existing Fields Unchanged
            Assert.Equal(
                appointment.Patient.FullName,
                result?.Patient.FullName);

            Assert.Equal(
                appointment.Doctor.FullName,
                result?.Doctor.FullName);
        }

        // Update Invalid Appointment
        [Fact]
        public void UpdateAppointment_InvalidId_ShouldNotUpdate()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            // Act
            _repository.UpdateAppointment(
                appointment);

            // Assert
            Assert.Empty(
                _dataStore.Appointments);
        }

        // Delete Existing Appointment
        [Fact]
        public void DeleteAppointmentById_ExistingId_ShouldRemoveAppointment()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            _dataStore
                .Appointments
                .Add(appointment);

            // Act
            _repository.DeleteAppointmentById(
                appointment.AppointmentId);

            // Assert
            Assert.Empty(
                _dataStore.Appointments);
        }

        // Delete Invalid Appointment
        [Fact]
        public void DeleteAppointmentById_InvalidId_ShouldNotRemoveAnything()
        {
            // Arrange
            Appointment appointment =
                CreateAppointment();

            _dataStore
                .Appointments
                .Add(appointment);

            // Act
            _repository.DeleteAppointmentById(
                32);

            // Assert
            Assert.Single(
                _dataStore.Appointments);
        }

        // GetAppointmentsByPatientId
        [Fact]
        public void GetAppointmentsByPatientId_ExistingPatient_ShouldReturnAppointments()
        {
            // Arrange
            Appointment appointment1 = CreateAppointment();
            appointment1.Patient.PatientId = 1;

            Appointment appointment2 = CreateAppointment();
            appointment2.Patient.PatientId = 2;

            _dataStore.Appointments.Add(appointment1);
            _dataStore.Appointments.Add(appointment2);

            // Act
            List<Appointment> result =
                _repository.GetAppointmentsByPatientId(1);

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result[0].Patient.PatientId);
        }

        // GetAppointmentsByDoctorId
        [Fact]
        public void GetAppointmentsByDoctorId_ExistingDoctor_ShouldReturnAppointments()
        {
            // Arrange
            Appointment appointment1 = CreateAppointment();
            appointment1.Doctor.DoctorId = 1;

            Appointment appointment2 = CreateAppointment();
            appointment2.Doctor.DoctorId = 2;

            _dataStore.Appointments.Add(appointment1);
            _dataStore.Appointments.Add(appointment2);

            // Act
            List<Appointment> result =
                _repository.GetAppointmentsByDoctorId(1);

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result[0].Doctor.DoctorId);
        }

        // GetConflictingAppointment - Conflict Exists
        [Fact]
        public void GetConflictingAppointment_ConflictExists_ShouldReturnAppointment()
        {
            // Arrange
            Appointment appointment = CreateAppointment();

            appointment.Doctor.DoctorId = 1;
            appointment.ScheduledDate =
                new DateOnly(2026, 5, 10);
            appointment.TimeSlot =
                new TimeOnly(10, 0);

            _dataStore.Appointments.Add(appointment);

            // Act
            Appointment? result =
                _repository.GetConflictingAppointment(
                    1,
                    new DateOnly(2026, 5, 10),
                    new TimeOnly(10, 0));

            // Assert
            Assert.NotNull(result);
        }

        // GetConflictingAppointment - Cancelled Appointment
        [Fact]
        public void GetConflictingAppointment_CancelledAppointment_ShouldReturnNull()
        {
            // Arrange
            Appointment appointment = CreateAppointment();

            appointment.Doctor.DoctorId = 1;
            appointment.Status =
                AppointmentStatus.Cancelled;

            _dataStore.Appointments.Add(appointment);

            // Act
            Appointment? result =
                _repository.GetConflictingAppointment(
                    1,
                    appointment.ScheduledDate,
                    appointment.TimeSlot);

            // Assert
            Assert.Null(result);
        }

        // GetUpcomingAppointments
        [Fact]
        public void GetUpcomingAppointments_ShouldReturnConfirmedAppointmentsOnly()
        {
            // Arrange
            Appointment confirmedAppointment =
                CreateAppointment();

            confirmedAppointment.ScheduledDate =
                DateOnly.FromDateTime(
                    DateTime.Now.AddDays(1));

            confirmedAppointment.Status =
                AppointmentStatus.Confirmed;

            Appointment completedAppointment =
                CreateAppointment();

            completedAppointment.ScheduledDate =
                DateOnly.FromDateTime(
                    DateTime.Now.AddDays(1));

            completedAppointment.Status =
                AppointmentStatus.Completed;

            _dataStore.Appointments.Add(
                confirmedAppointment);

            _dataStore.Appointments.Add(
                completedAppointment);

            // Act
            List<Appointment> result =
                _repository.GetUpcomingAppointments();

            // Assert
            Assert.Single(result);

            Assert.Equal(
                AppointmentStatus.Confirmed,
                result[0].Status);
        }

        // GetCompletedAppointments
        [Fact]
        public void GetCompletedAppointments_ShouldReturnCompletedAppointmentsOnly()
        {
            // Arrange
            Appointment completedAppointment =
                CreateAppointment();

            completedAppointment.Status =
                AppointmentStatus.Completed;

            Appointment pendingAppointment =
                CreateAppointment();

            pendingAppointment.Status =
                AppointmentStatus.Pending;

            _dataStore.Appointments.Add(
                completedAppointment);

            _dataStore.Appointments.Add(
                pendingAppointment);

            // Act
            List<Appointment> result =
                _repository.GetCompletedAppointments();

            // Assert
            Assert.Single(result);

            Assert.Equal(
                AppointmentStatus.Completed,
                result[0].Status);
        }

        [Fact]
        public void GetAppointmentsByPatientId_ShouldReturnAppointmentsInDateOrder()
        {
            // Arrange
            Appointment olderAppointment =
                CreateAppointment();

            olderAppointment.Patient.PatientId = 1;

            olderAppointment.ScheduledDate =
                new DateOnly(
                    2026,
                    1,
                    1);

            Appointment newerAppointment =
                CreateAppointment();

            newerAppointment.Patient.PatientId = 1;

            newerAppointment.ScheduledDate =
                new DateOnly(
                    2026,
                    12,
                    1);

            _dataStore.Appointments.Add(
                newerAppointment);

            _dataStore.Appointments.Add(
                olderAppointment);

            // Act
            List<Appointment> result =
                _repository.GetAppointmentsByPatientId(
                    1);

            // Assert
            Assert.Equal(
                new DateOnly(
                    2026,
                    1,
                    1),
                result[0].ScheduledDate);

            Assert.Equal(
                new DateOnly(
                    2026,
                    12,
                    1),
                result[1].ScheduledDate);
        }

        [Fact]
        public void GetAppointmentsByDoctorId_ShouldReturnAppointmentsInDateOrder()
        {
            // Arrange
            Appointment olderAppointment =
                CreateAppointment();

            olderAppointment.Doctor.DoctorId = 1;

            olderAppointment.ScheduledDate =
                new DateOnly(
                    2026,
                    1,
                    1);

            Appointment newerAppointment =
                CreateAppointment();

            newerAppointment.Doctor.DoctorId = 1;

            newerAppointment.ScheduledDate =
                new DateOnly(
                    2026,
                    12,
                    1);

            _dataStore.Appointments.Add(
                newerAppointment);

            _dataStore.Appointments.Add(
                olderAppointment);

            // Act
            List<Appointment> result =
                _repository.GetAppointmentsByDoctorId(
                    1);

            // Assert
            Assert.Equal(
                new DateOnly(
                    2026,
                    1,
                    1),
                result[0].ScheduledDate);

            Assert.Equal(
                new DateOnly(
                    2026,
                    12,
                    1),
                result[1].ScheduledDate);
        }

        [Fact]
        public void GetAppointmentsByPatientId_ShouldReturnAppointmentsSortedByDate()
        {
            // Arrange
            Appointment appointment1 =
                CreateAppointment();

            appointment1.Patient.PatientId = 1;
            appointment1.ScheduledDate =
                new DateOnly(2026, 12, 1);

            Appointment appointment2 =
                CreateAppointment();

            appointment2.Patient.PatientId = 1;
            appointment2.ScheduledDate =
                new DateOnly(2026, 1, 1);

            _dataStore.Appointments.Add(appointment1);
            _dataStore.Appointments.Add(appointment2);

            // Act
            List<Appointment> result =
                _repository.GetAppointmentsByPatientId(1);

            // Assert
            Assert.Equal(2, result.Count);

            Assert.Equal(
                new DateOnly(2026, 1, 1),
                result[0].ScheduledDate);

            Assert.Equal(
                new DateOnly(2026, 12, 1),
                result[1].ScheduledDate);
        }

        [Fact]
        public void GetUpcomingAppointments_ShouldReturnAppointmentsSortedByDate()
        {
            // Arrange
            Appointment laterAppointment =
                CreateAppointment();

            laterAppointment.Status =
                AppointmentStatus.Confirmed;

            laterAppointment.ScheduledDate =
                DateOnly.FromDateTime(
                    DateTime.Now.AddDays(10));

            Appointment earlierAppointment =
                CreateAppointment();

            earlierAppointment.Status =
                AppointmentStatus.Confirmed;

            earlierAppointment.ScheduledDate =
                DateOnly.FromDateTime(
                    DateTime.Now.AddDays(1));

            _dataStore.Appointments.Add(
                laterAppointment);

            _dataStore.Appointments.Add(
                earlierAppointment);

            // Act
            List<Appointment> result =
                _repository.GetUpcomingAppointments();

            // Assert
            Assert.Equal(
                2,
                result.Count);

            Assert.Equal(
                earlierAppointment.ScheduledDate,
                result[0].ScheduledDate);

            Assert.Equal(
                laterAppointment.ScheduledDate,
                result[1].ScheduledDate);
        }

        // Helper Method
        private static Appointment CreateAppointment()
        {
            Patient patient = new()
            {
                FullName =
                    "Ragu",

                DateOfBirth =
                    new DateOnly(
                        2001,
                        4,
                        22),

                Gender =
                    Gender.Male,

                PhoneNumber =
                    "9876543210",

                Email =
                    "ragu@gmail.com",

                InsuranceId =
                    "INS101"
            };

            Doctor doctor = new()
            {
                FullName =
                    "Dr Arun",

                Specialisation =
                    Specialisation.Cardiology,

                YearsOfExperience =
                    5,

                ConsultationFee =
                    1000,

                IsActive =
                    true
            };

            return new Appointment
            {
                Patient =
                    patient,

                Doctor =
                    doctor,

                ScheduledDate =
                    new DateOnly(
                        2026,
                        5,
                        10),

                TimeSlot =
                    new TimeOnly(
                        10,
                        0),

                Status =
                    AppointmentStatus.Pending
            };
        }
    }
}