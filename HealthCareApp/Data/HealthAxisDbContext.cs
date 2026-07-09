using HealthCareApp.Shared.Enums;
using HealthCareApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthCareApp.Data
{
    public class HealthAxisDbContext : IdentityDbContext<IdentityUser>
    {
        public HealthAxisDbContext(DbContextOptions<HealthAxisDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<HealthRecord> HealthRecords { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        private static DateTime UtcDate(int year, int month, int day)
        {
            return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Patient to IdentityUser relationship
            builder.Entity<Patient>()
                .HasOne(p => p.IdentityUser)
                .WithOne()
                .HasForeignKey<Patient>(p => p.IdentityUserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Appointment relationships
            builder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            // HealthRecord relationships
            builder.Entity<HealthRecord>()
                .HasOne(hr => hr.Patient)
                .WithMany(p => p.HealthRecords)
                .HasForeignKey(hr => hr.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<HealthRecord>()
                .HasOne(hr => hr.Doctor)
                .WithMany(d => d.HealthRecords)
                .HasForeignKey(hr => hr.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<HealthRecord>()
                .HasOne(hr => hr.Appointment)
                .WithOne(a => a.HealthRecord)
                .HasForeignKey<HealthRecord>(hr => hr.AppointmentId)
                .OnDelete(DeleteBehavior.NoAction);

            // Patient seed data
            builder.Entity<Patient>().HasData(
                new Patient
                {
                    PatientId = 1,
                    PatientName = "Ravi Kumar",
                    DateOfBirth = UtcDate(1998, 5, 12),
                    Gender = GenderType.Male,
                    Email = "ravi.kumar@example.com",
                    PhoneNumber = "9876543210",
                    InsuranceID = "INS1001",
                    IdentityUserId = null,
                    CreatedDate = UtcDate(2026, 6, 15)
                },
                new Patient
                {
                    PatientId = 2,
                    PatientName = "Anjali Nair",
                    DateOfBirth = UtcDate(2001, 8, 20),
                    Gender = GenderType.Female,
                    Email = "anjali.nair@example.com",
                    PhoneNumber = "8765432109",
                    InsuranceID = "INS1002",
                    IdentityUserId = null,
                    CreatedDate = UtcDate(2026, 6, 15)
                },
                new Patient
                {
                    PatientId = 3,
                    PatientName = "Kiran Das",
                    DateOfBirth = UtcDate(1995, 11, 3),
                    Gender = GenderType.Other,
                    Email = "kiran.das@example.com",
                    PhoneNumber = "7654321098",
                    InsuranceID = null,
                    IdentityUserId = null,
                    CreatedDate = UtcDate(2026, 6, 15)
                }
            );

            // Doctor seed data
            builder.Entity<Doctor>().HasData(
                new Doctor
                {
                    DoctorId = 1,
                    DoctorName = "Arun Menon",
                    Email = "arun.menon@example.com",
                    Specialisation = SpecialisationType.GeneralPractitioner,
                    YearsOfExperience = 10,
                    ConsultationFee = 5000,
                    IsActive = true,
                    MustChangePassword = false,
                    IdentityUserId = null,
                    CreatedDate = UtcDate(2026, 6, 15)
                },
                new Doctor
                {
                    DoctorId = 2,
                    DoctorName = "Meera Nair",
                    Email = "meera.nair@example.com",
                    Specialisation = SpecialisationType.Cardiologist,
                    YearsOfExperience = 15,
                    ConsultationFee = 1000,
                    IsActive = true,
                    MustChangePassword = false,
                    IdentityUserId = null,
                    CreatedDate = UtcDate(2026, 6, 15)
                },
                new Doctor
                {
                    DoctorId = 3,
                    DoctorName = "Vikram Das",
                    Email = "vikram.das@example.com",
                    Specialisation = SpecialisationType.Dermatologist,
                    YearsOfExperience = 8,
                    ConsultationFee = 700,
                    IsActive = true,
                    MustChangePassword = false,
                    IdentityUserId = null,
                    CreatedDate = UtcDate(2026, 6, 15)
                }
            );

            // Appointment seed data
            builder.Entity<Appointment>().HasData(
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate = UtcDate(2026, 6, 20),
                    TimeSlot = "10:00 AM - 10:30 AM",
                    Status = AppointmentStatus.Pending,
                    CancellationReason = null,
                    CreatedDate = UtcDate(2026, 6, 15)
                },
                new Appointment
                {
                    AppointmentId = 2,
                    PatientId = 2,
                    DoctorId = 2,
                    ScheduledDate = UtcDate(2026, 6, 21),
                    TimeSlot = "11:00 AM - 11:30 AM",
                    Status = AppointmentStatus.Confirmed,
                    CancellationReason = null,
                    CreatedDate = UtcDate(2026, 6, 15)
                },
                new Appointment
                {
                    AppointmentId = 3,
                    PatientId = 3,
                    DoctorId = 3,
                    ScheduledDate = UtcDate(2026, 6, 22),
                    TimeSlot = "02:00 PM - 02:30 PM",
                    Status = AppointmentStatus.Cancelled,
                    CancellationReason = "Patient requested cancellation",
                    CreatedDate = UtcDate(2026, 6, 15)
                }
            );

            // HealthRecord seed data
            builder.Entity<HealthRecord>().HasData(
                new HealthRecord
                {
                    HealthRecordId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    AppointmentId = 1,
                    VisitDate = UtcDate(2026, 6, 20),
                    Diagnosis = "Fever and cold",
                    Prescription = "Paracetamol 500mg twice daily",
                    Notes = "Drink enough water and take rest",
                    CreatedDate = UtcDate(2026, 6, 15)
                },
                new HealthRecord
                {
                    HealthRecordId = 2,
                    PatientId = 2,
                    DoctorId = 2,
                    AppointmentId = 2,
                    VisitDate = UtcDate(2026, 6, 21),
                    Diagnosis = "Chest discomfort",
                    Prescription = "ECG test and follow-up consultation",
                    Notes = "Avoid heavy exercise until review",
                    CreatedDate = UtcDate(2026, 6, 15)
                },
                new HealthRecord
                {
                    HealthRecordId = 3,
                    PatientId = 3,
                    DoctorId = null,
                    AppointmentId = 3,
                    VisitDate = UtcDate(2026, 6, 22),
                    Diagnosis = "Consultation cancelled",
                    Prescription = "No prescription issued",
                    Notes = "Appointment was cancelled by patient",
                    CreatedDate = UtcDate(2026, 6, 15)
                }
            );
        }
    }
}