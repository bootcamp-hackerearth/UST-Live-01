using HealthApp.Api.Models;
using HealthApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Data
{
    public static class DataSeeder
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { DoctorId = 1, FullName = "Dr. Gregory House", Specialisation = SpecialisationType.GeneralPhysician, DoctorPhoneNo = "9000000101", DoctorEmail = "gregory.house@healthapp.com", YearsOfExperience = 15, ConsultationFee = 600.00m, IsActive = true },
                new Doctor { DoctorId = 2, FullName = "Dr. Lisa Cuddy", Specialisation = SpecialisationType.Gynecologist, DoctorPhoneNo = "9000000102", DoctorEmail = "lisa.cuddy@healthapp.com", YearsOfExperience = 12, ConsultationFee = 500.00m, IsActive = true },
                new Doctor { DoctorId = 3, FullName = "Dr. Derek Shepherd", Specialisation = SpecialisationType.Neurologist, DoctorPhoneNo = "9000000103", DoctorEmail = "derek.shepherd@healthapp.com", YearsOfExperience = 18, ConsultationFee = 1000.00m, IsActive = true },
                new Doctor { DoctorId = 4, FullName = "Dr. Meredith Grey", Specialisation = SpecialisationType.GeneralPhysician, DoctorPhoneNo = "9000000104", DoctorEmail = "meredith.grey@healthapp.com", YearsOfExperience = 8, ConsultationFee = 450.00m, IsActive = true },
                new Doctor { DoctorId = 5, FullName = "Dr. Cristina Yang", Specialisation = SpecialisationType.Cardiologist, DoctorPhoneNo = "9000000105", DoctorEmail = "cristina.yang@healthapp.com", YearsOfExperience = 10, ConsultationFee = 750.00m, IsActive = true },
                new Doctor { DoctorId = 6, FullName = "Dr. Richard Webber", Specialisation = SpecialisationType.GeneralPhysician, DoctorPhoneNo = "9000000106", DoctorEmail = "richard.webber@healthapp.com", YearsOfExperience = 30, ConsultationFee = 800.00m, IsActive = true },
                new Doctor { DoctorId = 7, FullName = "Dr. Miranda Bailey", Specialisation = SpecialisationType.GeneralPhysician, DoctorPhoneNo = "9000000107", DoctorEmail = "miranda.bailey@healthapp.com", YearsOfExperience = 14, ConsultationFee = 550.00m, IsActive = true },
                new Doctor { DoctorId = 8, FullName = "Dr. Alex Karev", Specialisation = SpecialisationType.Pediatrician, DoctorPhoneNo = "9000000108", DoctorEmail = "alex.karev@healthapp.com", YearsOfExperience = 7, ConsultationFee = 400.00m, IsActive = true },
                new Doctor { DoctorId = 9, FullName = "Dr. Jackson Avery", Specialisation = SpecialisationType.ENT, DoctorPhoneNo = "9000000109", DoctorEmail = "jackson.avery@healthapp.com", YearsOfExperience = 9, ConsultationFee = 650.00m, IsActive = true },
                new Doctor { DoctorId = 10, FullName = "Dr. April Kepner", Specialisation = SpecialisationType.Orthopedic, DoctorPhoneNo = "9000000110", DoctorEmail = "april.kepner@healthapp.com", YearsOfExperience = 6, ConsultationFee = 400.00m, IsActive = true }
            );

            modelBuilder.Entity<Patient>().HasData(
                new Patient { PatientId = 1, FullName = "Michael Johnson", DateOfBirth = new DateOnly(1985, 4, 12), Gender = "Male", PhoneNumber = "8000000501", Email = "michael.j@gmail.com", InsuranceId = "INS-1001", CreatedDate = new DateTime(2025, 1, 10) },
                new Patient { PatientId = 2, FullName = "Sarah Williams", DateOfBirth = new DateOnly(1990, 8, 25), Gender = "Female", PhoneNumber = "8000000502", Email = "sarah.w@gmail.com", InsuranceId = "INS-1002", CreatedDate = new DateTime(2025, 1, 11) },
                new Patient { PatientId = 3, FullName = "David Brown", DateOfBirth = new DateOnly(1978, 2, 14), Gender = "Male", PhoneNumber = "8000000503", Email = "david.b@gmail.com", InsuranceId = "INS-1003", CreatedDate = new DateTime(2025, 1, 12) },
                new Patient { PatientId = 4, FullName = "Emily Davis", DateOfBirth = new DateOnly(1995, 11, 30), Gender = "Female", PhoneNumber = "8000000504", Email = "emily.d@gmail.com", InsuranceId = "INS-1004", CreatedDate = new DateTime(2025, 1, 13) },
                new Patient { PatientId = 5, FullName = "James Miller", DateOfBirth = new DateOnly(1982, 7, 19), Gender = "Male", PhoneNumber = "8000000505", Email = "james.m@gmail.com", InsuranceId = "INS-1005", CreatedDate = new DateTime(2025, 1, 14) },
                new Patient { PatientId = 6, FullName = "Jessica Wilson", DateOfBirth = new DateOnly(1988, 9, 5), Gender = "Female", PhoneNumber = "8000000506", Email = "jessica.w@gmail.com", InsuranceId = "INS-1006", CreatedDate = new DateTime(2025, 1, 15) },
                new Patient { PatientId = 7, FullName = "Robert Moore", DateOfBirth = new DateOnly(1970, 12, 1), Gender = "Male", PhoneNumber = "8000000507", Email = "robert.m@gmail.com", InsuranceId = "INS-1007", CreatedDate = new DateTime(2025, 1, 16) },
                new Patient { PatientId = 8, FullName = "Amanda Taylor", DateOfBirth = new DateOnly(1992, 3, 22), Gender = "Female", PhoneNumber = "8000000508", Email = "amanda.t@gmail.com", InsuranceId = "INS-1008", CreatedDate = new DateTime(2025, 1, 17) },
                new Patient { PatientId = 9, FullName = "William Anderson", DateOfBirth = new DateOnly(1980, 5, 17), Gender = "Male", PhoneNumber = "8000000509", Email = "william.a@gmail.com", InsuranceId = "INS-1009", CreatedDate = new DateTime(2025, 1, 18) },
                new Patient { PatientId = 10, FullName = "Ashley Thomas", DateOfBirth = new DateOnly(1998, 10, 8), Gender = "Female", PhoneNumber = "8000000510", Email = "ashley.t@gmail.com", InsuranceId = "INS-1010", CreatedDate = new DateTime(2025, 1, 19) }
            );

            modelBuilder.Entity<Appointment>().HasData(
                new Appointment { AppointmentId = 1001, PatientId = 1, DoctorId = 1, ScheduledDate = new DateOnly(2025, 10, 15), TimeSlot = "09:00 AM - 09:30 AM", Status = AppointmentStatus.Completed },
                new Appointment { AppointmentId = 1002, PatientId = 2, DoctorId = 2, ScheduledDate = new DateOnly(2025, 11, 20), TimeSlot = "10:00 AM - 10:30 AM", Status = AppointmentStatus.Completed },
                new Appointment { AppointmentId = 1003, PatientId = 3, DoctorId = 3, ScheduledDate = new DateOnly(2026, 1, 10), TimeSlot = "11:30 AM - 12:00 PM", Status = AppointmentStatus.Completed },
                new Appointment { AppointmentId = 1004, PatientId = 4, DoctorId = 4, ScheduledDate = new DateOnly(2026, 2, 14), TimeSlot = "02:00 PM - 02:30 PM", Status = AppointmentStatus.Completed },
                new Appointment { AppointmentId = 1005, PatientId = 5, DoctorId = 5, ScheduledDate = new DateOnly(2026, 4, 10), TimeSlot = "04:00 PM - 04:30 PM", Status = AppointmentStatus.Cancelled, CancellationReason = "Patient had a sudden family emergency." },
                new Appointment { AppointmentId = 1006, PatientId = 6, DoctorId = 1, ScheduledDate = new DateOnly(2026, 8, 15), TimeSlot = "09:00 AM - 09:30 AM", Status = AppointmentStatus.Confirmed },
                new Appointment { AppointmentId = 1007, PatientId = 7, DoctorId = 2, ScheduledDate = new DateOnly(2026, 9, 16), TimeSlot = "10:00 AM - 10:30 AM", Status = AppointmentStatus.Confirmed },
                new Appointment { AppointmentId = 1008, PatientId = 8, DoctorId = 3, ScheduledDate = new DateOnly(2026, 10, 17), TimeSlot = "11:30 AM - 12:00 PM", Status = AppointmentStatus.Confirmed },
                new Appointment { AppointmentId = 1009, PatientId = 9, DoctorId = 4, ScheduledDate = new DateOnly(2026, 11, 20), TimeSlot = "02:00 PM - 02:30 PM", Status = AppointmentStatus.Pending },
                new Appointment { AppointmentId = 1010, PatientId = 10, DoctorId = 5, ScheduledDate = new DateOnly(2026, 12, 5), TimeSlot = "04:00 PM - 04:30 PM", Status = AppointmentStatus.Pending }
            );

            modelBuilder.Entity<HealthRecord>().HasData(
                new HealthRecord { RecordId = 1001, PatientId = 1, DoctorId = 1, AppointmentId = 1001, VisitDate = new DateOnly(2025, 10, 15), Diagnosis = "Essential Hypertension", Prescription = "Lisinopril 10mg daily", Notes = "Patient educated on low sodium diet." },
                new HealthRecord { RecordId = 1002, PatientId = 2, DoctorId = 2, AppointmentId = 1002, VisitDate = new DateOnly(2025, 11, 20), Diagnosis = "Routine Pregnancy Checkup", Prescription = "Prenatal Vitamins", Notes = "Fetal heart rate normal. Continue regular checkups." },
                new HealthRecord { RecordId = 1003, PatientId = 3, DoctorId = 3, AppointmentId = 1003, VisitDate = new DateOnly(2026, 1, 10), Diagnosis = "Migraine with Aura", Prescription = "Sumatriptan 50mg as needed", Notes = "Advised to keep a headache diary to identify triggers." },
                new HealthRecord { RecordId = 1004, PatientId = 4, DoctorId = 4, AppointmentId = 1004, VisitDate = new DateOnly(2026, 2, 14), Diagnosis = "Acute Bronchitis", Prescription = "Azithromycin 500mg daily", Notes = "Rest and hydration recommended." }
            );
        }
    }
}