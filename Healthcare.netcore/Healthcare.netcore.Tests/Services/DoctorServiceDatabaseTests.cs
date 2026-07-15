using FluentAssertions;
using HealthAxis.API.Data;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Services.Implementations;
using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Doctor;
using HealthAxis.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Healthcare.netcore.Tests.Services
{
    public class DoctorServiceDatabaseTests
    {
        private static HealthAxisDbContext CreateDbContext()
        {
            var options =
                new DbContextOptionsBuilder<HealthAxisDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            return new HealthAxisDbContext(options);
        }

        private static Doctor CreateDoctor(
            int doctorId,
            string userId,
            bool isActive = true)
        {
            return new Doctor
            {
                DoctorId = doctorId,
                UserId = userId,
                FullName = $"Doctor {doctorId}",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 5,
                ConsultationFee = 800,
                IsActive = isActive,
                Appointments = new List<Appointment>()
            };
        }

        [Fact]
        public async Task GetAllAsync_WhenDatabaseContainsDoctors_ReturnsDoctorDtos()
        {
            await using var context = CreateDbContext();

            context.Doctors.AddRange(
                CreateDoctor(1, "doctor-user-1"),
                CreateDoctor(2, "doctor-user-2"));

            await context.SaveChangesAsync();

            var service = new DoctorService(context);

            var result = await service.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);

            result
                .Select(doctor => doctor.DoctorId)
                .Should()
                .Contain(new[] { 1, 2 });
        }

        [Fact]
        public async Task GetAllAsync_WhenDatabaseIsEmpty_ReturnsEmptyCollection()
        {
            await using var context = CreateDbContext();

            var service = new DoctorService(context);

            var result = await service.GetAllAsync();

            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_WithPagination_ReturnsRequestedPage()
        {
            await using var context = CreateDbContext();

            context.Doctors.AddRange(
                CreateDoctor(1, "doctor-user-1"),
                CreateDoctor(2, "doctor-user-2"),
                CreateDoctor(3, "doctor-user-3"));

            await context.SaveChangesAsync();

            var service = new DoctorService(context);

            var pagination = new PaginationParams
            {
                PageNumber = 2,
                PageSize = 2
            };

            var result = await service.GetAllAsync(
                pagination,
                CancellationToken.None);

            result.PageNumber.Should().Be(2);
            result.PageSize.Should().Be(2);
            result.TotalRecords.Should().Be(3);
            result.TotalPages.Should().Be(2);
            result.Items.Should().ContainSingle();
            result.Items.First().DoctorId.Should().Be(3);
        }

        [Fact]
        public async Task GetAllAsync_WhenPaginationInvalid_UsesDefaultValues()
        {
            await using var context = CreateDbContext();

            context.Doctors.Add(
                CreateDoctor(1, "doctor-user-1"));

            await context.SaveChangesAsync();

            var service = new DoctorService(context);

            var pagination = new PaginationParams
            {
                PageNumber = 0,
                PageSize = 0
            };

            var result = await service.GetAllAsync(
                pagination,
                CancellationToken.None);

            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
            result.TotalRecords.Should().Be(1);
            result.TotalPages.Should().Be(1);
            result.Items.Should().ContainSingle();
        }

        [Fact]
        public async Task GetByIdAsync_WhenDoctorExists_ReturnsDoctorDto()
        {
            await using var context = CreateDbContext();

            context.Doctors.Add(
                CreateDoctor(6, "doctor-user-6"));

            await context.SaveChangesAsync();

            var service = new DoctorService(context);

            var result = await service.GetByIdAsync(
                6,
                CancellationToken.None);

            result.Should().NotBeNull();
            result!.DoctorId.Should().Be(6);
            result.FullName.Should().Be("Doctor 6");
            result.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task GetByIdAsync_WhenDoctorDoesNotExist_ThrowsNotFoundException()
        {
            await using var context = CreateDbContext();

            var service = new DoctorService(context);

            Func<Task> action = async () =>
                await service.GetByIdAsync(
                    99,
                    CancellationToken.None);

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Doctor not found");
        }

        [Fact]
        public async Task GetByUserIdAsync_WhenDoctorExists_ReturnsDoctorDto()
        {
            await using var context = CreateDbContext();

            context.Doctors.Add(
                CreateDoctor(6, "doctor-user-6"));

            await context.SaveChangesAsync();

            var service = new DoctorService(context);

            var result = await service.GetByUserIdAsync(
                "doctor-user-6",
                CancellationToken.None);

            result.Should().NotBeNull();
            result!.DoctorId.Should().Be(6);
            result.FullName.Should().Be("Doctor 6");
        }

        [Fact]
        public async Task GetByUserIdAsync_WhenDoctorDoesNotExist_ThrowsNotFoundException()
        {
            await using var context = CreateDbContext();

            var service = new DoctorService(context);

            Func<Task> action = async () =>
                await service.GetByUserIdAsync(
                    "missing-user",
                    CancellationToken.None);

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Doctor profile not found.");
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDoctorActive_ReturnsAvailableSlots()
        {
            await using var context = CreateDbContext();

            context.Doctors.Add(
                CreateDoctor(
                    6,
                    "doctor-user-6",
                    true));

            await context.SaveChangesAsync();

            var service = new DoctorService(context);

            var selectedDate = DateTime.Today.AddDays(1);

            var result = await service.GetAvailabilityAsync(
                6,
                selectedDate,
                CancellationToken.None);

            result.Should().NotBeNull();
            result.DoctorId.Should().Be(6);
            result.IsActive.Should().BeTrue();
            result.Date.Should().Be(selectedDate.Date);
            result.AvailableSlots.Should().HaveCount(12);
            result.AvailableSlots.Should().Contain("09:00");
            result.AvailableSlots.Should().Contain("16:30");
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenSlotBooked_RemovesBookedSlot()
        {
            await using var context = CreateDbContext();

            var selectedDate = DateTime.Today.AddDays(1);

            var doctor = CreateDoctor(
                6,
                "doctor-user-6",
                true);

            context.Doctors.Add(doctor);

            context.Appointments.Add(
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 3,
                    DoctorId = 6,
                    ScheduledDate = selectedDate,
                    TimeSlot = "09:00",
                    Status = AppointmentStatus.Confirmed
                });

            await context.SaveChangesAsync();

            var service = new DoctorService(context);

            var result = await service.GetAvailabilityAsync(
                6,
                selectedDate,
                CancellationToken.None);

            result.AvailableSlots.Should().NotContain("09:00");
            result.AvailableSlots.Should().HaveCount(11);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenAppointmentCancelled_KeepsSlotAvailable()
        {
            await using var context = CreateDbContext();

            var selectedDate = DateTime.Today.AddDays(1);

            var doctor = CreateDoctor(
                6,
                "doctor-user-6",
                true);

            context.Doctors.Add(doctor);

            context.Appointments.Add(
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 3,
                    DoctorId = 6,
                    ScheduledDate = selectedDate,
                    TimeSlot = "09:00",
                    Status = AppointmentStatus.Cancelled
                });

            await context.SaveChangesAsync();

            var service = new DoctorService(context);

            var result = await service.GetAvailabilityAsync(
                6,
                selectedDate,
                CancellationToken.None);

            result.AvailableSlots.Should().Contain("09:00");
            result.AvailableSlots.Should().HaveCount(12);
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDoctorInactive_ReturnsEmptySlots()
        {
            await using var context = CreateDbContext();

            context.Doctors.Add(
                CreateDoctor(
                    6,
                    "doctor-user-6",
                    false));

            await context.SaveChangesAsync();

            var service = new DoctorService(context);

            var result = await service.GetAvailabilityAsync(
                6,
                DateTime.Today.AddDays(1),
                CancellationToken.None);

            result.IsActive.Should().BeFalse();
            result.AvailableSlots.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAvailabilityAsync_WhenDoctorDoesNotExist_ThrowsNotFoundException()
        {
            await using var context = CreateDbContext();

            var service = new DoctorService(context);

            Func<Task> action = async () =>
                await service.GetAvailabilityAsync(
                    99,
                    DateTime.Today.AddDays(1),
                    CancellationToken.None);

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Doctor not found");
        }

        [Fact]
        public async Task UpdateAsync_WhenDoctorExists_UpdatesDoctor()
        {
            await using var context = CreateDbContext();

            context.Doctors.Add(
                CreateDoctor(
                    6,
                    "doctor-user-6",
                    true));

            await context.SaveChangesAsync();

            var service = new DoctorService(context);

            var updateDto = new UpdateDoctorDto
            {
                FullName = "Doctor Updated",
                Specialisation = Specialisation.Neurology,
                YearsOfExperience = 10,
                ConsultationFee = 1200,
                IsActive = false
            };

            var result = await service.UpdateAsync(
                6,
                updateDto,
                CancellationToken.None);

            result.Should().NotBeNull();
            result.DoctorId.Should().Be(6);
            result.FullName.Should().Be("Doctor Updated");
            result.Specialisation.Should().Be(
                Specialisation.Neurology);
            result.YearsOfExperience.Should().Be(10);
            result.ConsultationFee.Should().Be(1200);
            result.IsActive.Should().BeFalse();

            var savedDoctor =
                await context.Doctors.FindAsync(6);

            savedDoctor.Should().NotBeNull();
            savedDoctor!.FullName.Should().Be(
                "Doctor Updated");
        }

        [Fact]
        public async Task UpdateAsync_WhenDoctorDoesNotExist_ThrowsNotFoundException()
        {
            await using var context = CreateDbContext();

            var service = new DoctorService(context);

            var updateDto = new UpdateDoctorDto
            {
                FullName = "Missing Doctor",
                Specialisation = Specialisation.Cardiology,
                YearsOfExperience = 5,
                ConsultationFee = 800,
                IsActive = true
            };

            Func<Task> action = async () =>
                await service.UpdateAsync(
                    99,
                    updateDto,
                    CancellationToken.None);

            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage("Doctor not found");
        }
    }
}