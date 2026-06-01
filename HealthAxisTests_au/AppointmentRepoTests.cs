using HAP_Pod4_ConsoleApp_au.Data;
using HAP_Pod4_ConsoleApp_au.Models;
using HAP_Pod4_ConsoleApp_au.Repositories;
using Xunit;

namespace HealthAxisTests.RepositoryTests
{
    public class AppointmentRepositoryTests
    {
        private readonly AppDbContext _dbcontext;
        private readonly AppointmentRepository _repo;

        public AppointmentRepositoryTests()
        {
            _dbcontext = new AppDbContext();
            _repo = new AppointmentRepository();
        }

        [Fact]
        public void BookAppointment_ShouldAddAppointment()
        {
            Appointment appointment = new Appointment
            {
                AppointmentId = _dbcontext.GetNextAppointmentId(),

                Patient = _dbcontext.Patients[0],

                Doctor = _dbcontext.Doctors[0],

                ScheduledDate = DateTime.Now.AddDays(1),

                TimeSlot = Appointment.TimeSlotOption.TenAMToTwelvePM
            };

            var result =
                _repo.BookAppointment(appointment);

            Assert.NotNull(result);

            Assert.Single(
                _repo.GetAllAppointments());
        }

        [Fact]
        public void GetAppointmentById_ShouldReturnAppointment()
        {
            Appointment appointment = new Appointment
            {
                AppointmentId = 1,
                Patient = _dbcontext.Patients[0],
                Doctor = _dbcontext.Doctors[0],
                ScheduledDate = DateTime.Now.AddDays(1),
                TimeSlot = Appointment.TimeSlotOption.TwoPMToFourPM
            };

            _repo.BookAppointment(appointment);

            var result = _repo.GetAppointmentById(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.AppointmentId);
        }

        [Fact]
        public void CancelAppointment_ShouldUpdateStatus()
        {
            Appointment appointment = new Appointment
            {
                AppointmentId = 1,
                Patient = _dbcontext.Patients[0],
                Doctor = _dbcontext.Doctors[0],
                ScheduledDate = DateTime.Now.AddDays(1),
                TimeSlot =
                    Appointment.TimeSlotOption.FourPMToSixPM
            };

            _repo.BookAppointment(appointment);

            bool result = _repo.CancelAppointment(1, "Emergency");

            Assert.True(result);

            var updated = _repo.GetAppointmentById(1);

            Assert.NotNull(updated);

            Assert.Equal(Appointment.StatusOption.Cancelled, updated.Status);
        }
    }
}