using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HealthApp.Api.Services.Impl;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Models;
using HealthApp.Api.Exceptions;
using HealthApp.Shared.Dtos;

namespace HealthApp.Api.Tests.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PatientService _patientService;

        public PatientServiceTests()
        {
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _mapperMock = new Mock<IMapper>();

            _patientService = new PatientService(
                _patientRepositoryMock.Object,
                _mapperMock.Object);
        }

        private static PatientCreateDto GetValidDto() => new()
        {
            FullName = "John Doe",
            DateOfBirth = new DateOnly(1998, 1, 1),
            Gender = "Male",
            PhoneNumber = "9876543210",
            Email = "patient@test.com",
            InsuranceId = "INS001"
        };


        [Fact]
        public async Task GetAll_ShouldReturnPatients()
        {
            var patients = new List<Patient>
            {
                new Patient { PatientId = 1, FullName = "John Doe" }
            };

            _patientRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(patients);

            _mapperMock.Setup(x =>
                x.Map<IEnumerable<PatientDto>>(patients))
                .Returns(new List<PatientDto>
                {
                    new PatientDto { FullName = "John Doe" }
                });

            var result = await _patientService.GetAllPatientsAsync();

            Assert.Single(result);
        }


        [Fact]
        public async Task GetById_Invalid_ShouldThrow()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _patientService.GetPatientByIdAsync(0));
        }

        [Fact]
        public async Task GetById_NotFound_ShouldThrow()
        {
            _patientRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Patient?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _patientService.GetPatientByIdAsync(1));
        }

        [Fact]
        public async Task GetById_Valid_ShouldReturnDto()
        {
            var patient = new Patient { PatientId = 1 };

            _patientRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _mapperMock.Setup(x => x.Map<PatientDto>(patient))
                .Returns(new PatientDto { PatientId = 1 });

            var result = await _patientService.GetPatientByIdAsync(1);

            Assert.Equal(1, result.PatientId);
        }


        [Fact]
        public async Task Register_NullDto_ShouldThrow()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _patientService.RegisterPatientAsync(null!));
        }

        [Fact]
        public async Task Register_EmptyName_ShouldThrow()
        {
            var dto = GetValidDto();
            dto.FullName = "";

            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _patientService.RegisterPatientAsync(dto));
        }

        [Fact]
        public async Task Register_FutureDob_ShouldThrow()
        {
            var dto = GetValidDto();
            dto.DateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

            await Assert.ThrowsAsync<BusinessRuleViolationException>(() =>
                _patientService.RegisterPatientAsync(dto));
        }

        [Fact]
        public async Task Register_EmailExists_ShouldThrow()
        {
            var dto = GetValidDto();

            _patientRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient>
                {
                    new Patient { Email = dto.Email }
                });

            await Assert.ThrowsAsync<DuplicateEntityException>(() =>
                _patientService.RegisterPatientAsync(dto));
        }

        [Fact]
        public async Task Register_DuplicatePatient_ShouldThrow()
        {
            var dto = GetValidDto();

            _patientRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient>());

            _patientRepositoryMock.Setup(x => x.IsDuplicatePatient(
                dto.FullName,
                dto.DateOfBirth!.Value.ToDateTime(TimeOnly.MinValue),
                dto.Email))
            .ReturnsAsync(true);

            await Assert.ThrowsAsync<DuplicateEntityException>(() =>
                _patientService.RegisterPatientAsync(dto));
        }

        [Fact]
        public async Task Register_Valid_ShouldAdd()
        {
            var dto = GetValidDto();

            _patientRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient>());

            _patientRepositoryMock.Setup(x => x.IsDuplicatePatient(
                dto.FullName,
                dto.DateOfBirth!.Value.ToDateTime(TimeOnly.MinValue),
                dto.Email))
            .ReturnsAsync(false);

            _mapperMock.Setup(x => x.Map<Patient>(dto))
                .Returns(new Patient());

            await _patientService.RegisterPatientAsync(dto);

            _patientRepositoryMock.Verify(x => x.Add(It.IsAny<Patient>()), Times.Once);
        }

        [Fact]
        public async Task Register_EmailIgnoreCase_ShouldThrow()
        {
            var dto = GetValidDto();
            dto.Email = "TEST@TEST.COM";

            _patientRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient>
                {
                    new Patient { Email = "test@test.com" }
                });

            await Assert.ThrowsAsync<DuplicateEntityException>(() =>
                _patientService.RegisterPatientAsync(dto));
        }


        [Fact]
        public async Task Update_InvalidId_ShouldThrow()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _patientService.UpdatePatientAsync(0, GetValidDto()));
        }

        [Fact]
        public async Task Update_DtoNull_ShouldThrow()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _patientService.UpdatePatientAsync(1, null!));
        }

        [Fact]
        public async Task Update_NotFound_ShouldThrow()
        {
            _patientRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync((Patient?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _patientService.UpdatePatientAsync(1, GetValidDto()));
        }

        [Fact]
        public async Task Update_EmailConflict_ShouldThrow()
        {
            var dto = GetValidDto();

            _patientRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(new Patient { PatientId = 1 });

            _patientRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient>
                {
                    new Patient { PatientId = 2, Email = dto.Email }
                });

            await Assert.ThrowsAsync<DuplicateEntityException>(() =>
                _patientService.UpdatePatientAsync(1, dto));
        }

        [Fact]
        public async Task Update_Valid_ShouldUpdate()
        {
            var dto = GetValidDto();

            var patient = new Patient { PatientId = 1 };

            _patientRepositoryMock.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(patient);

            _patientRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<Patient>());

            await _patientService.UpdatePatientAsync(1, dto);

            _patientRepositoryMock.Verify(x =>
                x.Update(1, It.IsAny<Patient>()), Times.Once);
        }


        [Fact]
        public async Task Search_Valid_ShouldReturnPaged()
        {
            var patients = new List<Patient>
            {
                new Patient { PatientId = 1, FullName = "John" }
            };

            _patientRepositoryMock.Setup(x =>
                x.GetPatientsAsync("John", "test@test.com", 1, 10))
                .ReturnsAsync((patients, 1));

            _mapperMock.Setup(x => x.Map<List<PatientDto>>(patients))
                .Returns(new List<PatientDto>
                {
                    new PatientDto { PatientId = 1 }
                });

            var result = await _patientService.SearchPatientsAsync("John", "test@test.com", 1, 10);

            Assert.Single(result.Items);
            Assert.Equal(1, result.TotalCount);
        }

        [Fact]
        public async Task Search_InvalidPage_ShouldThrow()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _patientService.SearchPatientsAsync(null, null, 0, 10));
        }

        [Fact]
        public async Task Search_InvalidPageSize_ShouldThrow()
        {
            await Assert.ThrowsAsync<InvalidRequestException>(() =>
                _patientService.SearchPatientsAsync(null, null, 1, 101));
        }
    }
}