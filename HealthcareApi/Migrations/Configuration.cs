namespace HealthcareMvcApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using HealthcareApi.Data;
    using SharedClasses.Enums;
    using HealthcareApi.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<HealthcareDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HealthcareDbContext context)
        {
            DateTime today = DateTime.Today;

            SeedPatients(context, today);
            SeedDoctors(context, today);

            context.SaveChanges();

            SeedAppointments(context, today);

            context.SaveChanges();

            SeedHealthRecords(context);

            context.SaveChanges();
        }

        private void SeedPatients(HealthcareDbContext context, DateTime today)
        {
            context.Patients.AddOrUpdate(
                p => p.Email,

                new Patient
                {
                    FullName = "Arun Kumar",
                    DateOfBirth = today.AddYears(-34).AddMonths(-2),
                    Gender = Gender.Male,
                    PhoneNumber = "9876543210",
                    Email = "arun.kumar@example.com",
                    InsuranceId = "INS1001",
                    CreatedDate = today.AddDays(-30)
                },

                new Patient
                {
                    FullName = "Meera Nair",
                    DateOfBirth = today.AddYears(-38).AddMonths(-1),
                    Gender = Gender.Female,
                    PhoneNumber = "9876543211",
                    Email = "meera.nair@example.com",
                    InsuranceId = "INS1002",
                    CreatedDate = today.AddDays(-25)
                },

                new Patient
                {
                    FullName = "Rohan Mathew",
                    DateOfBirth = today.AddYears(-25).AddMonths(-4),
                    Gender = Gender.Male,
                    PhoneNumber = "9876543212",
                    Email = "rohan.mathew@example.com",
                    InsuranceId = "INS1003",
                    CreatedDate = today.AddDays(-20)
                },

                new Patient
                {
                    FullName = "Anjali Menon",
                    DateOfBirth = today.AddYears(-31).AddMonths(-3),
                    Gender = Gender.Female,
                    PhoneNumber = "9876543213",
                    Email = "anjali.menon@example.com",
                    InsuranceId = "INS1004",
                    CreatedDate = today.AddDays(-15)
                },

                new Patient
                {
                    FullName = "Kiran Joseph",
                    DateOfBirth = today.AddYears(-46).AddMonths(-5),
                    Gender = Gender.Male,
                    PhoneNumber = "9876543214",
                    Email = "kiran.joseph@example.com",
                    InsuranceId = "INS1005",
                    CreatedDate = today.AddDays(-10)
                },

                new Patient
                {
                    FullName = "Sara Thomas",
                    DateOfBirth = today.AddYears(-12).AddMonths(-1),
                    Gender = Gender.Female,
                    PhoneNumber = "9876543215",
                    Email = "sara.thomas@example.com",
                    InsuranceId = "INS1006",
                    CreatedDate = today.AddDays(-5)
                }
            );
        }

        private void SeedDoctors(HealthcareDbContext context, DateTime today)
        {
            context.Doctors.AddOrUpdate(
                d => d.FullName,

                new Doctor
                {
                    FullName = "Anitha Varghese",
                    Specialisation = Specialisation.GeneralMedicine,
                    PracticeStartDate = today.AddYears(-12),
                    ConsultationFee = 500,
                    IsActive = true
                },

                new Doctor
                {
                    FullName = "Rahul Menon",
                    Specialisation = Specialisation.Cardiology,
                    PracticeStartDate = today.AddYears(-15),
                    ConsultationFee = 900,
                    IsActive = true
                },

                new Doctor
                {
                    FullName = "Priya Nair",
                    Specialisation = Specialisation.Dermatology,
                    PracticeStartDate = today.AddYears(-8),
                    ConsultationFee = 650,
                    IsActive = true
                },

                new Doctor
                {
                    FullName = "Vikram Iyer",
                    Specialisation = Specialisation.Neurology,
                    PracticeStartDate = today.AddYears(-20),
                    ConsultationFee = 1200,
                    IsActive = true
                },

                new Doctor
                {
                    FullName = "Lakshmi Pillai",
                    Specialisation = Specialisation.Paediatrics,
                    PracticeStartDate = today.AddYears(-10),
                    ConsultationFee = 550,
                    IsActive = true
                },

                new Doctor
                {
                    FullName = "Sameer Khan",
                    Specialisation = Specialisation.ENT,
                    PracticeStartDate = today.AddYears(-7),
                    ConsultationFee = 600,
                    IsActive = true
                },

                new Doctor
                {
                    FullName = "Divya Krishnan",
                    Specialisation = Specialisation.Orthopaedics,
                    PracticeStartDate = today.AddYears(-14),
                    ConsultationFee = 850,
                    IsActive = false
                },

                new Doctor
                {
                    FullName = "Naveen Raj",
                    Specialisation = Specialisation.Ophthalmology,
                    PracticeStartDate = today.AddYears(-9),
                    ConsultationFee = 700,
                    IsActive = true
                }
            );
        }

        private void SeedAppointments(HealthcareDbContext context, DateTime today)
        {
            Patient arun = context.Patients.First(p => p.Email == "arun.kumar@example.com");
            Patient meera = context.Patients.First(p => p.Email == "meera.nair@example.com");
            Patient rohan = context.Patients.First(p => p.Email == "rohan.mathew@example.com");
            Patient anjali = context.Patients.First(p => p.Email == "anjali.menon@example.com");
            Patient kiran = context.Patients.First(p => p.Email == "kiran.joseph@example.com");
            Patient sara = context.Patients.First(p => p.Email == "sara.thomas@example.com");

            Doctor anitha = context.Doctors.First(d => d.FullName == "Anitha Varghese");
            Doctor rahul = context.Doctors.First(d => d.FullName == "Rahul Menon");
            Doctor priya = context.Doctors.First(d => d.FullName == "Priya Nair");
            Doctor vikram = context.Doctors.First(d => d.FullName == "Vikram Iyer");
            Doctor lakshmi = context.Doctors.First(d => d.FullName == "Lakshmi Pillai");
            Doctor sameer = context.Doctors.First(d => d.FullName == "Sameer Khan");

            context.Appointments.AddOrUpdate(
                a => new
                {
                    a.PatientId,
                    a.DoctorId,
                    a.SlotNumber,
                    a.Status
                },

                new Appointment
                {
                    PatientId = arun.PatientId,
                    DoctorId = anitha.DoctorId,
                    ScheduledDate = today.AddDays(1),
                    SlotNumber = 1,
                    Status = AppointmentStatus.Pending,
                    CancellationReason = string.Empty
                },

                new Appointment
                {
                    PatientId = meera.PatientId,
                    DoctorId = rahul.DoctorId,
                    ScheduledDate = today.AddDays(1),
                    SlotNumber = 1,
                    Status = AppointmentStatus.Pending,
                    CancellationReason = string.Empty
                },

                new Appointment
                {
                    PatientId = rohan.PatientId,
                    DoctorId = anitha.DoctorId,
                    ScheduledDate = today,
                    SlotNumber = 2,
                    Status = AppointmentStatus.Confirmed,
                    CancellationReason = string.Empty
                },

                new Appointment
                {
                    PatientId = anjali.PatientId,
                    DoctorId = rahul.DoctorId,
                    ScheduledDate = today,
                    SlotNumber = 2,
                    Status = AppointmentStatus.Confirmed,
                    CancellationReason = string.Empty
                },

                new Appointment
                {
                    PatientId = kiran.PatientId,
                    DoctorId = priya.DoctorId,
                    ScheduledDate = today.AddDays(2),
                    SlotNumber = 1,
                    Status = AppointmentStatus.Confirmed,
                    CancellationReason = string.Empty
                },

                new Appointment
                {
                    PatientId = sara.PatientId,
                    DoctorId = lakshmi.DoctorId,
                    ScheduledDate = today.AddDays(3),
                    SlotNumber = 1,
                    Status = AppointmentStatus.Confirmed,
                    CancellationReason = string.Empty
                },

                new Appointment
                {
                    PatientId = arun.PatientId,
                    DoctorId = rahul.DoctorId,
                    ScheduledDate = today.AddDays(-5),
                    SlotNumber = 3,
                    Status = AppointmentStatus.Completed,
                    CancellationReason = string.Empty
                },

                new Appointment
                {
                    PatientId = meera.PatientId,
                    DoctorId = priya.DoctorId,
                    ScheduledDate = today.AddDays(-3),
                    SlotNumber = 3,
                    Status = AppointmentStatus.Completed,
                    CancellationReason = string.Empty
                },

                new Appointment
                {
                    PatientId = kiran.PatientId,
                    DoctorId = sameer.DoctorId,
                    ScheduledDate = today.AddDays(-2),
                    SlotNumber = 4,
                    Status = AppointmentStatus.Completed,
                    CancellationReason = string.Empty
                },

                new Appointment
                {
                    PatientId = anjali.PatientId,
                    DoctorId = vikram.DoctorId,
                    ScheduledDate = today.AddDays(4),
                    SlotNumber = 1,
                    Status = AppointmentStatus.Cancelled,
                    CancellationReason = "Patient requested rescheduling. - Cancelled by Patient"
                },

                new Appointment
                {
                    PatientId = meera.PatientId,
                    DoctorId = sameer.DoctorId,
                    ScheduledDate = today.AddDays(1),
                    SlotNumber = 2,
                    Status = AppointmentStatus.Confirmed,
                    CancellationReason = string.Empty
                }
            );
        }

        private void SeedHealthRecords(HealthcareDbContext context)
        {
            Appointment appointmentSeven = FindAppointment(
                context,
                "arun.kumar@example.com",
                "Rahul Menon",
                AppointmentStatus.Completed,
                3);

            Appointment appointmentEight = FindAppointment(
                context,
                "meera.nair@example.com",
                "Priya Nair",
                AppointmentStatus.Completed,
                3);

            Appointment appointmentNine = FindAppointment(
                context,
                "kiran.joseph@example.com",
                "Sameer Khan",
                AppointmentStatus.Completed,
                4);

            context.HealthRecords.AddOrUpdate(
                h => h.AppointmentId,

                new HealthRecord
                {
                    PatientId = appointmentSeven.PatientId,
                    DoctorId = appointmentSeven.DoctorId,
                    AppointmentId = appointmentSeven.AppointmentId,
                    VisitDate = appointmentSeven.ScheduledDate,
                    Diagnosis = "Mild hypertension",
                    Prescription = "Amlodipine 5mg once daily",
                    Notes = "Advised low-salt diet and follow-up after one month."
                },

                new HealthRecord
                {
                    PatientId = appointmentEight.PatientId,
                    DoctorId = appointmentEight.DoctorId,
                    AppointmentId = appointmentEight.AppointmentId,
                    VisitDate = appointmentEight.ScheduledDate,
                    Diagnosis = "Skin allergy",
                    Prescription = "Cetirizine 10mg once daily for five days",
                    Notes = "Avoid suspected allergen and monitor symptoms."
                },

                new HealthRecord
                {
                    PatientId = appointmentNine.PatientId,
                    DoctorId = appointmentNine.DoctorId,
                    AppointmentId = appointmentNine.AppointmentId,
                    VisitDate = appointmentNine.ScheduledDate,
                    Diagnosis = "Ear infection",
                    Prescription = "Antibiotic ear drops twice daily for seven days",
                    Notes = "Keep ear dry and return if pain increases."
                }
            );
        }

        private Appointment FindAppointment(
            HealthcareDbContext context,
            string patientEmail,
            string doctorName,
            AppointmentStatus status,
            int slotNumber)
        {
            Patient patient = context.Patients.First(p => p.Email == patientEmail);
            Doctor doctor = context.Doctors.First(d => d.FullName == doctorName);

            return context.Appointments.First(a =>
                a.PatientId == patient.PatientId &&
                a.DoctorId == doctor.DoctorId &&
                a.Status == status &&
                a.SlotNumber == slotNumber);
        }
    }
}