using System.Data.Entity.Migrations;
using System.Linq;
using Healthaxis2.Data;
using Healthaxis2.Models;

namespace Healthaxis2.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(AppDbContext context)
        {
            if (!context.Patients.Any())
            {
                context.Patients.AddOrUpdate(p => p.PatientId,

                    new Patient
                    {
                        PatientName = "John Mathew",
                        DateOfBirth = new System.DateTime(1995, 2, 15),
                        Gender = "Male",
                        PhoneNumber = "9876543210",
                        Email = "john@gmail.com",
                        InsuranceId = "INS0001",
                        RegisteredDate = System.DateTime.Today
                    },

                    new Patient
                    {
                        PatientName = "Anita Nair",
                        DateOfBirth = new System.DateTime(1998, 6, 22),
                        Gender = "Female",
                        PhoneNumber = "9876543211",
                        Email = "anita@gmail.com",
                        InsuranceId = "INS0002",
                        RegisteredDate = System.DateTime.Today
                    }
                );
            }
            if (!context.Doctors.Any())
            {
                context.Doctors.AddOrUpdate(d => d.DoctorId,

                    new Doctor
                    {
                        DoctorName = "Dr. Arun Kumar",
                        Specialisation = "Cardiologist",
                        Experience = 10,
                        Fees = 500,
                        IsActive = true
                    },

                    new Doctor
                    {
                        DoctorName = "Dr. Meera S",
                        Specialisation = "Dermatologist",
                        Experience = 8,
                        Fees = 400,
                        IsActive = true
                    }
                );
            }

            context.SaveChanges();

            if (!context.Appointments.Any())
            {
                context.Appointments.AddOrUpdate(a => a.AppointmentId,

                    new Appointment
                    {
                        PatientId = context.Patients.First().PatientId,
                        DoctorId = context.Doctors.First().DoctorId,
                        ScheduledDate = System.DateTime.Today.AddDays(1),
                        Slot = "09:00 AM",
                        Status = "Pending"
                    }
                );
            }

            context.SaveChanges();
            var completedAppt = context.Appointments.FirstOrDefault(a => a.Status == "Completed");

            if (completedAppt != null && !context.HealthRecords.Any())
            {
                context.HealthRecords.AddOrUpdate(h => h.RecordId,

                    new HealthRecord
                    {
                        AppointmentId = completedAppt.AppointmentId,
                        PatientId = completedAppt.PatientId,
                        DoctorId = completedAppt.DoctorId,
                        VisitDate = System.DateTime.Today,
                        Diagnosis = "Fever",
                        Prescription = "Paracetamol",
                        Notes = "Rest required"
                    }
                );
            }

            context.SaveChanges();
        }
    }
}