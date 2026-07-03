using Xunit;
using Moq;
using FluentAssertions;
using HealthApp.Api.Service.Impl;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Model;
using HealthApp.Shared.Dto;
using AutoMapper;
using HealthApp.Api.Exceptions;

namespace HealthApp.Test.Service_Testing
{
    public class DoctorServiceTesting
    {
        private readonly Mock<IDoctorRepository> _repo;
        private readonly Mock<IMapper> _mapper;
        private readonly DoctorService _service;

        public DoctorServiceTesting()
        {
            _repo = new Mock<IDoctorRepository>();
            _mapper = new Mock<IMapper>();
            _service = new DoctorService(_repo.Object, _mapper.Object);
        }

        [Fact]
        public async Task AddDoctor_ShouldCreateDoctor()
        {
            var dto = new DoctorDto
            {
                FullName = "Test",
                Email = "test@mail.com",
                Specialisation = "Cardio",
                PracticeStartDate = DateTime.Now,
                ConsultationFee = 500,
                DoctorPhoneNumber = "123456"
            };

            _repo.Setup(x => x.getallAsync())
                .ReturnsAsync(new List<Doctor>());

            _mapper.Setup(x => x.Map<Doctor>(dto))
                .Returns(new Doctor());

            _repo.Setup(x => x.addAsync(It.IsAny<Doctor>()))
                .ReturnsAsync(new Doctor());

            _mapper.Setup(x => x.Map<DoctorDto>(It.IsAny<Doctor>()))
                .Returns(dto);

            var result = await _service.AddDoctorAsync(dto);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task AddDoctor_ShouldThrow_WhenEmailExists()
        {
            var dto = new DoctorDto
            {
                FullName = "Test",
                Email = "test@mail.com",
                Specialisation = "Cardio",
                PracticeStartDate = DateTime.Now,
                ConsultationFee = 500,
                DoctorPhoneNumber = "123456"
            };

            var existing = new List<Doctor>
            {
                new Doctor { Email = "test@mail.com" }
            };

            _repo.Setup(x => x.getallAsync()).ReturnsAsync(existing);

            await Assert.ThrowsAsync<ConflictException>(() =>
                _service.AddDoctorAsync(dto));
        }

        [Fact]
        public async Task GetDoctorById_ShouldReturnDoctor()
        {
            var doctor = new Doctor { DoctorId = 1 };

            _repo.Setup(x => x.getbyidAsync(1))
                .ReturnsAsync(doctor);

            _mapper.Setup(x => x.Map<DoctorDto>(doctor))
                .Returns(new DoctorDto());

            var result = await _service.GetDoctorByIdAsync(1);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetDoctorById_ShouldThrow_WhenInvalidId()
        {
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _service.GetDoctorByIdAsync(0));
        }

        [Fact]
        public async Task GetDoctorById_ShouldThrow_WhenNotFound()
        {
            _repo.Setup(x => x.getbyidAsync(1))
                .ReturnsAsync((Doctor)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _service.GetDoctorByIdAsync(1));
        }

        [Fact]
        public async Task UpdateDoctor_ShouldUpdateSuccessfully()
        {
            var existing = new Doctor { DoctorId = 1 };

            var dto = new DoctorDto
            {
                FullName = "Updated",
                Email = "new@mail.com",
                Specialisation = "Neuro",
                PracticeStartDate = DateTime.Now,
                ConsultationFee = 1000,
                DoctorPhoneNumber = "99999",
                IsActive = true
            };

            _repo.Setup(x => x.getbyidAsync(1))
                .ReturnsAsync(existing);

            _repo.Setup(x => x.getallAsync())
                .ReturnsAsync(new List<Doctor>());

            _repo.Setup(x => x.updateAsync(1, existing))
                .ReturnsAsync(existing);

            _mapper.Setup(x => x.Map<DoctorDto>(existing))
                .Returns(dto);

            var result = await _service.UpdateDoctorByIdAsync(1, dto);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateDoctor_ShouldThrow_WhenNotFound()
        {
            _repo.Setup(x => x.getbyidAsync(1))
                .ReturnsAsync((Doctor)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _service.UpdateDoctorByIdAsync(1, new DoctorDto
                {
                    FullName = "Test",
                    Email = "test@mail.com",
                    Specialisation = "Cardio",
                    PracticeStartDate = DateTime.Now,
                    ConsultationFee = 100,
                    DoctorPhoneNumber = "123"
                }));
        }

        [Fact]
        public async Task UpdateDoctor_ShouldThrow_WhenDuplicateEmail()
        {
            var dto = new DoctorDto
            {
                FullName = "New",
                Email = "dup@mail.com",
                Specialisation = "Cardio",
                PracticeStartDate = DateTime.Now,
                ConsultationFee = 100,
                DoctorPhoneNumber = "123"
            };

            var existingDoctor = new Doctor { DoctorId = 1 };

            var list = new List<Doctor>
            {
                new Doctor { DoctorId = 2, Email = "dup@mail.com" }
            };

            _repo.Setup(x => x.getbyidAsync(1)).ReturnsAsync(existingDoctor);
            _repo.Setup(x => x.getallAsync()).ReturnsAsync(list);

            await Assert.ThrowsAsync<ConflictException>(() =>
                _service.UpdateDoctorByIdAsync(1, dto));
        }

        [Fact]
        public async Task GetMyProfile_ShouldReturn()
        {
            var doctor = new Doctor();

            _repo.Setup(x => x.GetByIdentityUserIdAsync("user1"))
                .ReturnsAsync(doctor);

            _mapper.Setup(x => x.Map<DoctorDto>(doctor))
                .Returns(new DoctorDto());

            var result = await _service.GetMyProfileAsync("user1");

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetMyProfile_ShouldThrow_WhenInvalid()
        {
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _service.GetMyProfileAsync(""));
        }

        [Fact]
        public async Task GetPagedDoctors_ShouldReturn()
        {
            var list = new List<Doctor>();

            _repo.Setup(x => x.GetPagedAsync(1, 5))
                .ReturnsAsync((list, 0));

            _mapper.Setup(x => x.Map<List<DoctorDto>>(list))
                .Returns(new List<DoctorDto>());

            var result = await _service.GetPagedDoctorsAsync(1, 5);

            result.Items.Should().NotBeNull();
        }

        [Fact]
        public async Task GetPagedDoctors_ShouldThrow_WhenInvalidPage()
        {
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _service.GetPagedDoctorsAsync(0, 5));
        }

        [Fact]
        public async Task SearchBySpecialisation_ShouldReturn()
        {
            var list = new List<Doctor>();

            _repo.Setup(x => x.SearchBySpecialisationPagedAsync("Cardio", 1, 5))
                .ReturnsAsync((list, 0));

            _mapper.Setup(x => x.Map<List<DoctorDto>>(list))
                .Returns(new List<DoctorDto>());

            var result = await _service.SearchBySpecialisationPagedAsync("Cardio", 1, 5);

            result.Items.Should().NotBeNull();
        }

        [Fact]
        public async Task Search_ShouldThrow_WhenEmpty()
        {
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _service.SearchBySpecialisationPagedAsync("", 1, 5));
        }

        [Fact]
        public async Task AddDoctor_ShouldThrow_WhenDtoIsNull()
        {
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _service.AddDoctorAsync(null));
        }

        [Fact]
        public async Task AddDoctor_ShouldThrow_WhenNameMissing()
        {
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _service.AddDoctorAsync(new DoctorDto
                {
                    Email = "a@test.com",
                    Specialisation = "Cardio",
                    PracticeStartDate = DateTime.Now,
                    ConsultationFee = 500,
                    DoctorPhoneNumber = "123"
                }));
        }

        [Fact]
        public async Task AddDoctor_ShouldThrow_WhenEmailMissing()
        {
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _service.AddDoctorAsync(new DoctorDto
                {
                    FullName = "Doctor",
                    Specialisation = "Cardio",
                    PracticeStartDate = DateTime.Now,
                    ConsultationFee = 500,
                    DoctorPhoneNumber = "123"
                }));
        }

        [Fact]
        public async Task AddDoctor_ShouldThrow_WhenSpecialisationMissing()
        {
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _service.AddDoctorAsync(new DoctorDto
                {
                    FullName = "Doctor",
                    Email = "a@test.com",
                    PracticeStartDate = DateTime.Now,
                    ConsultationFee = 500,
                    DoctorPhoneNumber = "123"
                }));
        }

        [Fact]
        public async Task AddDoctor_ShouldThrow_WhenPracticeStartDateMissing()
        {
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _service.AddDoctorAsync(new DoctorDto
                {
                    FullName = "Doctor",
                    Email = "a@test.com",
                    Specialisation = "Cardio",
                    ConsultationFee = 500,
                    DoctorPhoneNumber = "123"
                }));
        }

        [Fact]
        public async Task AddDoctor_ShouldThrow_WhenPhoneMissing()
        {
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _service.AddDoctorAsync(new DoctorDto
                {
                    FullName = "Doctor",
                    Email = "a@test.com",
                    Specialisation = "Cardio",
                    PracticeStartDate = DateTime.Now,
                    ConsultationFee = 500
                }));
        }

        [Fact]
        public async Task UpdateDoctor_ShouldThrow_WhenUpdatedDoctorNull()
        {
            var dto = new DoctorDto
            {
                FullName = "Doctor",
                Email = "a@test.com",
                Specialisation = "Cardio",
                PracticeStartDate = DateTime.Now,
                ConsultationFee = 100,
                DoctorPhoneNumber = "123"
            };

            _repo.Setup(x => x.getbyidAsync(1))
                .ReturnsAsync(new Doctor { DoctorId = 1 });

            _repo.Setup(x => x.getallAsync())
                .ReturnsAsync(new List<Doctor>());

            _repo.Setup(x => x.updateAsync(1, It.IsAny<Doctor>()))
                .ReturnsAsync((Doctor)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _service.UpdateDoctorByIdAsync(1, dto));
        }











    }
}