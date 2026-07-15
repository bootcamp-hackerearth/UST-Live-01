using FluentAssertions;
using HealthAxis.API.Events;
using Xunit;

namespace Healthcare.netcore.Tests
{
    public class AppointmentBookedEventTests
    {
        [Fact]
        public void Should_Set_Properties()
        {
            var evt = new AppointmentBookedEvent
            {
                AppointmentId = 1,
                DoctorId = 2,
                PatientId = 3,
                PatientName = "Kiran",
                TimeSlot = "10:00 AM"
            };

            evt.AppointmentId.Should().Be(1);
            evt.DoctorId.Should().Be(2);
            evt.PatientId.Should().Be(3);
            evt.PatientName.Should().Be("Kiran");
            evt.TimeSlot.Should().Be("10:00 AM");
        }
    }
}