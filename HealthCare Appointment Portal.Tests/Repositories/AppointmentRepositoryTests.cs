using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Repositories;
using HealthCare_Appointment_Portal.Tests.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HealthCare_Appointment_Portal.Tests.Repositories
{
    public class AppointmentRepositoryTests
    {
        private readonly Mock<ApplicationDbContext> _contextMock;
        private readonly Mock<DbSet<Appointment>> _appointmentDbSetMock;
        private readonly AppointmentRepository _repository;

        public AppointmentRepositoryTests()
        {
            _contextMock =
                new Mock<ApplicationDbContext>();

            _appointmentDbSetMock =
                new Mock<DbSet<Appointment>>();

            _contextMock
                .Setup(c => c.Appointments)
                .Returns(_appointmentDbSetMock.Object);

            _repository =
                new AppointmentRepository(
                    _contextMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsAppointment()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1
                };

            _appointmentDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync(appointment);

            var result =
                await _repository.GetByIdAsync(1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull()
        {
            _appointmentDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync((Appointment)null!);

            var result =
                await _repository.GetByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task AddAsync_AddsAppointment()
        {
            var appointment =
                new Appointment();

            await _repository.AddAsync(
                appointment);

            _appointmentDbSetMock.Verify(
                d => d.Add(appointment),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_RemovesAppointment()
        {
            var appointment =
                new Appointment
                {
                    AppointmentId = 1
                };

            _appointmentDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync(appointment);

            await _repository.DeleteAsync(1);

            _appointmentDbSetMock.Verify(
                d => d.Remove(appointment),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenNotFound_DoesNothing()
        {
            _appointmentDbSetMock
                .Setup(d => d.FindAsync(1))
                .ReturnsAsync((Appointment)null!);

            await _repository.DeleteAsync(1);

            _appointmentDbSetMock.Verify(
                d => d.Remove(
                    It.IsAny<Appointment>()),
                Times.Never);
        }
    }
}