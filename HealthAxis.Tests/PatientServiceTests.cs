using AutoMapper;
using FluentAssertions;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Implementation;
using HealthAxis.Shared.DTO.PatientDtos;
using HealthAxis.Shared.Enums;
using Moq;

using ApiValidationException = HealthAxis.API.Exceptions.ValidationException;

namespace HealthAxis.API.Tests.Services
{
    public class PatientServiceTests
    {
        private static readonly int[] ExpectedPatientIds = { 1, 2 };

        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IHealthRecordRepository> _healthRecordRepositoryMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _healthRecordRepositoryMock = new Mock<IHealthRecordRepository>();
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _mapperMock = new Mock<IMapper>();

            _mapperMock
                .Setup(mapper => mapper.Map<PatientDto>(It.IsAny<Patient>()))
                .Returns((Patient patient) => MapPatientDto(patient));

            _mapperMock
                .Setup(mapper => mapper.Map<List<PatientDto>>(It.IsAny<object>()))
                .Returns((object source) =>
                {
                    var patients = source as IEnumerable<Patient> ?? new List<Patient>();
                    return patients.Select(MapPatientDto).ToList();
                });

            _service = new PatientService(
                _patientRepositoryMock.Object,
                _doctorRepositoryMock.Object,
                _healthRecordRepositoryMock.Object,
                _appointmentRepositoryMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_WhenPatientsExist_ReturnsPatientDtos()
        {
            var patients = new List<Patient>
            {
                CreatePatient(id: 1, fullName: "Mona"),
                CreatePatient(id: 2, fullName: "Riya")
            };

            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(patients);

            var result = await _service.GetAllAsync();

            result.Should().HaveCount(2);
            result[0].PatientId.Should().Be(1);
            result[0].FullName.Should().Be("Mona");
            result[1].PatientId.Should().Be(2);
            result[1].FullName.Should().Be("Riya");

            _patientRepositoryMock.Verify(repository => repository.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoPatients_ReturnsEmptyList()
        {
            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Patient>());

            var result = await _service.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().BeEmpty();

            _patientRepositoryMock.Verify(repository => repository.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenPatientExists_ReturnsPatientDto()
        {
            var patient = CreatePatient(id: 1, fullName: "Mona");

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(patient);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.PatientId.Should().Be(1);
            result.FullName.Should().Be("Mona");

            _patientRepositoryMock.Verify(repository => repository.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenPatientDoesNotExist_ThrowsNotFoundException()
        {
            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(99))
                .ReturnsAsync((Patient?)null);

            Func<Task> act = async () => await _service.GetByIdAsync(99);

            await act.Should().ThrowAsync<NotFoundException>();

            _patientRepositoryMock.Verify(repository => repository.GetByIdAsync(99), Times.Once);
        }

        [Fact]
        public async Task GetByUserIdAsync_WhenPatientProfileExists_ReturnsPatientDto()
        {
            var patients = new List<Patient>
            {
                CreatePatient(id: 1, fullName: "Mona", userId: "patient-user-1"),
                CreatePatient(id: 2, fullName: "Riya", userId: "patient-user-2")
            };

            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(patients);

            var result = await _service.GetByUserIdAsync("patient-user-2");

            result.Should().NotBeNull();
            result!.PatientId.Should().Be(2);
            result.FullName.Should().Be("Riya");

            _patientRepositoryMock.Verify(repository => repository.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByUserIdAsync_WhenPatientsListIsEmpty_ThrowsNotFoundException()
        {
            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Patient>());

            Func<Task> act = async () => await _service.GetByUserIdAsync("missing-user");

            await act.Should().ThrowAsync<NotFoundException>();

            _patientRepositoryMock.Verify(repository => repository.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByUserIdAsync_WhenMatchingPatientDoesNotExist_ThrowsNotFoundException()
        {
            var patients = new List<Patient>
            {
                CreatePatient(id: 1, fullName: "Mona", userId: "patient-user-1")
            };

            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(patients);

            Func<Task> act = async () => await _service.GetByUserIdAsync("wrong-user");

            await act.Should().ThrowAsync<NotFoundException>();

            _patientRepositoryMock.Verify(repository => repository.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenPatientDoesNotExist_ThrowsNotFoundException()
        {
            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync((Patient?)null);

            var dto = CreateUpdatePatientDto();

            Func<Task> act = async () => await _service.UpdateAsync(1, dto);

            await act.Should().ThrowAsync<NotFoundException>();

            _patientRepositoryMock.Verify(repository => repository.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenEmailAlreadyRegisteredByAnotherPatient_ThrowsValidationException()
        {
            var existingPatient = CreatePatient(id: 1, email: "mona@gmail.com", phone: "1111111111");
            var otherPatient = CreatePatient(id: 2, email: "other@gmail.com", phone: "2222222222");

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(existingPatient);

            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Patient> { existingPatient, otherPatient });

            var dto = CreateUpdatePatientDto(email: "OTHER@gmail.com", phone: "9999999999");

            Func<Task> act = async () => await _service.UpdateAsync(1, dto);

            await act.Should().ThrowAsync<ApiValidationException>();

            _patientRepositoryMock.Verify(repository => repository.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenPhoneAlreadyRegisteredByAnotherPatient_ThrowsValidationException()
        {
            var existingPatient = CreatePatient(id: 1, email: "mona@gmail.com", phone: "1111111111");
            var otherPatient = CreatePatient(id: 2, email: "other@gmail.com", phone: "2222222222");

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(existingPatient);

            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Patient> { existingPatient, otherPatient });

            var dto = CreateUpdatePatientDto(email: "updated@gmail.com", phone: "2222222222");

            Func<Task> act = async () => await _service.UpdateAsync(1, dto);

            await act.Should().ThrowAsync<ApiValidationException>();

            _patientRepositoryMock.Verify(repository => repository.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenSameEmailAndPhoneBelongToSamePatient_AllowsUpdate()
        {
            var existingPatient = CreatePatient(
                id: 1,
                fullName: "Mona",
                email: "mona@gmail.com",
                phone: "9876543210");

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(existingPatient);

            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Patient> { existingPatient });

            _patientRepositoryMock
                .Setup(repository => repository.UpdateAsync(1, It.IsAny<Patient>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int _, Patient patient, CancellationToken _) => patient);

            var dto = CreateUpdatePatientDto(
                fullName: "Mona Updated",
                email: "MONA@gmail.com",
                phone: "9876543210");

            var result = await _service.UpdateAsync(1, dto);

            result.Should().NotBeNull();
            result!.FullName.Should().Be("Mona Updated");
            result.Email.Should().Be("MONA@gmail.com");
            result.PhoneNumber.Should().Be("9876543210");

            _patientRepositoryMock.Verify(repository => repository.UpdateAsync(1, It.IsAny<Patient>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenValidData_UpdatesPatientAndReturnsDto()
        {
            var existingPatient = CreatePatient(
                id: 1,
                fullName: "Mona",
                email: "mona@gmail.com",
                phone: "9876543210");

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(existingPatient);

            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Patient> { existingPatient });

            _patientRepositoryMock
                .Setup(repository => repository.UpdateAsync(1, It.IsAny<Patient>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((int _, Patient patient, CancellationToken _) => patient);

            var dto = CreateUpdatePatientDto(
                fullName: "Mona Updated",
                email: "updated@gmail.com",
                phone: "9999999999");

            var result = await _service.UpdateAsync(1, dto);

            result.Should().NotBeNull();
            result!.PatientId.Should().Be(1);
            result.FullName.Should().Be("Mona Updated");
            result.Email.Should().Be("updated@gmail.com");
            result.PhoneNumber.Should().Be("9999999999");

            _patientRepositoryMock.Verify(repository => repository.UpdateAsync(
                1,
                It.Is<Patient>(patient =>
                    patient.FullName == "Mona Updated" &&
                    patient.Email == "updated@gmail.com" &&
                    patient.PhoneNumber == "9999999999"),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetHealthRecordsByPatientIdAsync_WhenPatientDoesNotExist_ThrowsNotFoundException()
        {
            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync((Patient?)null);

            Func<Task> act = async () => await _service.GetHealthRecordsByPatientIdAsync(1);

            await act.Should().ThrowAsync<NotFoundException>();

            _patientRepositoryMock.Verify(repository => repository.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetHealthRecordsByPatientIdAsync_WhenPatientHasNoRecords_ReturnsEmptyList()
        {
            var patient = CreatePatient(id: 1, fullName: "Mona");

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<HealthRecord>());

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Doctor>());

            var result = await _service.GetHealthRecordsByPatientIdAsync(1);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetHealthRecordsByPatientIdAsync_WhenRecordsExist_ReturnsOnlyPatientRecordsInDescendingOrder()
        {
            var patient = CreatePatient(id: 1, fullName: "Mona");

            var records = new List<HealthRecord>
            {
                CreateHealthRecord(id: 1, patientId: 1, doctorId: 1, visitDate: DateTime.Today.AddDays(-5)),
                CreateHealthRecord(id: 2, patientId: 1, doctorId: 1, visitDate: DateTime.Today),
                CreateHealthRecord(id: 3, patientId: 2, doctorId: 1, visitDate: DateTime.Today.AddDays(1))
            };

            var doctors = new List<Doctor>
            {
                CreateDoctor(id: 1, fullName: "Dr John")
            };

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(records);

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(doctors);

            var result = await _service.GetHealthRecordsByPatientIdAsync(1);

            result.Should().HaveCount(2);
            result[0].HealthRecordId.Should().Be(2);
            result[0].RecordId.Should().Be(2);
            result[0].PatientId.Should().Be(1);
            result[0].PatientName.Should().Be("Mona");
            result[0].DoctorName.Should().Be("Dr John");
            result[0].Specialisation.Should().Be(Specialisation.Cardiology.ToString());

            result[1].HealthRecordId.Should().Be(1);
            result[1].RecordId.Should().Be(1);
        }

        [Fact]
        public async Task GetHealthRecordsByPatientIdAsync_WhenDoctorMissing_UsesFallbackValues()
        {
            var patient = CreatePatient(id: 1, fullName: "Mona");

            var records = new List<HealthRecord>
            {
                CreateHealthRecord(id: 1, patientId: 1, doctorId: 99, visitDate: DateTime.Today)
            };

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(records);

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Doctor>());

            var result = await _service.GetHealthRecordsByPatientIdAsync(1);

            result.Should().HaveCount(1);
            result[0].DoctorName.Should().Be("Doctor not assigned");
            result[0].Specialisation.Should().Be("Not assigned");
        }

        [Fact]
        public async Task GetHealthRecordsByPatientIdAsync_WhenUpdatedDateExists_MapsUpdatedDate()
        {
            var patient = CreatePatient(id: 1, fullName: "Mona");

            var updatedDate = DateTime.UtcNow;

            var records = new List<HealthRecord>
            {
                CreateHealthRecord(
                    id: 1,
                    patientId: 1,
                    doctorId: 1,
                    visitDate: DateTime.Today,
                    updatedDate: updatedDate)
            };

            var doctors = new List<Doctor>
            {
                CreateDoctor(id: 1, fullName: "Dr John")
            };

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _healthRecordRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(records);

            _doctorRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(doctors);

            var result = await _service.GetHealthRecordsByPatientIdAsync(1);

            result[0].UpdatedDate.Should().Be(updatedDate);
        }

        [Fact]
        public async Task GetPatientsForDoctorAsync_WhenDoctorHasAppointments_ReturnsDistinctPatients()
        {
            var appointments = new List<Appointment>
            {
                CreateAppointment(id: 1, doctorId: 1, patientId: 1),
                CreateAppointment(id: 2, doctorId: 1, patientId: 1),
                CreateAppointment(id: 3, doctorId: 1, patientId: 2),
                CreateAppointment(id: 4, doctorId: 2, patientId: 3)
            };

            var patients = new List<Patient>
            {
                CreatePatient(id: 1, fullName: "Mona"),
                CreatePatient(id: 2, fullName: "Riya"),
                CreatePatient(id: 3, fullName: "Asha")
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(patients);

            var result = await _service.GetPatientsForDoctorAsync(1);

            result.Should().HaveCount(2);
            result.Select(patient => patient.PatientId).Should().Contain(ExpectedPatientIds);
            result.Select(patient => patient.PatientId).Should().NotContain(3);
        }

        [Fact]
        public async Task GetPatientsForDoctorAsync_WhenDoctorHasNoAppointments_ReturnsEmptyList()
        {
            var appointments = new List<Appointment>
            {
                CreateAppointment(id: 1, doctorId: 2, patientId: 1)
            };

            var patients = new List<Patient>
            {
                CreatePatient(id: 1, fullName: "Mona")
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(patients);

            var result = await _service.GetPatientsForDoctorAsync(1);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetPatientsForDoctorAsync_WhenNoAppointmentsExist_ReturnsEmptyList()
        {
            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            _patientRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(new List<Patient>
                {
                    CreatePatient(id: 1, fullName: "Mona")
                });

            var result = await _service.GetPatientsForDoctorAsync(1);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetPatientForDoctorAsync_WhenAppointmentExistsAndPatientExists_ReturnsPatient()
        {
            var appointments = new List<Appointment>
            {
                CreateAppointment(id: 1, doctorId: 1, patientId: 1)
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync(CreatePatient(id: 1, fullName: "Mona"));

            var result = await _service.GetPatientForDoctorAsync(1, 1);

            result.Should().NotBeNull();
            result!.PatientId.Should().Be(1);
            result.FullName.Should().Be("Mona");

            _patientRepositoryMock.Verify(repository => repository.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetPatientForDoctorAsync_WhenNoAppointmentWithDoctor_ReturnsNull()
        {
            var appointments = new List<Appointment>
            {
                CreateAppointment(id: 1, doctorId: 2, patientId: 1)
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            var result = await _service.GetPatientForDoctorAsync(1, 1);

            result.Should().BeNull();

            _patientRepositoryMock.Verify(repository => repository.GetByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetPatientForDoctorAsync_WhenAppointmentExistsButPatientMissing_ReturnsNull()
        {
            var appointments = new List<Appointment>
            {
                CreateAppointment(id: 1, doctorId: 1, patientId: 1)
            };

            _appointmentRepositoryMock
                .Setup(repository => repository.GetAllAsync())
                .ReturnsAsync(appointments);

            _patientRepositoryMock
                .Setup(repository => repository.GetByIdAsync(1))
                .ReturnsAsync((Patient?)null);

            var result = await _service.GetPatientForDoctorAsync(1, 1);

            result.Should().BeNull();

            _patientRepositoryMock.Verify(repository => repository.GetByIdAsync(1), Times.Once);
        }

        private static Patient CreatePatient(
            int id = 1,
            string fullName = "Mona",
            string email = "mona@gmail.com",
            string phone = "9876543210",
            string userId = "patient-user-1")
        {
            return new Patient
            {
                PatientId = id,
                FullName = fullName,
                DateOfBirth = new DateTime(2004, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = phone,
                Email = email,
                UserId = userId,
                CreatedDate = DateTime.Today
            };
        }

        private static UpdatePatientDto CreateUpdatePatientDto(
            string fullName = "Mona Updated",
            string email = "updated@gmail.com",
            string phone = "9999999999")
        {
            return new UpdatePatientDto
            {
                FullName = fullName,
                DateOfBirth = new DateTime(2004, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = phone,
                Email = email
            };
        }

        private static Doctor CreateDoctor(
            int id = 1,
            string fullName = "Dr John")
        {
            return new Doctor
            {
                DoctorId = id,
                FullName = fullName,
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true,
                UserId = "doctor-user-1"
            };
        }

        private static HealthRecord CreateHealthRecord(
            int id = 1,
            int appointmentId = 1,
            int patientId = 1,
            int doctorId = 1,
            DateTime? visitDate = null,
            DateTime? updatedDate = null)
        {
            return new HealthRecord
            {
                HealthRecordId = id,
                AppointmentId = appointmentId,
                PatientId = patientId,
                DoctorId = doctorId,
                VisitDate = visitDate ?? DateTime.Today,
                Diagnosis = "Fever",
                Prescription = "Tablet",
                Notes = "Take rest",
                UpdatedDate = updatedDate
            };
        }

        private static Appointment CreateAppointment(
            int id = 1,
            int doctorId = 1,
            int patientId = 1)
        {
            return new Appointment
            {
                AppointmentId = id,
                DoctorId = doctorId,
                PatientId = patientId,
                ScheduledDate = DateTime.Today.AddDays(1),
                TimeSlot = "10:00 AM - 11:00 AM",
                Status = AppointmentStatus.Pending
            };
        }

        private static PatientDto MapPatientDto(Patient patient)
        {
            return new PatientDto
            {
                PatientId = patient.PatientId,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,
                PhoneNumber = patient.PhoneNumber,
                Email = patient.Email
            };
        }
    }
}