using AutoMapper;
using FluentAssertions;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Implementations;
using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.HealthRecord;
using HealthAxis.Shared.DTOs.Patient;
using HealthAxis.Shared.Enums;
using Moq;

namespace Healthcare.netcore.Tests.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IRepository<Patient>> _patientRepositoryMock;
        private readonly Mock<IRepository<Doctor>> _doctorRepositoryMock;
        private readonly Mock<IRepository<Appointment>> _appointmentRepositoryMock;
        private readonly Mock<IRepository<HealthRecord>> _healthRecordRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PatientService _service;

        public PatientServiceTests()
        {
            _patientRepositoryMock = new Mock<IRepository<Patient>>();
            _doctorRepositoryMock = new Mock<IRepository<Doctor>>();
            _appointmentRepositoryMock = new Mock<IRepository<Appointment>>();
            _healthRecordRepositoryMock = new Mock<IRepository<HealthRecord>>();
            _mapperMock = new Mock<IMapper>();

            _service = new PatientService(
                _patientRepositoryMock.Object,
                _doctorRepositoryMock.Object,
                _appointmentRepositoryMock.Object,
                _healthRecordRepositoryMock.Object,
                _mapperMock.Object);
        }

        private static Patient GetPatient()
        {
            return new Patient
            {
                PatientId = 3,
                UserId = "patient-user-1",
                FullName = "Kiran",
                Email = "kiran@gmail.com",
                PhoneNumber = "9876543210",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(2003, 8, 21),
                InsuranceId = "INS1003"
            };
        }

        private static PatientDto GetPatientDto()
        {
            return new PatientDto
            {
                PatientId = 3,
                FullName = "Kiran",
                Email = "kiran@gmail.com",
                PhoneNumber = "9876543210"
            };
        }

        private static  UpdatePatientDto GetValidUpdatePatientDto()
        {
            return new UpdatePatientDto
            {
                FullName = "Kiran Updated",
                Email = "kiran.updated@gmail.com",
                PhoneNumber = "9876543211",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(2003, 8, 21),
                InsuranceId = "INS1003"
            };
        }

        [Fact]
        public async Task GetByIdAsync_WhenPatientExists_ReturnsPatientDto()
        {
            var patient = GetPatient();
            var patientDto = GetPatientDto();

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(3))
                .ReturnsAsync(patient);

            _mapperMock
                .Setup(x => x.Map<PatientDto>(patient))
                .Returns(patientDto);

            var result = await _service.GetByIdAsync(3);

            result.Should().NotBeNull();
            result!.PatientId.Should().Be(3);
            result.FullName.Should().Be("Kiran");
            result.Email.Should().Be("kiran@gmail.com");
        }

        [Fact]
        public async Task GetByIdAsync_WhenPatientDoesNotExist_ThrowsNotFoundException()
        {
            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Patient?)null);

            Func<Task> action = async () => await _service.GetByIdAsync(99);

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Patient not found");
        }

        [Fact]
        public async Task GetByIdAsync_WhenRepositoryFails_ThrowsException()
        {
            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(3))
                .ThrowsAsync(new Exception("Database Error"));

            Func<Task> action = async () => await _service.GetByIdAsync(3);

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database Error");
        }

        [Fact]
        public async Task IsPatientOwnerAsync_WhenUserOwnsPatient_ReturnsTrue()
        {
            var patient = GetPatient();

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(3))
                .ReturnsAsync(patient);

            var result = await _service.IsPatientOwnerAsync(3, "patient-user-1");

            result.Should().BeTrue();
        }

        [Fact]
        public async Task IsPatientOwnerAsync_WhenUserDoesNotOwnPatient_ReturnsFalse()
        {
            var patient = GetPatient();

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(3))
                .ReturnsAsync(patient);

            var result = await _service.IsPatientOwnerAsync(3, "another-user");

            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsPatientOwnerAsync_WhenPatientDoesNotExist_ThrowsNotFoundException()
        {
            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Patient?)null);

            Func<Task> action = async () =>
                await _service.IsPatientOwnerAsync(99, "patient-user-1");

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Patient not found");
        }

        [Fact]
        public async Task GetPagedAsync_WhenPatientsExist_ReturnsPagedPatients()
        {
            var patients = new List<Patient>
            {
                GetPatient(),
                new Patient
                {
                    PatientId = 4,
                    UserId = "patient-user-2",
                    FullName = "Rahul",
                    Email = "rahul@gmail.com",
                    PhoneNumber = "9876543211",
                    Gender = Gender.Male,
                    DateOfBirth = new DateTime(2000, 1, 1)
                }
            };

            var patientDtos = new List<PatientDto>
            {
                GetPatientDto(),
                new PatientDto
                {
                    PatientId = 4,
                    FullName = "Rahul",
                    Email = "rahul@gmail.com",
                    PhoneNumber = "9876543211"
                }
            };

            _patientRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<PatientDto>>(It.IsAny<IEnumerable<Patient>>()))
                .Returns(patientDtos);

            var result = await _service.GetPagedAsync(new PaginationParams
            {
                PageNumber = 1,
                PageSize = 10
            });

            result.Should().NotBeNull();
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.TotalRecords.Should().Be(2);
            result.TotalPages.Should().Be(1);
            result.Data.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetPagedAsync_WhenInvalidPaginationValues_UsesDefaultValues()
        {
            var patients = new List<Patient>
            {
                GetPatient()
            };

            var patientDtos = new List<PatientDto>
            {
                GetPatientDto()
            };

            _patientRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<PatientDto>>(It.IsAny<IEnumerable<Patient>>()))
                .Returns(patientDtos);

            var result = await _service.GetPagedAsync(new PaginationParams
            {
                PageNumber = 0,
                PageSize = 0
            });

            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.TotalRecords.Should().Be(1);
        }

        [Fact]
        public async Task GetDoctorPatientsAsync_WhenDoctorProfileNotFound_ThrowsNotFoundException()
        {
            _doctorRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Doctor>());

            Func<Task> action = async () =>
                await _service.GetDoctorPatientsAsync(
                    "doctor-user-1",
                    new PaginationParams());

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Doctor profile not found");
        }

        [Fact]
        public async Task GetDoctorPatientsAsync_WhenDoctorHasAppointments_ReturnsAssignedPatients()
        {
            var doctors = new List<Doctor>
            {
                new Doctor
                {
                    DoctorId = 6,
                    UserId = "doctor-user-1",
                    FullName = "Dr Nevin"
                }
            };

            var appointments = new List<Appointment>
            {
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 3,
                    DoctorId = 6
                }
            };

            var patients = new List<Patient>
            {
                GetPatient()
            };

            var patientDtos = new List<PatientDto>
            {
                GetPatientDto()
            };

            _doctorRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(doctors);

            _appointmentRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(appointments);

            _patientRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(patients);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<PatientDto>>(It.IsAny<IEnumerable<Patient>>()))
                .Returns(patientDtos);

            var result = await _service.GetDoctorPatientsAsync(
                "doctor-user-1",
                new PaginationParams
                {
                    PageNumber = 1,
                    PageSize = 10
                });

            result.Should().NotBeNull();
            result.TotalRecords.Should().Be(1);
            result.Data.Should().HaveCount(1);
            result.Data.First().PatientId.Should().Be(3);
        }

        [Fact]
        public async Task GetDoctorPatientsAsync_WhenDoctorHasNoAppointments_ReturnsEmptyData()
        {
            var doctors = new List<Doctor>
            {
                new Doctor
                {
                    DoctorId = 6,
                    UserId = "doctor-user-1",
                    FullName = "Dr Nevin"
                }
            };

            _doctorRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(doctors);

            _appointmentRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Appointment>());

            _patientRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient>());

            _mapperMock
                .Setup(x => x.Map<IEnumerable<PatientDto>>(It.IsAny<IEnumerable<Patient>>()))
                .Returns(new List<PatientDto>());

            var result = await _service.GetDoctorPatientsAsync(
                "doctor-user-1",
                new PaginationParams
                {
                    PageNumber = 1,
                    PageSize = 10
                });

            result.Should().NotBeNull();
            result.TotalRecords.Should().Be(0);
            result.Data.Should().BeEmpty();
        }

        [Fact]
        public async Task UpdateAsync_WhenPatientExists_ReturnsUpdatedPatientDto()
        {
            var patient = GetPatient();

            var updateDto = GetValidUpdatePatientDto();

            var updatedDto = new PatientDto
            {
                PatientId = 3,
                FullName = "Kiran Updated",
                Email = "kiran.updated@gmail.com",
                PhoneNumber = "9876543211"
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(3))
                .ReturnsAsync(patient);

            _patientRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient> { patient });

            _patientRepositoryMock
                .Setup(x => x.UpdateAsync(3, patient, It.IsAny<CancellationToken>()))
                .ReturnsAsync(patient);

            _mapperMock
                .Setup(x => x.Map<PatientDto>(patient))
                .Returns(updatedDto);

            var result = await _service.UpdateAsync(3, updateDto);

            result.Should().NotBeNull();
            result.PatientId.Should().Be(3);
            result.FullName.Should().Be("Kiran Updated");
            result.Email.Should().Be("kiran.updated@gmail.com");

            _patientRepositoryMock.Verify(
                x => x.UpdateAsync(3, patient, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenPatientDoesNotExist_ThrowsNotFoundException()
        {
            var updateDto = GetValidUpdatePatientDto();

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Patient?)null);

            Func<Task> action = async () => await _service.UpdateAsync(99, updateDto);

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Patient not found");
        }

        [Fact]
        public async Task UpdateAsync_WhenRepositoryFails_ThrowsException()
        {
            var patient = GetPatient();

            var updateDto = GetValidUpdatePatientDto();

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(3))
                .ReturnsAsync(patient);

            _patientRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient> { patient });

            _patientRepositoryMock
                .Setup(x => x.UpdateAsync(3, patient, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database Error"));

            Func<Task> action = async () => await _service.UpdateAsync(3, updateDto);

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database Error");
        }

        [Fact]
        public async Task GetHealthRecordsAsync_WhenPatientExists_ReturnsHealthRecords()
        {
            var patient = GetPatient();

            var healthRecords = new List<HealthRecord>
            {
                new HealthRecord
                {
                    RecordId = 1,
                    PatientId = 3,
                    DoctorId = 6,
                    AppointmentId = 1,
                    VisitDate = DateTime.Today,
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Take rest"
                }
            };

            var healthRecordDtos = new List<HealthRecordDto>
            {
                new HealthRecordDto
                {
                    RecordId = 1,
                    PatientId = 3,
                    DoctorId = 6,
                    AppointmentId = 1,
                    VisitDate = DateTime.Today,
                    Diagnosis = "Fever",
                    Prescription = "Paracetamol",
                    Notes = "Take rest"
                }
            };

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(3))
                .ReturnsAsync(patient);

            _healthRecordRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(healthRecords);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<HealthRecordDto>>(
                    It.IsAny<IEnumerable<HealthRecord>>()))
                .Returns(healthRecordDtos);

            var result = await _service.GetHealthRecordsAsync(3);

            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result.First().Diagnosis.Should().Be("Fever");
        }

        [Fact]
        public async Task GetHealthRecordsAsync_WhenPatientDoesNotExist_ThrowsNotFoundException()
        {
            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync((Patient?)null);

            Func<Task> action = async () => await _service.GetHealthRecordsAsync(99);

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Patient not found");
        }

        [Fact]
        public async Task GetHealthRecordsAsync_WhenPatientExistsButNoHealthRecords_ReturnsEmptyCollection()
        {
            var patient = GetPatient();

            var healthRecords = new List<HealthRecord>();

            var healthRecordDtos = new List<HealthRecordDto>();

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(3))
                .ReturnsAsync(patient);

            _healthRecordRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(healthRecords);

            _mapperMock
                .Setup(x => x.Map<IEnumerable<HealthRecordDto>>(
                    It.IsAny<IEnumerable<HealthRecord>>()))
                .Returns(healthRecordDtos);

            var result = await _service.GetHealthRecordsAsync(3);

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetHealthRecordsAsync_WhenRepositoryFails_ThrowsException()
        {
            var patient = GetPatient();

            _patientRepositoryMock
                .Setup(x => x.GetByIdAsync(3))
                .ReturnsAsync(patient);

            _healthRecordRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ThrowsAsync(new Exception("Database Error"));

            Func<Task> action = async () => await _service.GetHealthRecordsAsync(3);

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database Error");
        }
        [Fact]
        public async Task AddAsync_WhenValidPatient_ReturnsPatientDto()
        {
            var createDto = new CreatePatientDto
            {
                FullName = "Anu",
                Email = "anu@gmail.com",
                PhoneNumber = "9876543222",
                Gender = Gender.Female,
                DateOfBirth = new DateTime(2002, 5, 10)
            };

            var savedPatient = new Patient
            {
                PatientId = 10,
                FullName = "Anu",
                Email = "anu@gmail.com",
                PhoneNumber = "9876543222",
                Gender = Gender.Female,
                DateOfBirth = new DateTime(2002, 5, 10)
            };

            var patientDto = new PatientDto
            {
                PatientId = 10,
                FullName = "Anu",
                Email = "anu@gmail.com",
                PhoneNumber = "9876543222"
            };

            _patientRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient>());

            _patientRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Patient>()))
                .ReturnsAsync(savedPatient);

            _mapperMock
                .Setup(x => x.Map<PatientDto>(savedPatient))
                .Returns(patientDto);

            var result = await _service.AddAsync(createDto);

            result.Should().NotBeNull();
            result.PatientId.Should().Be(10);
            result.FullName.Should().Be("Anu");
            result.Email.Should().Be("anu@gmail.com");
        }
        [Fact]
        public async Task AddAsync_WhenDateOfBirthIsFuture_ThrowsValidationException()
        {
            var createDto = new CreatePatientDto
            {
                FullName = "Future Patient",
                Email = "future@gmail.com",
                PhoneNumber = "9876543333",
                Gender = Gender.Male,
                DateOfBirth = DateTime.Today.AddDays(1)
            };

            Func<Task> action = async () => await _service.AddAsync(createDto);

            await action.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Date of birth cannot be in the future.");
        }
        [Fact]
        public async Task AddAsync_WhenDateOfBirthBefore1900_ThrowsValidationException()
        {
            var createDto = new CreatePatientDto
            {
                FullName = "Old Patient",
                Email = "old@gmail.com",
                PhoneNumber = "9876543444",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(1800, 1, 1)
            };

            Func<Task> action = async () => await _service.AddAsync(createDto);

            await action.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Date of birth must be after 01 Jan 1900.");
        }
        [Fact]
        public async Task AddAsync_WhenEmailAlreadyExists_ThrowsValidationException()
        {
            var existingPatient = GetPatient();

            var createDto = new CreatePatientDto
            {
                FullName = "Another Patient",
                Email = "kiran@gmail.com",
                PhoneNumber = "9876543999",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(2001, 1, 1)
            };

            _patientRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient> { existingPatient });

            Func<Task> action = async () => await _service.AddAsync(createDto);

            await action.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Patient email already exists.");
        }
        [Fact]
        public async Task AddAsync_WhenPhoneNumberAlreadyExists_ThrowsValidationException()
        {
            var existingPatient = GetPatient();

            var createDto = new CreatePatientDto
            {
                FullName = "Another Patient",
                Email = "another@gmail.com",
                PhoneNumber = "9876543210",
                Gender = Gender.Male,
                DateOfBirth = new DateTime(2001, 1, 1)
            };

            _patientRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient> { existingPatient });

            Func<Task> action = async () => await _service.AddAsync(createDto);

            await action.Should()
                .ThrowAsync<ValidationException>()
                .WithMessage("Patient phone number already exists.");
        }
        [Fact]
        public async Task AddAsync_WhenRepositoryFails_ThrowsException()
        {
            var createDto = new CreatePatientDto
            {
                FullName = "Anu",
                Email = "anu@gmail.com",
                PhoneNumber = "9876543222",
                Gender = Gender.Female,
                DateOfBirth = new DateTime(2002, 5, 10)
            };

            _patientRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient>());

            _patientRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Patient>()))
                .ThrowsAsync(new Exception("Database Error"));

            Func<Task> action = async () => await _service.AddAsync(createDto);

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database Error");
        }
        [Fact]
        public async Task GetByUserIdAsync_WhenPatientExists_ReturnsPatientDto()
        {
            var patient = GetPatient();
            var patientDto = GetPatientDto();

            _patientRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient> { patient });

            _mapperMock
                .Setup(x => x.Map<PatientDto>(patient))
                .Returns(patientDto);

            var result = await _service.GetByUserIdAsync("patient-user-1");

            result.Should().NotBeNull();
            result!.PatientId.Should().Be(3);
            result.FullName.Should().Be("Kiran");
        }
        [Fact]
        public async Task GetByUserIdAsync_WhenPatientDoesNotExist_ThrowsNotFoundException()
        {
            _patientRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient>());

            Func<Task> action = async () => await _service.GetByUserIdAsync("missing-user");

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Patient profile not found.");
        }
        [Fact]
        public async Task GetByUserIdAsync_WhenRepositoryFails_ThrowsException()
        {
            _patientRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ThrowsAsync(new Exception("Database Error"));

            Func<Task> action = async () => await _service.GetByUserIdAsync("patient-user-1");

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database Error");
        }
        [Fact]
        public async Task GetPagedAsync_WhenRepositoryFails_ThrowsException()
        {
            _patientRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ThrowsAsync(new Exception("Database Error"));

            Func<Task> action = async () => await _service.GetPagedAsync(new PaginationParams
            {
                PageNumber = 1,
                PageSize = 10
            });

            await action.Should()
                .ThrowAsync<Exception>()
                .WithMessage("Database Error");
        }
    }

}