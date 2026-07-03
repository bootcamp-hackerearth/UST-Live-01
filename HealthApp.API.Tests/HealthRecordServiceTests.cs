using System.Security.Claims;
using AutoMapper;
using HealthApp.API.Exceptions;
using HealthApp.API.Models;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;
using HealthApp.Shared.Constants;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace HealthApp.API.Tests;

public class HealthRecordServiceTests
{
    private readonly Mock<IHealthRecordRepository> _healthRecordRepository = new();
    private readonly Mock<IPatientRepository> _patientRepository = new();
    private readonly Mock<IDoctorRepository> _doctorRepository = new();
    private readonly Mock<IAppointmentRepository> _appointmentRepository = new();
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new();
    private readonly Mock<IMapper> _mapper = new();

    private HealthRecordService CreateService(string userId, string role)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Role, role),
            new(ClaimTypes.Email, $"{role.ToLower()}@healthapp.com")
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);

        _httpContextAccessor.Setup(x => x.HttpContext)
            .Returns(new DefaultHttpContext
            {
                User = principal
            });

        return new HealthRecordService(
            _healthRecordRepository.Object,
            _patientRepository.Object,
            _doctorRepository.Object,
            _appointmentRepository.Object,
            _httpContextAccessor.Object,
            _mapper.Object);
    }

    [Fact]
    public async Task GetHealthRecordsByPatientIdAsync_WhenPatientRequestsOwnRecords_ReturnsRecords()
    {
        // Arrange
        const string userId = "patient-user-1";
        const int patientId = 10;

        var service = CreateService(userId, Roles.Patient);

        var patient = new Patient
        {
            PatientId = patientId,
            UserId = userId,
            PatientName = "Arjun Menon"
        };

        var records = new List<HealthRecord>
        {
            new()
            {
                HealthRecordId = 1,
                PatientId = patientId,
                DoctorId = 5,
                AppointmentId = 100,
                Diagnosis = "Fever",
                Prescription = "Paracetamol",
                CreatedDate = DateTime.Now
            }
        };

        var recordDtos = new List<HealthRecordDto>
        {
            new()
            {
                HealthRecordId = 1,
                PatientId = patientId,
                DoctorId = 5,
                AppointmentId = 100,
                Diagnosis = "Fever",
                Prescription = "Paracetamol"
            }
        };

        _patientRepository.Setup(x => x.GetByIdAsync(patientId))
            .ReturnsAsync(patient);

        _patientRepository.Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync(patient);

        _healthRecordRepository.Setup(x => x.GetByPatientIdAsync(patientId))
            .ReturnsAsync(records);

        _mapper.Setup(x => x.Map<List<HealthRecordDto>>(records))
            .Returns(recordDtos);

        // Act
        var result = await service.GetHealthRecordsByPatientIdAsync(patientId);

        // Assert
        Assert.Single(result);
        Assert.Equal(patientId, result[0].PatientId);
    }

    [Fact]
    public async Task GetHealthRecordsByPatientIdAsync_WhenPatientRequestsAnotherPatientRecords_ThrowsForbidden()
    {
        // Arrange
        const string userId = "patient-user-1";
        const int loggedInPatientId = 10;
        const int requestedPatientId = 11;

        var service = CreateService(userId, Roles.Patient);

        _patientRepository.Setup(x => x.GetByIdAsync(requestedPatientId))
            .ReturnsAsync(new Patient
            {
                PatientId = requestedPatientId,
                UserId = "another-user",
                PatientName = "Another Patient"
            });

        _patientRepository.Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync(new Patient
            {
                PatientId = loggedInPatientId,
                UserId = userId,
                PatientName = "Logged Patient"
            });

        // Act + Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            service.GetHealthRecordsByPatientIdAsync(requestedPatientId));
    }

    [Fact]
    public async Task GetHealthRecordsByPatientIdAsync_WhenAdminRequestsHealthRecords_ThrowsForbidden()
    {
        // Arrange
        const int patientId = 10;

        var service = CreateService("admin-user-1", Roles.Admin);

        _patientRepository.Setup(x => x.GetByIdAsync(patientId))
            .ReturnsAsync(new Patient
            {
                PatientId = patientId,
                PatientName = "Arjun Menon"
            });

        // Act + Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            service.GetHealthRecordsByPatientIdAsync(patientId));
    }

    [Fact]
    public async Task GetHealthRecordsByPatientIdAsync_WhenDoctorHasAppointmentWithPatient_ReturnsRecords()
    {
        // Arrange
        const string doctorUserId = "doctor-user-1";
        const int doctorId = 5;
        const int patientId = 10;

        var service = CreateService(doctorUserId, Roles.Doctor);

        var doctor = new Doctor
        {
            DoctorId = doctorId,
            UserId = doctorUserId,
            DoctorName = "Dr Sneha Paul"
        };

        var appointment = new Appointment
        {
            AppointmentId = 100,
            DoctorId = doctorId,
            PatientId = patientId,
            Status = AppointmentStatus.Confirmed.ToString()
        };

        var records = new List<HealthRecord>
        {
            new()
            {
                HealthRecordId = 1,
                PatientId = patientId,
                DoctorId = doctorId,
                AppointmentId = 100,
                Diagnosis = "Cold",
                Prescription = "Rest"
            }
        };

        var recordDtos = new List<HealthRecordDto>
        {
            new()
            {
                HealthRecordId = 1,
                PatientId = patientId,
                DoctorId = doctorId,
                AppointmentId = 100,
                Diagnosis = "Cold",
                Prescription = "Rest"
            }
        };

        _patientRepository.Setup(x => x.GetByIdAsync(patientId))
            .ReturnsAsync(new Patient
            {
                PatientId = patientId,
                PatientName = "Arjun Menon"
            });

        _doctorRepository.Setup(x => x.GetByUserIdAsync(doctorUserId))
            .ReturnsAsync(doctor);

        _appointmentRepository.Setup(x => x.GetByDoctorIdAsync(doctorId))
            .ReturnsAsync(new List<Appointment> { appointment });

        _healthRecordRepository.Setup(x => x.GetByPatientIdAsync(patientId))
            .ReturnsAsync(records);

        _mapper.Setup(x => x.Map<List<HealthRecordDto>>(records))
            .Returns(recordDtos);

        // Act
        var result = await service.GetHealthRecordsByPatientIdAsync(patientId);

        // Assert
        Assert.Single(result);
        Assert.Equal(patientId, result[0].PatientId);
    }

    [Fact]
    public async Task GetHealthRecordsByPatientIdAsync_WhenDoctorHasNoAppointmentWithPatient_ThrowsForbidden()
    {
        // Arrange
        const string doctorUserId = "doctor-user-1";
        const int doctorId = 5;
        const int requestedPatientId = 10;

        var service = CreateService(doctorUserId, Roles.Doctor);

        _patientRepository.Setup(x => x.GetByIdAsync(requestedPatientId))
            .ReturnsAsync(new Patient
            {
                PatientId = requestedPatientId,
                PatientName = "Arjun Menon"
            });

        _doctorRepository.Setup(x => x.GetByUserIdAsync(doctorUserId))
            .ReturnsAsync(new Doctor
            {
                DoctorId = doctorId,
                UserId = doctorUserId,
                DoctorName = "Dr Sneha Paul"
            });

        _appointmentRepository.Setup(x => x.GetByDoctorIdAsync(doctorId))
            .ReturnsAsync(new List<Appointment>());

        // Act + Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            service.GetHealthRecordsByPatientIdAsync(requestedPatientId));
    }

    [Fact]
    public async Task AddHealthRecordAsync_WhenDoctorAddsForOwnConfirmedAppointment_CreatesRecordAndCompletesAppointment()
    {
        // Arrange
        const string doctorUserId = "doctor-user-1";
        const int doctorId = 5;
        const int patientId = 10;
        const int appointmentId = 100;

        var service = CreateService(doctorUserId, Roles.Doctor);

        var appointment = new Appointment
        {
            AppointmentId = appointmentId,
            DoctorId = doctorId,
            PatientId = patientId,
            Status = AppointmentStatus.Confirmed.ToString()
        };

        var request = new AddHealthRecordDto
        {
            PatientId = patientId,
            AppointmentId = appointmentId,
            Diagnosis = "Fever",
            Prescription = "Paracetamol",
            Notes = "Drink fluids"
        };

        var savedRecord = new HealthRecord
        {
            HealthRecordId = 1,
            PatientId = patientId,
            DoctorId = doctorId,
            AppointmentId = appointmentId,
            Diagnosis = request.Diagnosis,
            Prescription = request.Prescription,
            Notes = request.Notes
        };

        var resultDto = new HealthRecordDto
        {
            HealthRecordId = 1,
            PatientId = patientId,
            DoctorId = doctorId,
            AppointmentId = appointmentId,
            Diagnosis = request.Diagnosis,
            Prescription = request.Prescription,
            Notes = request.Notes
        };

        _doctorRepository.Setup(x => x.GetByUserIdAsync(doctorUserId))
            .ReturnsAsync(new Doctor
            {
                DoctorId = doctorId,
                UserId = doctorUserId,
                DoctorName = "Dr Sneha Paul"
            });

        _appointmentRepository.Setup(x => x.GetByIdAsync(appointmentId))
            .ReturnsAsync(appointment);

        _healthRecordRepository.Setup(x => x.ExistsByAppointmentIdAsync(appointmentId))
            .ReturnsAsync(false);

        _mapper.Setup(x => x.Map<HealthRecord>(request))
            .Returns(new HealthRecord
            {
                Diagnosis = request.Diagnosis,
                Prescription = request.Prescription,
                Notes = request.Notes
            });

        _healthRecordRepository.Setup(x => x.AddAsync(It.IsAny<HealthRecord>()))
            .ReturnsAsync(savedRecord);

        _appointmentRepository.Setup(x => x.UpdateAsync(appointmentId, It.IsAny<Appointment>()))
            .ReturnsAsync(appointment);

        _mapper.Setup(x => x.Map<HealthRecordDto>(savedRecord))
            .Returns(resultDto);

        // Act
        var result = await service.AddHealthRecordAsync(request);

        // Assert
        Assert.Equal(patientId, result.PatientId);
        Assert.Equal(doctorId, result.DoctorId);
        Assert.Equal(appointmentId, result.AppointmentId);

        _appointmentRepository.Verify(x =>
            x.UpdateAsync(
                appointmentId,
                It.Is<Appointment>(a => a.Status == AppointmentStatus.Completed.ToString())),
            Times.Once);
    }

    [Fact]
    public async Task AddHealthRecordAsync_WhenAppointmentBelongsToAnotherDoctor_ThrowsForbidden()
    {
        // Arrange
        const string doctorUserId = "doctor-user-1";
        const int loggedInDoctorId = 5;
        const int anotherDoctorId = 6;
        const int patientId = 10;
        const int appointmentId = 100;

        var service = CreateService(doctorUserId, Roles.Doctor);

        _doctorRepository.Setup(x => x.GetByUserIdAsync(doctorUserId))
            .ReturnsAsync(new Doctor
            {
                DoctorId = loggedInDoctorId,
                UserId = doctorUserId,
                DoctorName = "Dr Sneha Paul"
            });

        _appointmentRepository.Setup(x => x.GetByIdAsync(appointmentId))
            .ReturnsAsync(new Appointment
            {
                AppointmentId = appointmentId,
                DoctorId = anotherDoctorId,
                PatientId = patientId,
                Status = AppointmentStatus.Confirmed.ToString()
            });

        var request = new AddHealthRecordDto
        {
            PatientId = patientId,
            AppointmentId = appointmentId,
            Diagnosis = "Fever",
            Prescription = "Paracetamol"
        };

        // Act + Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            service.AddHealthRecordAsync(request));
    }

    [Fact]
    public async Task AddHealthRecordAsync_WhenAppointmentIsPending_ThrowsHealthRecordRuleException()
    {
        // Arrange
        const string doctorUserId = "doctor-user-1";
        const int doctorId = 5;
        const int patientId = 10;
        const int appointmentId = 100;

        var service = CreateService(doctorUserId, Roles.Doctor);

        _doctorRepository.Setup(x => x.GetByUserIdAsync(doctorUserId))
            .ReturnsAsync(new Doctor
            {
                DoctorId = doctorId,
                UserId = doctorUserId,
                DoctorName = "Dr Sneha Paul"
            });

        _appointmentRepository.Setup(x => x.GetByIdAsync(appointmentId))
            .ReturnsAsync(new Appointment
            {
                AppointmentId = appointmentId,
                DoctorId = doctorId,
                PatientId = patientId,
                Status = AppointmentStatus.Pending.ToString()
            });

        var request = new AddHealthRecordDto
        {
            PatientId = patientId,
            AppointmentId = appointmentId,
            Diagnosis = "Fever",
            Prescription = "Paracetamol"
        };

        // Act + Assert
        await Assert.ThrowsAsync<HealthRecordRuleException>(() =>
            service.AddHealthRecordAsync(request));
    }

    [Fact]
    public async Task AddHealthRecordAsync_WhenRecordAlreadyExists_ThrowsConflictException()
    {
        // Arrange
        const string doctorUserId = "doctor-user-1";
        const int doctorId = 5;
        const int patientId = 10;
        const int appointmentId = 100;

        var service = CreateService(doctorUserId, Roles.Doctor);

        _doctorRepository.Setup(x => x.GetByUserIdAsync(doctorUserId))
            .ReturnsAsync(new Doctor
            {
                DoctorId = doctorId,
                UserId = doctorUserId,
                DoctorName = "Dr Sneha Paul"
            });

        _appointmentRepository.Setup(x => x.GetByIdAsync(appointmentId))
            .ReturnsAsync(new Appointment
            {
                AppointmentId = appointmentId,
                DoctorId = doctorId,
                PatientId = patientId,
                Status = AppointmentStatus.Confirmed.ToString()
            });

        _healthRecordRepository.Setup(x => x.ExistsByAppointmentIdAsync(appointmentId))
            .ReturnsAsync(true);

        var request = new AddHealthRecordDto
        {
            PatientId = patientId,
            AppointmentId = appointmentId,
            Diagnosis = "Fever",
            Prescription = "Paracetamol"
        };

        // Act + Assert
        await Assert.ThrowsAsync<ConflictException>(() =>
            service.AddHealthRecordAsync(request));
    }
    [Fact]
    public async Task GetAllHealthRecordsAsync_WhenAdminRequestsHealthRecords_ThrowsForbidden()
    {
        // Arrange
        var service = CreateService("admin-user-1", Roles.Admin);

        // Act + Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            service.GetAllHealthRecordsAsync());
    }

    [Fact]
    public async Task GetAllHealthRecordsAsync_WhenPatientRequestsRecords_ReturnsOwnRecords()
    {
        // Arrange
        const string userId = "patient-user-1";
        const int patientId = 10;

        var service = CreateService(userId, Roles.Patient);

        var patient = new Patient
        {
            PatientId = patientId,
            UserId = userId,
            PatientName = "Arjun Menon"
        };

        var records = new List<HealthRecord>
    {
        new()
        {
            HealthRecordId = 1,
            PatientId = patientId,
            DoctorId = 5,
            AppointmentId = 100,
            Diagnosis = "Fever",
            Prescription = "Paracetamol"
        }
    };

        var recordDtos = new List<HealthRecordDto>
    {
        new()
        {
            HealthRecordId = 1,
            PatientId = patientId,
            DoctorId = 5,
            AppointmentId = 100,
            Diagnosis = "Fever",
            Prescription = "Paracetamol"
        }
    };

        _patientRepository.Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync(patient);

        _healthRecordRepository.Setup(x => x.GetByPatientIdAsync(patientId))
            .ReturnsAsync(records);

        _mapper.Setup(x => x.Map<List<HealthRecordDto>>(records))
            .Returns(recordDtos);

        // Act
        var result = await service.GetAllHealthRecordsAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(patientId, result[0].PatientId);
    }

    [Fact]
    public async Task GetAllHealthRecordsAsync_WhenDoctorRequestsRecords_ReturnsOnlyRelatedPatientRecords()
    {
        // Arrange
        const string doctorUserId = "doctor-user-1";
        const int doctorId = 5;
        const int allowedPatientId = 10;
        const int cancelledPatientId = 11;

        var service = CreateService(doctorUserId, Roles.Doctor);

        var doctor = new Doctor
        {
            DoctorId = doctorId,
            UserId = doctorUserId,
            DoctorName = "Dr Sneha Paul"
        };

        var appointments = new List<Appointment>
    {
        new()
        {
            AppointmentId = 100,
            DoctorId = doctorId,
            PatientId = allowedPatientId,
            Status = AppointmentStatus.Confirmed.ToString()
        },
        new()
        {
            AppointmentId = 101,
            DoctorId = doctorId,
            PatientId = cancelledPatientId,
            Status = AppointmentStatus.Cancelled.ToString()
        }
    };

        var allowedRecords = new List<HealthRecord>
    {
        new()
        {
            HealthRecordId = 1,
            PatientId = allowedPatientId,
            DoctorId = doctorId,
            AppointmentId = 100,
            Diagnosis = "Cold",
            Prescription = "Rest"
        }
    };

        var mappedDtos = new List<HealthRecordDto>
    {
        new()
        {
            HealthRecordId = 1,
            PatientId = allowedPatientId,
            DoctorId = doctorId,
            AppointmentId = 100,
            Diagnosis = "Cold",
            Prescription = "Rest"
        }
    };

        _doctorRepository.Setup(x => x.GetByUserIdAsync(doctorUserId))
            .ReturnsAsync(doctor);

        _appointmentRepository.Setup(x => x.GetByDoctorIdAsync(doctorId))
            .ReturnsAsync(appointments);

        _healthRecordRepository.Setup(x => x.GetByPatientIdAsync(allowedPatientId))
            .ReturnsAsync(allowedRecords);

        _mapper.Setup(x => x.Map<List<HealthRecordDto>>(It.IsAny<List<HealthRecord>>()))
            .Returns(mappedDtos);

        // Act
        var result = await service.GetAllHealthRecordsAsync();

        // Assert
        Assert.Single(result);
        Assert.Equal(allowedPatientId, result[0].PatientId);

        _healthRecordRepository.Verify(x =>
            x.GetByPatientIdAsync(allowedPatientId),
            Times.Once);

        _healthRecordRepository.Verify(x =>
            x.GetByPatientIdAsync(cancelledPatientId),
            Times.Never);
    }

    [Fact]
    public async Task GetHealthRecordByIdAsync_WhenHealthRecordIdInvalid_ThrowsHealthRecordRuleException()
    {
        // Arrange
        var service = CreateService("patient-user-1", Roles.Patient);

        // Act + Assert
        await Assert.ThrowsAsync<HealthRecordRuleException>(() =>
            service.GetHealthRecordByIdAsync(0));
    }

    [Fact]
    public async Task GetHealthRecordByIdAsync_WhenRecordNotFound_ThrowsEntityNotFoundException()
    {
        // Arrange
        var service = CreateService("patient-user-1", Roles.Patient);

        _healthRecordRepository.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync((HealthRecord?)null);

        // Act + Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            service.GetHealthRecordByIdAsync(1));
    }

    [Fact]
    public async Task GetHealthRecordByIdAsync_WhenPatientRequestsOwnRecord_ReturnsRecord()
    {
        // Arrange
        const string userId = "patient-user-1";
        const int patientId = 10;
        const int healthRecordId = 1;

        var service = CreateService(userId, Roles.Patient);

        var patient = new Patient
        {
            PatientId = patientId,
            UserId = userId,
            PatientName = "Arjun Menon"
        };

        var record = new HealthRecord
        {
            HealthRecordId = healthRecordId,
            PatientId = patientId,
            DoctorId = 5,
            AppointmentId = 100,
            Diagnosis = "Fever",
            Prescription = "Paracetamol"
        };

        var recordDto = new HealthRecordDto
        {
            HealthRecordId = healthRecordId,
            PatientId = patientId,
            DoctorId = 5,
            AppointmentId = 100,
            Diagnosis = "Fever",
            Prescription = "Paracetamol"
        };

        _healthRecordRepository.Setup(x => x.GetByIdAsync(healthRecordId))
            .ReturnsAsync(record);

        _patientRepository.Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync(patient);

        _mapper.Setup(x => x.Map<HealthRecordDto>(record))
            .Returns(recordDto);

        // Act
        var result = await service.GetHealthRecordByIdAsync(healthRecordId);

        // Assert
        Assert.Equal(healthRecordId, result.HealthRecordId);
        Assert.Equal(patientId, result.PatientId);
    }

    [Fact]
    public async Task GetHealthRecordByIdAsync_WhenPatientRequestsAnotherPatientRecord_ThrowsForbidden()
    {
        // Arrange
        const string userId = "patient-user-1";
        const int loggedInPatientId = 10;
        const int anotherPatientId = 11;
        const int healthRecordId = 1;

        var service = CreateService(userId, Roles.Patient);

        _healthRecordRepository.Setup(x => x.GetByIdAsync(healthRecordId))
            .ReturnsAsync(new HealthRecord
            {
                HealthRecordId = healthRecordId,
                PatientId = anotherPatientId,
                DoctorId = 5,
                AppointmentId = 100,
                Diagnosis = "Fever",
                Prescription = "Paracetamol"
            });

        _patientRepository.Setup(x => x.GetByUserIdAsync(userId))
            .ReturnsAsync(new Patient
            {
                PatientId = loggedInPatientId,
                UserId = userId,
                PatientName = "Logged Patient"
            });

        // Act + Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            service.GetHealthRecordByIdAsync(healthRecordId));
    }

    [Fact]
    public async Task GetHealthRecordByIdAsync_WhenAdminRequestsRecord_ThrowsForbidden()
    {
        // Arrange
        const int healthRecordId = 1;

        var service = CreateService("admin-user-1", Roles.Admin);

        _healthRecordRepository.Setup(x => x.GetByIdAsync(healthRecordId))
            .ReturnsAsync(new HealthRecord
            {
                HealthRecordId = healthRecordId,
                PatientId = 10,
                DoctorId = 5,
                AppointmentId = 100,
                Diagnosis = "Fever",
                Prescription = "Paracetamol"
            });

        // Act + Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            service.GetHealthRecordByIdAsync(healthRecordId));
    }

    [Fact]
    public async Task GetHealthRecordByIdAsync_WhenDoctorHasPatientRelation_ReturnsRecord()
    {
        // Arrange
        const string doctorUserId = "doctor-user-1";
        const int doctorId = 5;
        const int patientId = 10;
        const int healthRecordId = 1;

        var service = CreateService(doctorUserId, Roles.Doctor);

        var record = new HealthRecord
        {
            HealthRecordId = healthRecordId,
            PatientId = patientId,
            DoctorId = doctorId,
            AppointmentId = 100,
            Diagnosis = "Cold",
            Prescription = "Rest"
        };

        var recordDto = new HealthRecordDto
        {
            HealthRecordId = healthRecordId,
            PatientId = patientId,
            DoctorId = doctorId,
            AppointmentId = 100,
            Diagnosis = "Cold",
            Prescription = "Rest"
        };

        _healthRecordRepository.Setup(x => x.GetByIdAsync(healthRecordId))
            .ReturnsAsync(record);

        _doctorRepository.Setup(x => x.GetByUserIdAsync(doctorUserId))
            .ReturnsAsync(new Doctor
            {
                DoctorId = doctorId,
                UserId = doctorUserId,
                DoctorName = "Dr Sneha Paul"
            });

        _appointmentRepository.Setup(x => x.GetByDoctorIdAsync(doctorId))
            .ReturnsAsync(new List<Appointment>
            {
            new()
            {
                AppointmentId = 100,
                DoctorId = doctorId,
                PatientId = patientId,
                Status = AppointmentStatus.Completed.ToString()
            }
            });

        _mapper.Setup(x => x.Map<HealthRecordDto>(record))
            .Returns(recordDto);

        // Act
        var result = await service.GetHealthRecordByIdAsync(healthRecordId);

        // Assert
        Assert.Equal(healthRecordId, result.HealthRecordId);
        Assert.Equal(patientId, result.PatientId);
    }

    [Fact]
    public async Task GetHealthRecordByIdAsync_WhenDoctorHasOnlyCancelledAppointmentRelation_ThrowsForbidden()
    {
        // Arrange
        const string doctorUserId = "doctor-user-1";
        const int doctorId = 5;
        const int patientId = 10;
        const int healthRecordId = 1;

        var service = CreateService(doctorUserId, Roles.Doctor);

        _healthRecordRepository.Setup(x => x.GetByIdAsync(healthRecordId))
            .ReturnsAsync(new HealthRecord
            {
                HealthRecordId = healthRecordId,
                PatientId = patientId,
                DoctorId = doctorId,
                AppointmentId = 100,
                Diagnosis = "Cold",
                Prescription = "Rest"
            });

        _doctorRepository.Setup(x => x.GetByUserIdAsync(doctorUserId))
            .ReturnsAsync(new Doctor
            {
                DoctorId = doctorId,
                UserId = doctorUserId,
                DoctorName = "Dr Sneha Paul"
            });

        _appointmentRepository.Setup(x => x.GetByDoctorIdAsync(doctorId))
            .ReturnsAsync(new List<Appointment>
            {
            new()
            {
                AppointmentId = 100,
                DoctorId = doctorId,
                PatientId = patientId,
                Status = AppointmentStatus.Cancelled.ToString()
            }
            });

        // Act + Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            service.GetHealthRecordByIdAsync(healthRecordId));
    }

    [Fact]
    public async Task AddHealthRecordAsync_WhenDtoIsNull_ThrowsHealthRecordRuleException()
    {
        // Arrange
        var service = CreateService("doctor-user-1", Roles.Doctor);

        // Act + Assert
        await Assert.ThrowsAsync<HealthRecordRuleException>(() =>
            service.AddHealthRecordAsync(null!));
    }

    [Fact]
    public async Task AddHealthRecordAsync_WhenUserIsNotDoctor_ThrowsForbidden()
    {
        // Arrange
        var service = CreateService("patient-user-1", Roles.Patient);

        var request = new AddHealthRecordDto
        {
            PatientId = 10,
            AppointmentId = 100,
            Diagnosis = "Fever",
            Prescription = "Paracetamol"
        };

        // Act + Assert
        await Assert.ThrowsAsync<ForbiddenAccessException>(() =>
            service.AddHealthRecordAsync(request));
    }

    [Fact]
    public async Task AddHealthRecordAsync_WhenAppointmentNotFound_ThrowsEntityNotFoundException()
    {
        // Arrange
        const string doctorUserId = "doctor-user-1";
        const int doctorId = 5;
        const int appointmentId = 100;

        var service = CreateService(doctorUserId, Roles.Doctor);

        _doctorRepository.Setup(x => x.GetByUserIdAsync(doctorUserId))
            .ReturnsAsync(new Doctor
            {
                DoctorId = doctorId,
                UserId = doctorUserId,
                DoctorName = "Dr Sneha Paul"
            });

        _appointmentRepository.Setup(x => x.GetByIdAsync(appointmentId))
            .ReturnsAsync((Appointment?)null);

        var request = new AddHealthRecordDto
        {
            PatientId = 10,
            AppointmentId = appointmentId,
            Diagnosis = "Fever",
            Prescription = "Paracetamol"
        };

        // Act + Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            service.AddHealthRecordAsync(request));
    }

    [Fact]
    public async Task AddHealthRecordAsync_WhenAppointmentPatientDoesNotMatchDtoPatient_ThrowsHealthRecordRuleException()
    {
        // Arrange
        const string doctorUserId = "doctor-user-1";
        const int doctorId = 5;
        const int appointmentPatientId = 10;
        const int requestedPatientId = 11;
        const int appointmentId = 100;

        var service = CreateService(doctorUserId, Roles.Doctor);

        _doctorRepository.Setup(x => x.GetByUserIdAsync(doctorUserId))
            .ReturnsAsync(new Doctor
            {
                DoctorId = doctorId,
                UserId = doctorUserId,
                DoctorName = "Dr Sneha Paul"
            });

        _appointmentRepository.Setup(x => x.GetByIdAsync(appointmentId))
            .ReturnsAsync(new Appointment
            {
                AppointmentId = appointmentId,
                DoctorId = doctorId,
                PatientId = appointmentPatientId,
                Status = AppointmentStatus.Confirmed.ToString()
            });

        var request = new AddHealthRecordDto
        {
            PatientId = requestedPatientId,
            AppointmentId = appointmentId,
            Diagnosis = "Fever",
            Prescription = "Paracetamol"
        };

        // Act + Assert
        await Assert.ThrowsAsync<HealthRecordRuleException>(() =>
            service.AddHealthRecordAsync(request));
    }

    [Fact]
    public async Task AddHealthRecordAsync_WhenAppointmentIsCancelled_ThrowsHealthRecordRuleException()
    {
        // Arrange
        const string doctorUserId = "doctor-user-1";
        const int doctorId = 5;
        const int patientId = 10;
        const int appointmentId = 100;

        var service = CreateService(doctorUserId, Roles.Doctor);

        _doctorRepository.Setup(x => x.GetByUserIdAsync(doctorUserId))
            .ReturnsAsync(new Doctor
            {
                DoctorId = doctorId,
                UserId = doctorUserId,
                DoctorName = "Dr Sneha Paul"
            });

        _appointmentRepository.Setup(x => x.GetByIdAsync(appointmentId))
            .ReturnsAsync(new Appointment
            {
                AppointmentId = appointmentId,
                DoctorId = doctorId,
                PatientId = patientId,
                Status = AppointmentStatus.Cancelled.ToString()
            });

        var request = new AddHealthRecordDto
        {
            PatientId = patientId,
            AppointmentId = appointmentId,
            Diagnosis = "Fever",
            Prescription = "Paracetamol"
        };

        // Act + Assert
        await Assert.ThrowsAsync<HealthRecordRuleException>(() =>
            service.AddHealthRecordAsync(request));
    }
}