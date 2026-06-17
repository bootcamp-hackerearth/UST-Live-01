using HealthAxisHealth.Shared.Enums;
using HealthAxisHealth.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxisHealth.API.Data
{
    [ExcludeFromCodeCoverage]
    public class ApplicationDbContext : DbContext
    {
        #region Constructor

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        #endregion

        #region DbSets

        public DbSet<User> Users { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<HealthRecord> HealthRecords { get; set; }

        #endregion

        #region Model Configuration

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureUserPatientRelationship(modelBuilder);
            ConfigureUserDoctorRelationship(modelBuilder);
            ConfigureAppointmentRelationships(modelBuilder);
            ConfigureHealthRecordRelationships(modelBuilder);

            modelBuilder.Entity<Doctor>()
                .Property(d => d.ConsultationFee)
                .HasPrecision(18, 2);

            SeedData(modelBuilder);
        }

        #endregion

        #region Relationship Configuration

        private static void ConfigureUserPatientRelationship(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
               .HasOne(p => p.User)
               .WithOne(u => u.Patient)
               .HasForeignKey<Patient>(p => p.UserId)
               .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureUserDoctorRelationship(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.User)
                .WithOne(u => u.Doctor)
                .HasForeignKey<Doctor>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureAppointmentRelationships(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void ConfigureHealthRecordRelationships(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HealthRecord>()
                .HasOne(h => h.Patient)
                .WithMany(p => p.HealthRecords)
                .HasForeignKey(h => h.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HealthRecord>()
                .HasOne(h => h.Doctor)
                .WithMany(d => d.HealthRecords)
                .HasForeignKey(h => h.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.HealthRecord)
                .WithOne(hr => hr.Appointment)
                .HasForeignKey<HealthRecord>(hr => hr.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        #endregion

        #region Seed Data

        private static void SeedData(ModelBuilder modelBuilder)
        {
            const string adminHash =
                "$2a$12$Q2Jb3hFjwJQRUmtUYUxaiewf36VHVzeEAcBkIUwbHyUJNiAl4gMYO";

            const string doctorHash =
                "$2a$12$yu4qYDDFYpEIBQv.Afx./e8.HRq.GhQ2m3x7qBS42BR7v.YgwpM8G";

            DateTime seedDate =
                new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<User>()
                .HasData(
                    new User
                    {
                        UserId = 1,
                        Email = "admin@healthaxis.com",
                        PasswordHash = adminHash,
                        Role = UserRole.Admin,
                        IsActive = true,
                        CreatedDate = seedDate
                    },
                    new User
                    {
                        UserId = 2,
                        Email = "doctor1@healthaxis.com",
                        PasswordHash = doctorHash,
                        Role = UserRole.Doctor,
                        IsActive = true,
                        CreatedDate = seedDate
                    },
                    new User
                    {
                        UserId = 3,
                        Email = "doctor2@healthaxis.com",
                        PasswordHash = doctorHash,
                        Role = UserRole.Doctor,
                        IsActive = true,
                        CreatedDate = seedDate
                    },
                    new User
                    {
                        UserId = 4,
                        Email = "doctor3@healthaxis.com",
                        PasswordHash = doctorHash,
                        Role = UserRole.Doctor,
                        IsActive = true,
                        CreatedDate = seedDate
                    },
                    new User
                    {
                        UserId = 5,
                        Email = "doctor4@healthaxis.com",
                        PasswordHash = doctorHash,
                        Role = UserRole.Doctor,
                        IsActive = true,
                        CreatedDate = seedDate
                    },
                    new User
                    {
                        UserId = 6,
                        Email = "doctor5@healthaxis.com",
                        PasswordHash = doctorHash,
                        Role = UserRole.Doctor,
                        IsActive = true,
                        CreatedDate = seedDate
                    });

            modelBuilder.Entity<Doctor>()
                .HasData(
                    new Doctor
                    {
                        DoctorId = 1,
                        UserId = 2,
                        FullName = "John Smith",
                        Specialisation = Specialisation.Cardiology,
                        YearsOfExperience = 10,
                        ConsultationFee = 800m,
                        IsActive = true,
                        CreatedDate = seedDate
                    },
                    new Doctor
                    {
                        DoctorId = 2,
                        UserId = 3,
                        FullName = "Emily Davis",
                        Specialisation = Specialisation.Dermatology,
                        YearsOfExperience = 8,
                        ConsultationFee = 700m,
                        IsActive = true,
                        CreatedDate = seedDate
                    },
                    new Doctor
                    {
                        DoctorId = 3,
                        UserId = 4,
                        FullName = "Michael Brown",
                        Specialisation = Specialisation.Neurology,
                        YearsOfExperience = 12,
                        ConsultationFee = 1000m,
                        IsActive = true,
                        CreatedDate = seedDate
                    },
                    new Doctor
                    {
                        DoctorId = 4,
                        UserId = 5,
                        FullName = "Sarah Wilson",
                        Specialisation = Specialisation.Pediatrics,
                        YearsOfExperience = 7,
                        ConsultationFee = 600m,
                        IsActive = true,
                        CreatedDate = seedDate
                    },
                    new Doctor
                    {
                        DoctorId = 5,
                        UserId = 6,
                        FullName = "David Johnson",
                        Specialisation = Specialisation.Orthopedics,
                        YearsOfExperience = 15,
                        ConsultationFee = 900m,
                        IsActive = true,
                        CreatedDate = seedDate
                    });
        }

        #endregion
    }
}
