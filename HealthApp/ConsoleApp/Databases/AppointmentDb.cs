using HealthApp.ConsoleApp.Models;
namespace HealthApp.ConsoleApp.Databases
{
    public class AppointmentDb
    {
        public List<Appointment> Appointments { get; set; } = new List<Appointment>
        {
            new Appointment
            {
                AppointmentId = 1,
                Patient = new Patient
                {
                    PatientId = 101,
                    FullName = "John Doe",
                    Email = "john.doe@email.com",
                    PhoneNumber = "9876543210",
                    InsuranceId = "sdfasd234324"
                },
                Doctor = new Doctor
                {
                    DoctorId = 1,
                    FullName = "Dr. Smith",
                    Specialisation = "Cardiology"
                },
                ScheduledDate = DateTime.Now.AddDays(1),
                TimeSlot = "10:00 AM",
                Status = AppointmentStatus.Confirmed
            },

            new Appointment
            {
                AppointmentId = 2,
                Patient = new Patient
                {
                    PatientId = 2,
                    FullName = "Jane Doe",
                    Email = "jane.doe@email.com",
                    PhoneNumber = "9123456780",
                    InsuranceId = "sdf234"
                },
                Doctor = new Doctor
                {
                    DoctorId = 2,
                    FullName = "Dr. Brown",
                    Specialisation = "Dermatology"
                },
                ScheduledDate = DateTime.Now.AddDays(2),
                TimeSlot = "11:00 AM",
                Status = AppointmentStatus.Pending
            },

            new Appointment
            {
                AppointmentId = 3,
                Patient = new Patient
                {
                    PatientId = 3,
                    FullName = "Alice Smith",
                    Email = "alice.smith@email.com",
                    PhoneNumber = "9988776655",
                    InsuranceId = "sdf234"
                },
                Doctor = new Doctor
                {
                    DoctorId = 1,
                    FullName = "Dr. Smith",
                    Specialisation = "Cardiology"
                },
                ScheduledDate = DateTime.Now.AddDays(3),
                TimeSlot = "02:00 PM",
                Status = AppointmentStatus.Cancelled,
                CancellationReason = "Patient requested cancellation"
            },

            new Appointment
            {
                AppointmentId = 4,
                Patient = new Patient
                {
                    PatientId = 4,
                    FullName = "Bob Johnson",
                    Email = "bob.johnson@email.com",
                    PhoneNumber = "9012345678",
                    InsuranceId = "sdf234"
                },
                Doctor = new Doctor
                {
                    DoctorId = 3,
                    FullName = "Dr. Green",
                    Specialisation = "Orthopedics"
                },
                ScheduledDate = DateTime.Now.AddDays(4),
                TimeSlot = "03:00 PM",
                Status = AppointmentStatus.Completed
            }
        };
    }
}