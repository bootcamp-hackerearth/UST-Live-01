using HealthCareApp.Enums;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Appointment relationships
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            // HealthRecord relationships
            modelBuilder.Entity<HealthRecord>()
                .HasOne(hr => hr.Patient)
                .WithMany(p => p.HealthRecords)
                .HasForeignKey(hr => hr.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HealthRecord>()
                .HasOne(hr => hr.Doctor)
                .WithMany(d => d.HealthRecords)
                .HasForeignKey(hr => hr.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HealthRecord>()
                .HasOne(hr => hr.Appointment)
                .WithOne(a => a.HealthRecord)
                .HasForeignKey<HealthRecord>(hr => hr.AppointmentId)
                .OnDelete(DeleteBehavior.NoAction);

            // Patient seed data
            modelBuilder.Entity<Patient>().HasData(
                new Patient
                {
                    PatientId = 1,
                    PatientName = "Ravi Kumar",
                    DateOfBirth = new DateTime(1998, 5, 12),
                    Gender = GenderType.Male,
                    Email = "ravi.kumar@example.com",
                    PhoneNumber = "9876543210",
                    InsuranceID = "INS1001",
                    CreatedDate = new DateTime(2026, 6, 15)
                },
                new Patient
                {
                    PatientId = 2,
                    PatientName = "Anjali Nair",
                    DateOfBirth = new DateTime(2001, 8, 20),
                    Gender = GenderType.Female,
                    Email = "anjali.nair@example.com",
                    PhoneNumber = "8765432109",
                    InsuranceID = "INS1002",
                    CreatedDate = new DateTime(2026, 6, 15)
                },
                new Patient
                {
                    PatientId = 3,
                    PatientName = "Kiran Das",
                    DateOfBirth = new DateTime(1995, 11, 3),
                    Gender = GenderType.Other,
                    Email = "kiran.das@example.com",
                    PhoneNumber = "7654321098",
                    InsuranceID = null,
                    CreatedDate = new DateTime(2026, 6, 15)
                }
            );

            // Doctor seed data
            modelBuilder.Entity<Doctor>().HasData(
                new Doctor
                {
                    DoctorId = 1,
                    DoctorName = "Arun Menon",
                    Specialisation = SpecialisationType.GeneralPractitioner,
                    YearsOfExperience = 10,
                    ConsultationFee = 500,
                    IsActive = true,
                    CreatedDate = new DateTime(2026, 6, 15)
                },
                new Doctor
                {
                    DoctorId = 2,
                    DoctorName = "Meera Nair",
                    Specialisation = SpecialisationType.Cardiologist,
                    YearsOfExperience = 15,
                    ConsultationFee = 1000,
                    IsActive = true,
                    CreatedDate = new DateTime(2026, 6, 15)
                },
                new Doctor
                {
                    DoctorId = 3,
                    DoctorName = "Vikram Das",
                    Specialisation = SpecialisationType.Dermatologist,
                    YearsOfExperience = 8,
                    ConsultationFee = 700,
                    IsActive = true,
                    CreatedDate = new DateTime(2026, 6, 15)
                }
            );

            // Appointment seed data
            modelBuilder.Entity<Appointment>().HasData(
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate = new DateTime(2026, 6, 20),
                    TimeSlot = "10:00 AM - 10:30 AM",
                    Status = AppointmentStatus.Pending,
                    CancellationReason = null,
                    CreatedDate = new DateTime(2026, 6, 15)
                },
                new Appointment
                {
                    AppointmentId = 2,
                    PatientId = 2,
                    DoctorId = 2,
                    ScheduledDate = new DateTime(2026, 6, 21),
                    TimeSlot = "11:00 AM - 11:30 AM",
                    Status = AppointmentStatus.Confirmed,
                    CancellationReason = null,
                    CreatedDate = new DateTime(2026, 6, 15)
                },
                new Appointment
                {
                    AppointmentId = 3,
                    PatientId = 3,
                    DoctorId = 3,
                    ScheduledDate = new DateTime(2026, 6, 22),
                    TimeSlot = "02:00 PM - 02:30 PM",
                    Status = AppointmentStatus.Cancelled,
                    CancellationReason = "Patient requested cancellation",
                    CreatedDate = new DateTime(2026, 6, 15)
                }
            );

            // HealthRecord seed data
            modelBuilder.Entity<HealthRecord>().HasData(
                new HealthRecord
                {
                    HealthRecordId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    AppointmentId = 1,
                    VisitDate = new DateTime(2026, 6, 20),
                    Diagnosis = "Fever and cold",
                    Prescription = "Paracetamol 500mg twice daily",
                    Notes = "Drink enough water and take rest",
                    CreatedDate = new DateTime(2026, 6, 15)
                },
                new HealthRecord
                {
                    HealthRecordId = 2,
                    PatientId = 2,
                    DoctorId = 2,
                    AppointmentId = 2,
                    VisitDate = new DateTime(2026, 6, 21),
                    Diagnosis = "Chest discomfort",
                    Prescription = "ECG test and follow-up consultation",
                    Notes = "Avoid heavy exercise until review",
                    CreatedDate = new DateTime(2026, 6, 15)
                },
                new HealthRecord
                {
                    HealthRecordId = 3,
                    PatientId = 3,
                    DoctorId = null,
                    AppointmentId = 3,
                    VisitDate = new DateTime(2026, 6, 22),
                    Diagnosis = "Consultation cancelled",
                    Prescription = "No prescription issued",
                    Notes = "Appointment was cancelled by patient",
                    CreatedDate = new DateTime(2026, 6, 15)
                }
            );

          
        }
    }
}