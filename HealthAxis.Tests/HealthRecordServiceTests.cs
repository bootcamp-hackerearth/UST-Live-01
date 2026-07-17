using FluentAssertions;
using HealthAxis.API.Data;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Services.Implementation;
using HealthAxis.Shared.DTO.HealthRecordDtos;
using HealthAxis.Shared.Enums;
using Microsoft.EntityFrameworkCore;

using ApiValidationException = HealthAxis.API.Exceptions.ValidationException;

namespace HealthAxis.API.Tests.Services
{
    public class HealthRecordServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly HealthRecordService _service;

        public HealthRecordServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            _service = new HealthRecordService(_context);
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenPatientIdIsZero_ReturnsEmptyList()
        {
            var result = await _service.GetByPatientIdAsync(0);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenPatientIdIsNegative_ReturnsEmptyList()
        {
            var result = await _service.GetByPatientIdAsync(-1);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenPatientDoesNotExist_ReturnsEmptyList()
        {
            var result = await _service.GetByPatientIdAsync(99);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByPatientIdAsync_WhenPatientExistsButNoRecords_ReturnsEmptyList()
        {
            _context.Patients.Add(CreatePatient());
            await _context.SaveChangesAsync();

            var result = await _service.GetByPatientIdAsync(1);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

       
        [Fact]
        public async Task GetByPatientIdAsync_WhenRecordsExist_MapsPatientAndDoctorDetails()
        {
            await SeedPatientDoctorAppointmentAsync();

            _context.HealthRecords.Add(CreateHealthRecord());
            await _context.SaveChangesAsync();

            var result = await _service.GetByPatientIdAsync(1);

            result.Should().HaveCount(1);
            result[0].PatientId.Should().Be(1);
            result[0].PatientName.Should().Be("Mona");
            result[0].DoctorId.Should().Be(1);
            result[0].DoctorName.Should().Be("Dr John");
            result[0].Specialisation.Should().Be(Specialisation.Cardiology.ToString());
        }

      
        [Fact]
        public async Task GetByIdAsync_WhenIdIsZero_ReturnsNull()
        {
            var result = await _service.GetByIdAsync(0);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_WhenIdIsNegative_ReturnsNull()
        {
            var result = await _service.GetByIdAsync(-1);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_WhenRecordDoesNotExist_ReturnsNull()
        {
            var result = await _service.GetByIdAsync(99);

            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_WhenRecordExists_ReturnsHealthRecordDto()
        {
            await SeedPatientDoctorAppointmentAsync();

            _context.HealthRecords.Add(CreateHealthRecord());
            await _context.SaveChangesAsync();

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result!.HealthRecordId.Should().Be(1);
            result.RecordId.Should().Be(1);
            result.PatientName.Should().Be("Mona");
            result.DoctorName.Should().Be("Dr John");
            result.Diagnosis.Should().Be("Fever");
        }

        [Fact]
        public async Task AddAsync_WhenDtoIsNull_ThrowsArgumentNullException()
        {
            Func<Task> act = async () => await _service.AddAsync(null!, 1);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddAsync_WhenVisitDateIsDefault_ThrowsValidationException()
        {
            var dto = CreateCreateHealthRecordDto();
            dto.VisitDate = default;

            Func<Task> act = async () => await _service.AddAsync(dto, 1);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task AddAsync_WhenVisitDateIsFuture_ThrowsValidationException()
        {
            var dto = CreateCreateHealthRecordDto();
            dto.VisitDate = DateTime.Today.AddDays(1);

            Func<Task> act = async () => await _service.AddAsync(dto, 1);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task AddAsync_WhenDiagnosisIsInvalid_ThrowsValidationException(string? diagnosis)
        {
            var dto = CreateCreateHealthRecordDto();
            dto.Diagnosis = diagnosis!;

            Func<Task> act = async () => await _service.AddAsync(dto, 1);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task AddAsync_WhenPrescriptionIsInvalid_ThrowsValidationException(string? prescription)
        {
            var dto = CreateCreateHealthRecordDto();
            dto.Prescription = prescription!;

            Func<Task> act = async () => await _service.AddAsync(dto, 1);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task AddAsync_WhenAppointmentDoesNotExist_ThrowsNotFoundException()
        {
            var dto = CreateCreateHealthRecordDto();

            Func<Task> act = async () => await _service.AddAsync(dto, 1);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task AddAsync_WhenAppointmentBelongsToAnotherDoctor_ThrowsBusinessRuleException()
        {
            await SeedPatientDoctorAppointmentAsync(doctorId: 2);

            var dto = CreateCreateHealthRecordDto();

            Func<Task> act = async () => await _service.AddAsync(dto, 1);

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Theory]
        [InlineData(AppointmentStatus.Pending)]
        [InlineData(AppointmentStatus.Completed)]
        [InlineData(AppointmentStatus.Cancelled)]
        public async Task AddAsync_WhenAppointmentIsNotConfirmed_ThrowsBusinessRuleException(
            AppointmentStatus status)
        {
            await SeedPatientDoctorAppointmentAsync(status: status);

            var dto = CreateCreateHealthRecordDto();

            Func<Task> act = async () => await _service.AddAsync(dto, 1);

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task AddAsync_WhenRecordAlreadyExists_ThrowsBusinessRuleException()
        {
            await SeedPatientDoctorAppointmentAsync();

            _context.HealthRecords.Add(CreateHealthRecord());
            await _context.SaveChangesAsync();

            var dto = CreateCreateHealthRecordDto();

            Func<Task> act = async () => await _service.AddAsync(dto, 1);

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task AddAsync_WhenPatientDoesNotExist_ThrowsNotFoundException()
        {
            _context.Doctors.Add(CreateDoctor());

            _context.Appointments.Add(CreateAppointment(
                patientId: 99,
                doctorId: 1,
                status: AppointmentStatus.Confirmed));

            await _context.SaveChangesAsync();

            var dto = CreateCreateHealthRecordDto();

            Func<Task> act = async () => await _service.AddAsync(dto, 1);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task AddAsync_WhenDoctorDoesNotExist_ThrowsNotFoundException()
        {
            _context.Patients.Add(CreatePatient());

            _context.Appointments.Add(CreateAppointment(
                patientId: 1,
                doctorId: 99,
                status: AppointmentStatus.Confirmed));

            await _context.SaveChangesAsync();

            var dto = CreateCreateHealthRecordDto();

            Func<Task> act = async () => await _service.AddAsync(dto, 99);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task AddAsync_WhenValid_AddsHealthRecordAndCompletesAppointment()
        {
            await SeedPatientDoctorAppointmentAsync();

            var dto = CreateCreateHealthRecordDto();
            dto.Diagnosis = " Fever ";
            dto.Prescription = " Tablet ";
            dto.Notes = " Take rest ";

            var result = await _service.AddAsync(dto, 1);

            result.Should().NotBeNull();
            result.HealthRecordId.Should().BeGreaterThan(0);
            result.AppointmentId.Should().Be(1);
            result.PatientId.Should().Be(1);
            result.DoctorId.Should().Be(1);
            result.Diagnosis.Should().Be("Fever");
            result.Prescription.Should().Be("Tablet");
            result.Notes.Should().Be("Take rest");

            var appointment = await _context.Appointments.FindAsync(1);
            appointment.Should().NotBeNull();
            appointment!.Status.Should().Be(AppointmentStatus.Completed);
        }

        [Fact]
        public async Task AddAsync_WhenNotesIsNull_SavesEmptyNotes()
        {
            await SeedPatientDoctorAppointmentAsync();

            var dto = CreateCreateHealthRecordDto();
            dto.Notes = null!;

            var result = await _service.AddAsync(dto, 1);

            result.Notes.Should().BeEmpty();
        }

        [Fact]
        public async Task UpdateAsync_WhenDtoIsNull_ThrowsArgumentNullException()
        {
            Func<Task> act = async () => await _service.UpdateAsync(1, null!, 1);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task UpdateAsync_WhenDiagnosisIsInvalid_ThrowsValidationException(string? diagnosis)
        {
            var dto = CreateUpdateHealthRecordDto();
            dto.Diagnosis = diagnosis!;

            Func<Task> act = async () => await _service.UpdateAsync(1, dto, 1);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task UpdateAsync_WhenPrescriptionIsInvalid_ThrowsValidationException(string? prescription)
        {
            var dto = CreateUpdateHealthRecordDto();
            dto.Prescription = prescription!;

            Func<Task> act = async () => await _service.UpdateAsync(1, dto, 1);

            await act.Should().ThrowAsync<ApiValidationException>();
        }

        [Fact]
        public async Task UpdateAsync_WhenRecordDoesNotExist_ReturnsNull()
        {
            var dto = CreateUpdateHealthRecordDto();

            var result = await _service.UpdateAsync(99, dto, 1);

            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_WhenDoctorIsDifferent_ThrowsBusinessRuleException()
        {
            await SeedPatientDoctorAppointmentAsync();

            _context.HealthRecords.Add(CreateHealthRecord(doctorId: 2));
            await _context.SaveChangesAsync();

            var dto = CreateUpdateHealthRecordDto();

            Func<Task> act = async () => await _service.UpdateAsync(1, dto, 1);

            await act.Should().ThrowAsync<BusinessRuleException>();
        }

        [Fact]
        public async Task UpdateAsync_WhenValid_UpdatesHealthRecord()
        {
            await SeedPatientDoctorAppointmentAsync();

            _context.HealthRecords.Add(CreateHealthRecord());
            await _context.SaveChangesAsync();

            var dto = CreateUpdateHealthRecordDto();
            dto.Diagnosis = " Updated Diagnosis ";
            dto.Prescription = " Updated Prescription ";
            dto.Notes = " Updated Notes ";

            var result = await _service.UpdateAsync(1, dto, 1);

            result.Should().NotBeNull();
            result!.Diagnosis.Should().Be("Updated Diagnosis");
            result.Prescription.Should().Be("Updated Prescription");
            result.Notes.Should().Be("Updated Notes");
            result.UpdatedDate.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateAsync_WhenNotesIsNull_SavesEmptyNotes()
        {
            await SeedPatientDoctorAppointmentAsync();

            _context.HealthRecords.Add(CreateHealthRecord());
            await _context.SaveChangesAsync();

            var dto = CreateUpdateHealthRecordDto();
            dto.Notes = null!;

            var result = await _service.UpdateAsync(1, dto, 1);

            result.Should().NotBeNull();
            result!.Notes.Should().BeEmpty();
        }

        private async Task SeedPatientDoctorAppointmentAsync(
            int patientId = 1,
            int doctorId = 1,
            AppointmentStatus status = AppointmentStatus.Confirmed)
        {
            _context.Patients.Add(CreatePatient(patientId));
            _context.Doctors.Add(CreateDoctor(doctorId));

            _context.Appointments.Add(CreateAppointment(
                patientId: patientId,
                doctorId: doctorId,
                status: status));

            await _context.SaveChangesAsync();
        }

        private static Patient CreatePatient(int id = 1)
        {
            return new Patient
            {
                PatientId = id,
                FullName = "Mona",
                DateOfBirth = new DateTime(2004, 1, 1),
                Gender = Gender.Female,
                PhoneNumber = "9876543210",
                Email = "patient@gmail.com",
                UserId = "patient-user-1",
                CreatedDate = DateTime.Today
            };
        }

        private static Doctor CreateDoctor(int id = 1)
        {
            return new Doctor
            {
                DoctorId = id,
                FullName = "Dr John",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 10,
                ConsultationFee = 500,
                IsActive = true,
                UserId = "doctor-user-1"
            };
        }

        private static Appointment CreateAppointment(
            int id = 1,
            int patientId = 1,
            int doctorId = 1,
            AppointmentStatus status = AppointmentStatus.Confirmed)
        {
            return new Appointment
            {
                AppointmentId = id,
                PatientId = patientId,
                DoctorId = doctorId,
                ScheduledDate = DateTime.Today,
                TimeSlot = "10:00 AM - 11:00 AM",
                Status = status
            };
        }

        private static HealthRecord CreateHealthRecord(
            int id = 1,
            int appointmentId = 1,
            int patientId = 1,
            int doctorId = 1,
            DateTime? visitDate = null,
            string diagnosis = "Fever",
            string prescription = "Tablet",
            string notes = "Take rest")
        {
            return new HealthRecord
            {
                HealthRecordId = id,
                AppointmentId = appointmentId,
                PatientId = patientId,
                DoctorId = doctorId,
                VisitDate = visitDate ?? DateTime.Today,
                Diagnosis = diagnosis,
                Prescription = prescription,
                Notes = notes
            };
        }

        private static CreateHealthRecordDto CreateCreateHealthRecordDto()
        {
            return new CreateHealthRecordDto
            {
                AppointmentId = 1,
                
                VisitDate = DateTime.Today,
                Diagnosis = "Fever",
                Prescription = "Tablet",
                Notes = "Take rest"
            };
        }

        private static UpdateHealthRecordDto CreateUpdateHealthRecordDto()
        {
            return new UpdateHealthRecordDto
            {
                Diagnosis = "Updated Fever",
                Prescription = "Updated Tablet",
                Notes = "Updated Notes"
            };
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}