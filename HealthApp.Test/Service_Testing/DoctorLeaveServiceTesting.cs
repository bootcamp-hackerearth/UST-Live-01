using AutoMapper;
using FluentAssertions;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Service.Impl;
using HealthApp.Shared.Dto;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HealthApp.Test.Service_Testing
{
    public class DoctorLeaveServiceTesting
    {
        private readonly Mock<IDoctorLeaveRepository> _leaveRepositoryMock;
        private readonly Mock<IDoctorRepository> _doctorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<DoctorLeaveService>> _loggerMock;
        private readonly DoctorLeaveService _service;

        public DoctorLeaveServiceTesting()
        {
            _leaveRepositoryMock = new Mock<IDoctorLeaveRepository>();
            _doctorRepositoryMock = new Mock<IDoctorRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<DoctorLeaveService>>();

            _service = new DoctorLeaveService(
                _leaveRepositoryMock.Object,
                _doctorRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task CreateMyLeaveAsync_ShouldCreateLeave_WhenValidData()
        {
            var identityUserId = "user-123";

            var dto = new DoctorLeaveCreateDto
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(2),
                Reason = " Personal leave "
            };

            var doctor = new Doctor
            {
                DoctorId = 1,
                FullName = "Dr John"
            };

            var savedLeave = new DoctorLeave
            {
                DoctorLeaveId = 10,
                DoctorId = 1,
                StartDate = dto.StartDate.Value.Date,
                EndDate = dto.EndDate.Value.Date,
                Reason = "Personal leave",
                CreatedDate = DateTime.UtcNow
            };

            _doctorRepositoryMock
                .Setup(x => x.GetByIdentityUserIdAsync(identityUserId))
                .ReturnsAsync(doctor);

            _leaveRepositoryMock
                .Setup(x => x.HasOverlappingLeaveAsync(
                    doctor.DoctorId,
                    dto.StartDate.Value.Date,
                    dto.EndDate.Value.Date))
                .ReturnsAsync(false);

            _leaveRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<DoctorLeave>()))
                .ReturnsAsync(savedLeave);

            var result = await _service.CreateMyLeaveAsync(dto, identityUserId);

            result.Should().NotBeNull();
            result.DoctorLeaveId.Should().Be(10);
            result.DoctorId.Should().Be(1);
            result.DoctorName.Should().Be("Dr John");
            result.Reason.Should().Be("Personal leave");

            _leaveRepositoryMock.Verify(x => x.AddAsync(It.Is<DoctorLeave>(l =>
                l.DoctorId == 1 &&
                l.StartDate == dto.StartDate.Value.Date &&
                l.EndDate == dto.EndDate.Value.Date &&
                l.Reason == "Personal leave")), Times.Once);
        }

        [Fact]
        public async Task CreateMyLeaveAsync_ShouldThrowBusinessRuleException_WhenDtoIsNull()
        {
            Func<Task> act = async () => await _service.CreateMyLeaveAsync(null!, "user-123");

            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Leave data is required.");
        }

        [Fact]
        public async Task CreateMyLeaveAsync_ShouldThrowBusinessRuleException_WhenStartDateIsNull()
        {
            var dto = new DoctorLeaveCreateDto
            {
                StartDate = null,
                EndDate = DateTime.Today.AddDays(1),
                Reason = "Leave"
            };

            Func<Task> act = async () => await _service.CreateMyLeaveAsync(dto, "user-123");

            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Start date is required.");
        }

        [Fact]
        public async Task CreateMyLeaveAsync_ShouldThrowBusinessRuleException_WhenEndDateIsNull()
        {
            var dto = new DoctorLeaveCreateDto
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = null,
                Reason = "Leave"
            };

            Func<Task> act = async () => await _service.CreateMyLeaveAsync(dto, "user-123");

            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("End date is required.");
        }

        [Fact]
        public async Task CreateMyLeaveAsync_ShouldThrowBusinessRuleException_WhenStartDateIsInPast()
        {
            var dto = new DoctorLeaveCreateDto
            {
                StartDate = DateTime.Today.AddDays(-1),
                EndDate = DateTime.Today.AddDays(1),
                Reason = "Leave"
            };

            Func<Task> act = async () => await _service.CreateMyLeaveAsync(dto, "user-123");

            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Leave start date cannot be in the past.");
        }

        [Fact]
        public async Task CreateMyLeaveAsync_ShouldThrowBusinessRuleException_WhenEndDateBeforeStartDate()
        {
            var dto = new DoctorLeaveCreateDto
            {
                StartDate = DateTime.Today.AddDays(3),
                EndDate = DateTime.Today.AddDays(1),
                Reason = "Leave"
            };

            Func<Task> act = async () => await _service.CreateMyLeaveAsync(dto, "user-123");

            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Leave end date cannot be before start date.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task CreateMyLeaveAsync_ShouldThrowBusinessRuleException_WhenReasonIsInvalid(string? reason)
        {
            var dto = new DoctorLeaveCreateDto
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(2),
                Reason = reason!
            };

            Func<Task> act = async () => await _service.CreateMyLeaveAsync(dto, "user-123");

            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Reason is required.");
        }

        [Fact]
        public async Task CreateMyLeaveAsync_ShouldThrowEntityNotFoundException_WhenDoctorNotFound()
        {
            var identityUserId = "user-123";

            var dto = new DoctorLeaveCreateDto
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(2),
                Reason = "Leave"
            };

            _doctorRepositoryMock
                .Setup(x => x.GetByIdentityUserIdAsync(identityUserId))
                .ReturnsAsync((Doctor?)null);

            Func<Task> act = async () => await _service.CreateMyLeaveAsync(dto, identityUserId);

            await act.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task CreateMyLeaveAsync_ShouldThrowBusinessRuleException_WhenLeaveOverlaps()
        {
            var identityUserId = "user-123";

            var dto = new DoctorLeaveCreateDto
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(2),
                Reason = "Leave"
            };

            var doctor = new Doctor
            {
                DoctorId = 1,
                FullName = "Dr John"
            };

            _doctorRepositoryMock
                .Setup(x => x.GetByIdentityUserIdAsync(identityUserId))
                .ReturnsAsync(doctor);

            _leaveRepositoryMock
                .Setup(x => x.HasOverlappingLeaveAsync(
                    doctor.DoctorId,
                    dto.StartDate.Value.Date,
                    dto.EndDate.Value.Date))
                .ReturnsAsync(true);

            Func<Task> act = async () => await _service.CreateMyLeaveAsync(dto, identityUserId);

            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Leave overlaps with an existing leave range.");
        }

        [Fact]
        public async Task GetMyLeavesAsync_ShouldReturnLeaves_WhenDoctorExists()
        {
            var identityUserId = "user-123";

            var doctor = new Doctor
            {
                DoctorId = 1,
                FullName = "Dr John"
            };

            var leaves = new List<DoctorLeave>
            {
                new DoctorLeave
                {
                    DoctorLeaveId = 1,
                    DoctorId = 1,
                    StartDate = DateTime.Today.AddDays(1),
                    EndDate = DateTime.Today.AddDays(2),
                    Reason = "Leave 1",
                    CreatedDate = DateTime.UtcNow
                },
                new DoctorLeave
                {
                    DoctorLeaveId = 2,
                    DoctorId = 1,
                    StartDate = DateTime.Today.AddDays(3),
                    EndDate = DateTime.Today.AddDays(4),
                    Reason = "Leave 2",
                    CreatedDate = DateTime.UtcNow
                }
            };

            _doctorRepositoryMock
                .Setup(x => x.GetByIdentityUserIdAsync(identityUserId))
                .ReturnsAsync(doctor);

            _leaveRepositoryMock
                .Setup(x => x.GetByDoctorIdAsync(doctor.DoctorId))
                .ReturnsAsync(leaves);

            var result = await _service.GetMyLeavesAsync(identityUserId);

            result.Should().HaveCount(2);
            result[0].DoctorName.Should().Be("Dr John");
            result[1].DoctorName.Should().Be("Dr John");
        }

        [Fact]
        public async Task GetMyLeavesAsync_ShouldThrowEntityNotFoundException_WhenDoctorNotFound()
        {
            var identityUserId = "user-123";

            _doctorRepositoryMock
                .Setup(x => x.GetByIdentityUserIdAsync(identityUserId))
                .ReturnsAsync((Doctor?)null);

            Func<Task> act = async () => await _service.GetMyLeavesAsync(identityUserId);

            await act.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetLeavesByDoctorIdAsync_ShouldThrowBusinessRuleException_WhenDoctorIdInvalid(int doctorId)
        {
            Func<Task> act = async () => await _service.GetLeavesByDoctorIdAsync(doctorId);

            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Invalid doctor id.");
        }

        [Fact]
        public async Task GetLeavesByDoctorIdAsync_ShouldThrowEntityNotFoundException_WhenDoctorNotFound()
        {
            var doctorId = 1;

            _doctorRepositoryMock
                .Setup(x => x.getbyidAsync(doctorId))
                .ReturnsAsync((Doctor?)null);

            Func<Task> act = async () => await _service.GetLeavesByDoctorIdAsync(doctorId);

            await act.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetLeavesByDoctorIdAsync_ShouldReturnLeaves_WhenDoctorExists()
        {
            var doctorId = 1;

            var doctor = new Doctor
            {
                DoctorId = doctorId,
                FullName = "Dr John"
            };

            var leaves = new List<DoctorLeave>
            {
                new DoctorLeave
                {
                    DoctorLeaveId = 1,
                    DoctorId = doctorId,
                    StartDate = DateTime.Today.AddDays(1),
                    EndDate = DateTime.Today.AddDays(2),
                    Reason = "Leave",
                    CreatedDate = DateTime.UtcNow
                }
            };

            _doctorRepositoryMock
                .Setup(x => x.getbyidAsync(doctorId))
                .ReturnsAsync(doctor);

            _leaveRepositoryMock
                .Setup(x => x.GetByDoctorIdAsync(doctorId))
                .ReturnsAsync(leaves);

            var result = await _service.GetLeavesByDoctorIdAsync(doctorId);

            result.Should().HaveCount(1);
            result[0].DoctorId.Should().Be(doctorId);
            result[0].DoctorName.Should().Be("Dr John");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public async Task IsDoctorOnLeaveAsync_ShouldThrowBusinessRuleException_WhenDoctorIdInvalid(int doctorId)
        {
            Func<Task> act = async () => await _service.IsDoctorOnLeaveAsync(doctorId, DateTime.Today);

            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Invalid doctor id.");
        }

        [Fact]
        public async Task IsDoctorOnLeaveAsync_ShouldReturnTrue_WhenDoctorIsOnLeave()
        {
            var doctorId = 1;
            var date = DateTime.Today.AddHours(10);

            _leaveRepositoryMock
                .Setup(x => x.IsDoctorOnLeaveAsync(doctorId, date.Date))
                .ReturnsAsync(true);

            var result = await _service.IsDoctorOnLeaveAsync(doctorId, date);

            result.Should().BeTrue();

            _leaveRepositoryMock.Verify(x =>
                x.IsDoctorOnLeaveAsync(doctorId, date.Date), Times.Once);
        }

        [Fact]
        public async Task IsDoctorOnLeaveAsync_ShouldReturnFalse_WhenDoctorIsNotOnLeave()
        {
            var doctorId = 1;
            var date = DateTime.Today.AddHours(10);

            _leaveRepositoryMock
                .Setup(x => x.IsDoctorOnLeaveAsync(doctorId, date.Date))
                .ReturnsAsync(false);

            var result = await _service.IsDoctorOnLeaveAsync(doctorId, date);

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(-1, 10)]
        public async Task GetAllLeaveDoctorAsync_ShouldThrowBusinessRuleException_WhenPageNumberInvalid(
            int pageNumber,
            int pageSize)
        {
            Func<Task> act = async () => await _service.GetAllLeaveDoctorAsync(pageNumber, pageSize);

            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Invalid page number");
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(1, -10)]
        public async Task GetAllLeaveDoctorAsync_ShouldThrowBusinessRuleException_WhenPageSizeInvalid(
            int pageNumber,
            int pageSize)
        {
            Func<Task> act = async () => await _service.GetAllLeaveDoctorAsync(pageNumber, pageSize);

            await act.Should()
                .ThrowAsync<BusinessRuleException>()
                .WithMessage("Invalid page size");
        }

        [Fact]
        public async Task GetAllLeaveDoctorAsync_ShouldReturnPagedLeaves_WhenValidRequest()
        {
            var pageNumber = 1;
            var pageSize = 10;

            var leaves = new List<DoctorLeave>
            {
                new DoctorLeave
                {
                    DoctorLeaveId = 1,
                    DoctorId = 1,
                    Doctor = new Doctor
                    {
                        DoctorId = 1,
                        FullName = "Dr John"
                    },
                    StartDate = DateTime.Today.AddDays(1),
                    EndDate = DateTime.Today.AddDays(2),
                    Reason = "Leave",
                    CreatedDate = DateTime.UtcNow
                }
            };

            _leaveRepositoryMock
                .Setup(x => x.GetAllLeaveDoctorAsync(pageNumber, pageSize))
                .ReturnsAsync((leaves, 1));

            
            var result = await _service.GetAllLeaveDoctorAsync(pageNumber, pageSize);

            result.Items.Should().HaveCount(1);
            result.TotalCount.Should().Be(1);
            result.Items[0].DoctorLeaveId.Should().Be(1);
            result.Items[0].DoctorName.Should().Be("Dr John");
        }

        [Fact]
        public async Task GetAllLeaveDoctorAsync_ShouldReturnEmptyDoctorName_WhenDoctorNavigationIsNull()
        {
            var pageNumber = 1;
            var pageSize = 10;

            var leaves = new List<DoctorLeave>
            {
                new DoctorLeave
                {
                    DoctorLeaveId = 1,
                    DoctorId = 1,
                    Doctor = null,
                    StartDate = DateTime.Today.AddDays(1),
                    EndDate = DateTime.Today.AddDays(2),
                    Reason = "Leave",
                    CreatedDate = DateTime.UtcNow
                }
            };

            _leaveRepositoryMock
                .Setup(x => x.GetAllLeaveDoctorAsync(pageNumber, pageSize))
                .ReturnsAsync((leaves, 1));

            var result = await _service.GetAllLeaveDoctorAsync(pageNumber, pageSize);

            result.Items.Should().HaveCount(1);
            result.Items[0].DoctorName.Should().Be(string.Empty);
            result.TotalCount.Should().Be(1);
        }
    }
}