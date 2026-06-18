using AutoMapper;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Repositories.Interfaces;
using HealthAxisCore_Api.Services.Implementation;
using Moq;
using Xunit;

namespace HealthAxisCore_Api.Tests.Services
{
    public class PatientServiceTests
    {
        private static PatientService CreateService(
            Mock<IPatientRepository>? repositoryMock = null,
            Mock<IMapper>? mapperMock = null)
        {
            return new PatientService(
                repositoryMock?.Object ?? new Mock<IPatientRepository>().Object,
                mapperMock?.Object ?? new Mock<IMapper>().Object);
        }

        private static Patient CreatePatient(
            int patientId = 1,
            string patientName = "Patient One",
            string gender = "Male",
            string email = "patient@test.com",
            string phoneNumber = "9876543210",
            string? insuranceId = "INS001",
            bool isActive = true)
        {
            return new Patient
            {
                PatientId = patientId,
                PatientName = patientName,
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = gender,
                Email = email,
                PhoneNumber = phoneNumber,
                InsuranceID = insuranceId,
                IsActive = isActive
            };
        }

        private static PatientDto CreatePatientDto(
            int patientId = 1,
            string patientName = "Patient One",
            string gender = "Male",
            string email = "patient@test.com",
            string phoneNumber = "9876543210",
            string? insuranceId = "INS001",
            bool isActive = true)
        {
            return new PatientDto
            {
                PatientId = patientId,
                PatientName = patientName,
                DateOfBirth = new DateTime(2000, 1, 1),
                Gender = gender,
                Email = email,
                PhoneNumber = phoneNumber,
                InsuranceID = insuranceId,
                IsActive = isActive
            };
        }

        private static Doctor CreateDoctor(
            int doctorId = 1,
            string doctorName = "Doctor One")
        {
            return new Doctor
            {
                DoctorId = doctorId,
                DoctorName = doctorName,
                Specialisation = "Cardiologist",
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true
            };
        }

        private static Appointment CreateAppointment(
            int appointmentId = 1,
            int patientId = 1,
            int doctorId = 1)
        {
            return new Appointment
            {
                AppointmentId = appointmentId,
                PatientId = patientId,
                Patient = CreatePatient(patientId),
                DoctorId = doctorId,
                Doctor = CreateDoctor(doctorId),
                ScheduledDate = DateTime.UtcNow.Date.AddDays(1),
                TimeSlot = "09:00",
                Status = "Completed",
                CancellationReason = string.Empty
            };
        }

        private static HealthRecord CreateHealthRecord(
            int healthRecordId = 1,
            int patientId = 1,
            int doctorId = 1,
            int appointmentId = 1)
        {
            return new HealthRecord
            {
                HealthRecordId = healthRecordId,
                PatientId = patientId,
                Patient = CreatePatient(patientId),
                DoctorId = doctorId,
                Doctor = CreateDoctor(doctorId),
                AppointmentId = appointmentId,
                Appointment = CreateAppointment(appointmentId, patientId, doctorId),
                VisitDate = DateTime.UtcNow,
                Diagnosis = "Diagnosis One",
                Prescription = "Prescription One",
                Notes = "Notes One"
            };
        }

        private static HealthRecordDto CreateHealthRecordDto(
            int healthRecordId = 1,
            int patientId = 1,
            int doctorId = 1,
            int appointmentId = 1)
        {
            return new HealthRecordDto
            {
                HealthRecordId = healthRecordId,
                PatientId = patientId,
                PatientName = "Patient One",
                DoctorId = doctorId,
                DoctorName = "Doctor One",
                AppointmentId = appointmentId,
                VisitDate = DateTime.UtcNow,
                Diagnosis = "Diagnosis One",
                Prescription = "Prescription One",
                Notes = "Notes One"
            };
        }

        [Fact]
        public async Task GetAllAsync_WhenPatientsExist_ShouldReturnMappedPatientDtos()
        {
            var ct = CancellationToken.None;

            var patients = new List<Patient>
            {
                CreatePatient(
                    patientId: 1,
                    patientName: "Patient One",
                    email: "patientone@test.com"),
                CreatePatient(
                    patientId: 2,
                    patientName: "Patient Two",
                    email: "patienttwo@test.com")
            };

            var expectedDtos = new List<PatientDto>
            {
                CreatePatientDto(
                    patientId: 1,
                    patientName: "Patient One",
                    email: "patientone@test.com"),
                CreatePatientDto(
                    patientId: 2,
                    patientName: "Patient Two",
                    email: "patienttwo@test.com")
            };

            var repositoryMock = new Mock<IPatientRepository>();

            repositoryMock
                .Setup(x => x.GetAllAsync(ct))
                .ReturnsAsync(patients);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<List<PatientDto>>(patients))
                .Returns(expectedDtos);

            var service = CreateService(
                repositoryMock: repositoryMock,
                mapperMock: mapperMock);

            var result = await service.GetAllAsync(ct);

            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].PatientId);
            Assert.Equal("Patient One", result[0].PatientName);
            Assert.Equal(2, result[1].PatientId);
            Assert.Equal("Patient Two", result[1].PatientName);

            repositoryMock.Verify(x => x.GetAllAsync(ct), Times.Once);
            mapperMock.Verify(x => x.Map<List<PatientDto>>(patients), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoPatientsExist_ShouldReturnEmptyList()
        {
            var ct = CancellationToken.None;

            var patients = new List<Patient>();

            var expectedDtos = new List<PatientDto>();

            var repositoryMock = new Mock<IPatientRepository>();

            repositoryMock
                .Setup(x => x.GetAllAsync(ct))
                .ReturnsAsync(patients);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<List<PatientDto>>(patients))
                .Returns(expectedDtos);

            var service = CreateService(
                repositoryMock: repositoryMock,
                mapperMock: mapperMock);

            var result = await service.GetAllAsync(ct);

            Assert.Empty(result);

            repositoryMock.Verify(x => x.GetAllAsync(ct), Times.Once);
            mapperMock.Verify(x => x.Map<List<PatientDto>>(patients), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenPatientExists_ShouldReturnMappedPatientDto()
        {
            var ct = CancellationToken.None;

            var patient = CreatePatient(
                patientId: 10,
                patientName: "Patient Ten",
                email: "patientten@test.com",
                phoneNumber: "9876543210",
                insuranceId: "INS010",
                isActive: true);

            var expectedDto = CreatePatientDto(
                patientId: 10,
                patientName: "Patient Ten",
                email: "patientten@test.com",
                phoneNumber: "9876543210",
                insuranceId: "INS010",
                isActive: true);

            var repositoryMock = new Mock<IPatientRepository>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(10, ct))
                .ReturnsAsync(patient);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<PatientDto>(patient))
                .Returns(expectedDto);

            var service = CreateService(
                repositoryMock: repositoryMock,
                mapperMock: mapperMock);

            var result = await service.GetByIdAsync(10, ct);

            Assert.Equal(10, result.PatientId);
            Assert.Equal("Patient Ten", result.PatientName);
            Assert.Equal("patientten@test.com", result.Email);
            Assert.Equal("INS010", result.InsuranceID);
            Assert.True(result.IsActive);

            repositoryMock.Verify(x => x.GetByIdAsync(10, ct), Times.Once);
            mapperMock.Verify(x => x.Map<PatientDto>(patient), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenPatientDoesNotExist_ShouldThrowNotFoundException()
        {
            var ct = CancellationToken.None;

            var repositoryMock = new Mock<IPatientRepository>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(99, ct))
                .ReturnsAsync((Patient?)null);

            var mapperMock = new Mock<IMapper>();

            var service = CreateService(
                repositoryMock: repositoryMock,
                mapperMock: mapperMock);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.GetByIdAsync(99, ct));

            Assert.Equal("Patient not found", exception.Message);

            repositoryMock.Verify(x => x.GetByIdAsync(99, ct), Times.Once);
            mapperMock.Verify(x => x.Map<PatientDto>(It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenPatientDoesNotExist_ShouldThrowNotFoundException()
        {
            var ct = CancellationToken.None;

            var request = new UpdatePatientDto
            {
                PatientName = "Updated Patient",
                DateOfBirth = new DateTime(1999, 1, 1),
                Gender = "Female",
                PhoneNumber = "9876543211",
                InsuranceID = "INS002"
            };

            var repositoryMock = new Mock<IPatientRepository>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(404, ct))
                .ReturnsAsync((Patient?)null);

            var mapperMock = new Mock<IMapper>();

            var service = CreateService(
                repositoryMock: repositoryMock,
                mapperMock: mapperMock);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.UpdatePatientAsync(404, request, ct));

            Assert.Equal("Patient not found", exception.Message);

            repositoryMock.Verify(x => x.GetByIdAsync(404, ct), Times.Once);
            repositoryMock.Verify(x => x.UpdateAsync(
                    It.IsAny<int>(),
                    It.IsAny<Patient>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);

            mapperMock.Verify(x => x.Map(request, It.IsAny<Patient>()), Times.Never);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenUpdateReturnsNull_ShouldThrowNotFoundException()
        {
            var ct = CancellationToken.None;

            var request = new UpdatePatientDto
            {
                PatientName = "Updated Patient",
                DateOfBirth = new DateTime(1999, 1, 1),
                Gender = "Female",
                PhoneNumber = "9876543211",
                InsuranceID = "INS002"
            };

            var existingPatient = CreatePatient(
                patientId: 1,
                patientName: "Old Patient",
                gender: "Male",
                email: "oldpatient@test.com",
                phoneNumber: "9876543210",
                insuranceId: "INS001",
                isActive: true);

            var repositoryMock = new Mock<IPatientRepository>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(1, ct))
                .ReturnsAsync(existingPatient);

            repositoryMock
                .Setup(x => x.UpdateAsync(1, existingPatient, ct))
                .ReturnsAsync((Patient?)null);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map(request, existingPatient))
                .Callback<UpdatePatientDto, Patient>((source, destination) =>
                {
                    destination.PatientName = source.PatientName;
                    destination.DateOfBirth = source.DateOfBirth;
                    destination.Gender = source.Gender;
                    destination.PhoneNumber = source.PhoneNumber;
                    destination.InsuranceID = source.InsuranceID;
                })
                .Returns(existingPatient);

            var service = CreateService(
                repositoryMock: repositoryMock,
                mapperMock: mapperMock);

            var exception = await Assert.ThrowsAsync<NotFoundException>(
                () => service.UpdatePatientAsync(1, request, ct));

            Assert.Equal("Patient not found", exception.Message);

            repositoryMock.Verify(x => x.GetByIdAsync(1, ct), Times.Once);
            repositoryMock.Verify(x => x.UpdateAsync(1, existingPatient, ct), Times.Once);
            mapperMock.Verify(x => x.Map(request, existingPatient), Times.Once);
        }

        [Fact]
        public async Task UpdatePatientAsync_WhenValidRequest_ShouldUpdateAndReturnMappedPatientDto()
        {
            var ct = CancellationToken.None;

            var request = new UpdatePatientDto
            {
                PatientName = "Updated Patient",
                DateOfBirth = new DateTime(1999, 1, 1),
                Gender = "Female",
                PhoneNumber = "9876543211",
                InsuranceID = "INS002"
            };

            var existingPatient = CreatePatient(
                patientId: 2,
                patientName: "Old Patient",
                gender: "Male",
                email: "oldpatient@test.com",
                phoneNumber: "9876543210",
                insuranceId: "INS001",
                isActive: true);

            var updatedPatient = CreatePatient(
                patientId: 2,
                patientName: request.PatientName,
                gender: request.Gender,
                email: existingPatient.Email,
                phoneNumber: request.PhoneNumber,
                insuranceId: request.InsuranceID,
                isActive: existingPatient.IsActive);

            updatedPatient.DateOfBirth = request.DateOfBirth;

            var expectedDto = CreatePatientDto(
                patientId: 2,
                patientName: request.PatientName,
                gender: request.Gender,
                email: existingPatient.Email,
                phoneNumber: request.PhoneNumber,
                insuranceId: request.InsuranceID,
                isActive: existingPatient.IsActive);

            expectedDto.DateOfBirth = request.DateOfBirth;

            var repositoryMock = new Mock<IPatientRepository>();

            repositoryMock
                .Setup(x => x.GetByIdAsync(2, ct))
                .ReturnsAsync(existingPatient);

            repositoryMock
                .Setup(x => x.UpdateAsync(2, existingPatient, ct))
                .ReturnsAsync(updatedPatient);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map(request, existingPatient))
                .Callback<UpdatePatientDto, Patient>((source, destination) =>
                {
                    destination.PatientName = source.PatientName;
                    destination.DateOfBirth = source.DateOfBirth;
                    destination.Gender = source.Gender;
                    destination.PhoneNumber = source.PhoneNumber;
                    destination.InsuranceID = source.InsuranceID;
                })
                .Returns(existingPatient);

            mapperMock
                .Setup(x => x.Map<PatientDto>(updatedPatient))
                .Returns(expectedDto);

            var service = CreateService(
                repositoryMock: repositoryMock,
                mapperMock: mapperMock);

            var result = await service.UpdatePatientAsync(2, request, ct);

            Assert.Equal(2, result.PatientId);
            Assert.Equal("Updated Patient", result.PatientName);
            Assert.Equal("Female", result.Gender);
            Assert.Equal("oldpatient@test.com", result.Email);
            Assert.Equal("9876543211", result.PhoneNumber);
            Assert.Equal("INS002", result.InsuranceID);
            Assert.True(result.IsActive);

            repositoryMock.Verify(x => x.GetByIdAsync(2, ct), Times.Once);
            repositoryMock.Verify(x => x.UpdateAsync(2, existingPatient, ct), Times.Once);
            mapperMock.Verify(x => x.Map(request, existingPatient), Times.Once);
            mapperMock.Verify(x => x.Map<PatientDto>(updatedPatient), Times.Once);
        }

        [Fact]
        public async Task GetHealthRecordsAsync_WhenRecordsExist_ShouldReturnMappedHealthRecordDtos()
        {
            var ct = CancellationToken.None;

            var patientId = 5;

            var records = new List<HealthRecord>
            {
                CreateHealthRecord(
                    healthRecordId: 1,
                    patientId: patientId,
                    doctorId: 20,
                    appointmentId: 100),
                CreateHealthRecord(
                    healthRecordId: 2,
                    patientId: patientId,
                    doctorId: 21,
                    appointmentId: 101)
            };

            var expectedDtos = new List<HealthRecordDto>
            {
                CreateHealthRecordDto(
                    healthRecordId: 1,
                    patientId: patientId,
                    doctorId: 20,
                    appointmentId: 100),
                CreateHealthRecordDto(
                    healthRecordId: 2,
                    patientId: patientId,
                    doctorId: 21,
                    appointmentId: 101)
            };

            var repositoryMock = new Mock<IPatientRepository>();

            repositoryMock
                .Setup(x => x.GetHealthRecordsAsync(patientId, ct))
                .ReturnsAsync(records);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<List<HealthRecordDto>>(records))
                .Returns(expectedDtos);

            var service = CreateService(
                repositoryMock: repositoryMock,
                mapperMock: mapperMock);

            var result = await service.GetHealthRecordsAsync(patientId, ct);

            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].HealthRecordId);
            Assert.Equal(2, result[1].HealthRecordId);
            Assert.Equal(patientId, result[0].PatientId);
            Assert.Equal(patientId, result[1].PatientId);

            repositoryMock.Verify(x => x.GetHealthRecordsAsync(patientId, ct), Times.Once);
            mapperMock.Verify(x => x.Map<List<HealthRecordDto>>(records), Times.Once);
        }

        [Fact]
        public async Task GetHealthRecordsAsync_WhenNoRecordsExist_ShouldReturnEmptyList()
        {
            var ct = CancellationToken.None;

            var patientId = 5;

            var records = new List<HealthRecord>();

            var expectedDtos = new List<HealthRecordDto>();

            var repositoryMock = new Mock<IPatientRepository>();

            repositoryMock
                .Setup(x => x.GetHealthRecordsAsync(patientId, ct))
                .ReturnsAsync(records);

            var mapperMock = new Mock<IMapper>();

            mapperMock
                .Setup(x => x.Map<List<HealthRecordDto>>(records))
                .Returns(expectedDtos);

            var service = CreateService(
                repositoryMock: repositoryMock,
                mapperMock: mapperMock);

            var result = await service.GetHealthRecordsAsync(patientId, ct);

            Assert.Empty(result);

            repositoryMock.Verify(x => x.GetHealthRecordsAsync(patientId, ct), Times.Once);
            mapperMock.Verify(x => x.Map<List<HealthRecordDto>>(records), Times.Once);
        }
    }
}