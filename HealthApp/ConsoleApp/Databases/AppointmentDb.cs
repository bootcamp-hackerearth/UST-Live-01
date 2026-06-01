using HealthApp.ConsoleApp.Models;
namespace HealthApp.ConsoleApp.Databases
{
    public class AppointmentDb
    {
        PatientDb patientDb = new();
        DoctorDb doctorDb = new();

        public List<Appointment> Appointments; //AppointmentId starts from 301 to avoid conflict with test data

        public AppointmentDb()
        {
            Appointments = new List<Appointment>
            {
                // CONFIRMED
                new Appointment
                {
                    AppointmentId = 301,

                    Patient = patientDb.Patients
                        .First(p => p.PatientId == 101),

                    Doctor = doctorDb.Doctors
                        .First(d => d.DoctorId == 201),

                    ScheduledDate = DateTime.Today.AddDays(1),

                    TimeSlot = "10:00 AM",

                    Status = AppointmentStatus.Confirmed
                },
                // PENDING
                new Appointment
                {
                    AppointmentId = 302,

                    Patient = patientDb.Patients
                        .First(p => p.PatientId == 102),

                    Doctor = doctorDb.Doctors
                        .First(d => d.DoctorId == 202),

                    ScheduledDate = DateTime.Today.AddDays(2),

                    TimeSlot = "12:00 PM",

                    Status = AppointmentStatus.Pending
                },
                // COMPLETED
                new Appointment
                {
                    AppointmentId = 303,

                    Patient = patientDb.Patients
                        .First(p => p.PatientId == 103),

                    Doctor = doctorDb.Doctors
                        .First(d => d.DoctorId == 204),

                    ScheduledDate = DateTime.Today.AddDays(-2),

                    TimeSlot = "03:00 PM",

                    Status = AppointmentStatus.Completed
                },
                // CANCELLED
                new Appointment
                {
                    AppointmentId = 304,

                    Patient = patientDb.Patients
                        .First(p => p.PatientId == 104),

                    Doctor = doctorDb.Doctors
                        .First(d => d.DoctorId == 205),

                    ScheduledDate = DateTime.Today.AddDays(1),

                    TimeSlot = "11:00 AM",

                    Status = AppointmentStatus.Cancelled,

                    CancellationReason =
                        "Patient unavailable"
                },
                // SLOT CONFLICT TEST
                new Appointment
                {
                    AppointmentId = 305,

                    Patient = patientDb.Patients
                        .First(p => p.PatientId == 105),

                    Doctor = doctorDb.Doctors
                        .First(d => d.DoctorId == 201),

                    ScheduledDate = DateTime.Today.AddDays(1),

                    TimeSlot = "10:00 AM",

                    Status = AppointmentStatus.Confirmed
                },
                // SAME DOCTOR DIFFERENT SLOT
                new Appointment
                {
                    AppointmentId = 306,

                    Patient = patientDb.Patients
                        .First(p => p.PatientId == 102),

                    Doctor = doctorDb.Doctors
                        .First(d => d.DoctorId == 201),

                    ScheduledDate = DateTime.Today.AddDays(1),

                    TimeSlot = "11:00 AM",

                    Status = AppointmentStatus.Confirmed
                }
            };
        }
    }
}