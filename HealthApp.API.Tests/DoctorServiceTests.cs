using AutoMapper;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using HealthApp.API.Service.Impl;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Models;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;
using HealthApp.API.Exceptions;

namespace HealthApp.API.Tests
{
    public class DoctorServiceTests
    {
        private readonly Mock<IDoctorRepository> _doctorRepoMock;
        private readonly Mock<IAppointmentRepository> _appointmentRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DoctorService _service;

        public DoctorServiceTests()
        {
            _doctorRepoMock = new Mock<IDoctorRepository>();
            _appointmentRepoMock = new Mock<IAppointmentRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new DoctorService(
                _doctorRepoMock.Object,
                _appointmentRepoMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllDoctorsAsync_ShouldReturnMappedDoctors()
        {
            var doctors = new List<Doctor>
        {
            new Doctor { DoctorId = 1, IsActive = true },
            new Doctor { DoctorId = 2, IsActive = true }
        };

            var doctorDtos = new List<DoctorDto> { new(), new() };

            _doctorRepoMock.Setup(r => r.GetAllActiveAsync())
                .ReturnsAsync(doctors);

            _mapperMock.Setup(m => m.Map<List<DoctorDto>>(doctors))
                .Returns(doctorDtos);

            var result = await _service.GetAllDoctorsAsync();

            Assert.Equal(2, result.Count);
            Assert.Equal(doctorDtos, result);
        }


        [Fact]
        public async Task GetDoctorByIdAsync_ShouldThrow_WhenInvalidId()
        {
            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _service.GetDoctorByIdAsync(0));
        }

        [Fact]
        public async Task GetDoctorByIdAsync_ShouldThrow_WhenDoctorNotFound()
        {
            _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.GetDoctorByIdAsync(1));
        }

        [Fact]
        public async Task GetDoctorByIdAsync_ShouldThrow_WhenDoctorInactive()
        {
            var doctor = new Doctor { DoctorId = 1, IsActive = false };

            _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            await Assert.ThrowsAsync<EntityNotFoundException>(
                () => _service.GetDoctorByIdAsync(1));
        }

        [Fact]
        public async Task GetDoctorByIdAsync_ShouldReturnDoctorDto_WhenValid()
        {
            var doctor = new Doctor { DoctorId = 1, IsActive = true };
            var dto = new DoctorDto { DoctorId = 1 };

            _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _mapperMock.Setup(m => m.Map<DoctorDto>(doctor))
                .Returns(dto);

            var result = await _service.GetDoctorByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.DoctorId);
        }


        [Fact]
        public async Task GetDoctorsBySpecialisationAsync_ShouldReturnMappedList()
        {
            var doctors = new List<Doctor>
        {
            new Doctor { DoctorId = 1, IsActive = true }
        };

            var dtos = new List<DoctorDto>
        {
            new DoctorDto { DoctorId = 1 }
        };

            _doctorRepoMock.Setup(r =>
                    r.GetActiveBySpecialisationAsync(It.IsAny<SpecialisationType>()))
                .ReturnsAsync(doctors);

            _mapperMock.Setup(m => m.Map<List<DoctorDto>>(doctors))
                .Returns(dtos);

            var result = await _service.GetDoctorsBySpecialisationAsync(
                SpecialisationType.Cardiologist);

            Assert.Single(result);
            Assert.Equal(1, result.First().DoctorId);
        }

        [Fact]
        public async Task GetDoctorAvailabilityAsync_ShouldThrow_WhenInvalidId()
        {
            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _service.GetDoctorAvailabilityAsync(0, DateTime.Today));
        }

        [Fact]
        public async Task GetDoctorAvailabilityAsync_ShouldThrow_WhenDoctorNotFound()
        {
            _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Doctor?)null);

            await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _service.GetDoctorAvailabilityAsync(1, DateTime.Today));
        }

        [Fact]
        public async Task GetDoctorAvailabilityAsync_ShouldThrow_WhenDoctorInactive()
        {
            var doctor = new Doctor { DoctorId = 1, IsActive = false };

            _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _service.GetDoctorAvailabilityAsync(1, DateTime.Today));
        }

        [Fact]
        public async Task GetDoctorAvailabilityAsync_ShouldThrow_WhenPastDate()
        {
            var doctor = new Doctor { DoctorId = 1, IsActive = true };

            _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _service.GetDoctorAvailabilityAsync(1, DateTime.Today.AddDays(-1)));
        }

        [Fact]
        public async Task GetDoctorAvailabilityAsync_ShouldReturnAvailableSlots()
        {
            var doctor = new Doctor { DoctorId = 1, IsActive = true };

            var appointments = new List<Appointment>
        {
            new Appointment
            {
                DoctorId = 1,
                ScheduledDate = DateTime.Today,
                Status = "Booked",
                TimeSlots = "10:00 AM - 10:30 AM"
            }
        };

            _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _appointmentRepoMock.Setup(r => r.GetByDoctorIdAsync(1))
                .ReturnsAsync(appointments);

            var result = await _service.GetDoctorAvailabilityAsync(1, DateTime.Today);

            Assert.NotNull(result);
            Assert.Equal(1, result.DoctorId);

            // Booked slot should NOT be available
            Assert.DoesNotContain("10:00 AM - 10:30 AM", result.AvailableSlots);
        }

        [Fact]
        public async Task GetDoctorAvailabilityAsync_ShouldIgnoreCancelledAppointments()
        {
            var doctor = new Doctor { DoctorId = 1, IsActive = true };

            var appointments = new List<Appointment>
        {
            new Appointment
            {
                DoctorId = 1,
                ScheduledDate = DateTime.Today,
                Status = AppointmentStatus.Cancelled.ToString(),
                TimeSlots = "10:00 AM - 10:30 AM"
            }
        };

            _doctorRepoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(doctor);

            _appointmentRepoMock.Setup(r => r.GetByDoctorIdAsync(1))
                .ReturnsAsync(appointments);

            var result = await _service.GetDoctorAvailabilityAsync(1, DateTime.Today);

            // Cancelled slot should still be available
            Assert.Contains("10:00 AM - 10:30 AM", result.AvailableSlots);
        }
    }
}