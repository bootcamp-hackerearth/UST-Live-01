using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using S3_HealthAxis.Shared.Enums;
using S3_HealthAxisApi.Models;

namespace HealthAxis.API.Data
{
    public class HealthAxisDbContext : IdentityDbContext<IdentityUser>
    {
        public HealthAxisDbContext(DbContextOptions<HealthAxisDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<HealthRecord> HealthRecords => Set<HealthRecord>();

        // Keep custom users separate from IdentityDbContext.Users
        public DbSet<User> AppUsers => Set<User>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // IMPORTANT:
            // Keep the custom User entity mapped to the existing Users table
            // even though the DbSet property name is AppUsers.
            builder.Entity<User>().ToTable("Users");

            // Appointment Relationships
            builder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent doctor double-booking
            builder.Entity<Appointment>()
                .HasIndex(a => new
                {
                    a.DoctorId,
                    a.ScheduledDate,
                    a.TimeSlot
                })
                .IsUnique();

            // Health Record Relationships
            builder.Entity<HealthRecord>()
                .HasOne(hr => hr.Appointment)
                .WithOne(a => a.HealthRecord)
                .HasForeignKey<HealthRecord>(hr => hr.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<HealthRecord>()
                .HasOne(hr => hr.Patient)
                .WithMany(p => p.HealthRecords)
                .HasForeignKey(hr => hr.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<HealthRecord>()
                .HasOne(hr => hr.Doctor)
                .WithMany(d => d.HealthRecords)
                .HasForeignKey(hr => hr.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // One Health Record per Appointment
            builder.Entity<HealthRecord>()
                .HasIndex(hr => hr.AppointmentId)
                .IsUnique();

            // Doctor Configuration
            builder.Entity<Doctor>()
                .Property(d => d.ConsultationFee)
                .HasPrecision(10, 2);

            // Seed Patients
            builder.Entity<Patient>().HasData(
                new Patient
                {
                    PatientId = 1,
                    FullName = "Ayush Sharma",
                    DateOfBirth = new DateOnly(1995, 5, 10),
                    Gender = Gender.Male,
                    PhoneNumber = "9876543210",
                    Email = "anand@example.com",
                    InsuranceStatus = InsuranceStatus.Active,
                    InsuranceNumber = "INS1001",
                    IsActive = true
                },
                new Patient
                {
                    PatientId = 2,
                    FullName = "Riya Shukla",
                    DateOfBirth = new DateOnly(2000, 11, 5),
                    Gender = Gender.Female,
                    PhoneNumber = "9876543211",
                    Email = "riya@example.com",
                    InsuranceStatus = InsuranceStatus.Active,
                    InsuranceNumber = "INS1002",
                    IsActive = true
                }
            );

            // Seed Doctors
            builder.Entity<Doctor>().HasData(
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Arun Nair",
                    Specialisation = DoctorSpecialisation.GeneralPractitioner,
                    YearsOfExperience = 8,
                    ConsultationFee = 500.00m,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = 2,
                    FullName = "Rohan Menon",
                    Specialisation = DoctorSpecialisation.Cardiologist,
                    YearsOfExperience = 12,
                    ConsultationFee = 1000.00m,
                    IsActive = true
                }
            );

            // Seed Appointments
            builder.Entity<Appointment>().HasData(
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    ScheduledDate = new DateOnly(2026, 6, 20),
                    TimeSlot = AppointmentTimeSlot.TenAM,
                    Status = AppointmentStatus.Pending
                }
            );
        }
    }
}