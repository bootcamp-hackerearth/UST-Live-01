using HealthAxis.Shared.Enums;
using HealthAxis.API.Models;
using HealthAxis.API.Models.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.Data
{
    public class HealthAxisDbContext : IdentityDbContext<ApplicationUser>
    {
        public HealthAxisDbContext(
            DbContextOptions<HealthAxisDbContext> options)
            : base(options) { }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<HealthRecord> HealthRecords { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ UserId should be nullable because seeded patients/doctors
            // do not have Identity login users.
            modelBuilder.Entity<Patient>()
                .Property(p => p.UserId)
                .IsRequired(false);

            modelBuilder.Entity<Doctor>()
                .Property(d => d.UserId)
                .IsRequired(false);

            foreach (var relationship in modelBuilder.Model
                .GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>().HasData(
                new Doctor
                {
                    DoctorId = 1,
                    UserId = null,
                    FullName = "Dr Nandhu",
                    Specialisation = Specialisation.Cardiology,
                    YearsOfExperience = 12,
                    ConsultationFee = 850m,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = 2,
                    UserId = null,
                    FullName = "Dr Kiran",
                    Specialisation = Specialisation.Dermatology,
                    YearsOfExperience = 8,
                    ConsultationFee = 700m,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = 3,
                    UserId = null,
                    FullName = "Dr Rohit",
                    Specialisation = Specialisation.Neurology,
                    YearsOfExperience = 15,
                    ConsultationFee = 1200m,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = 4,
                    UserId = null,
                    FullName = "Dr Prasana",
                    Specialisation = Specialisation.Pediatrics,
                    YearsOfExperience = 6,
                    ConsultationFee = 650m,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = 5,
                    UserId = null,
                    FullName = "Dr Sidhu",
                    Specialisation = Specialisation.Orthopedics,
                    YearsOfExperience = 10,
                    ConsultationFee = 900m,
                    IsActive = true
                }
            );

            modelBuilder.Entity<Patient>().HasData(
                new Patient
                {
                    PatientId = 1,
                    UserId = null,
                    FullName = "Hima",
                    DateOfBirth = new DateTime(1998, 5, 12),
                    Gender = Gender.Female,
                    PhoneNumber = "9876543210",
                    Email = "ananya@example.com",
                    InsuranceId = "INS1001",
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new Patient
                {
                    PatientId = 2,
                    UserId = null,
                    FullName = "Rahul",
                    DateOfBirth = new DateTime(1992, 9, 25),
                    Gender = Gender.Male,
                    PhoneNumber = "9876543211",
                    Email = "rahul@example.com",
                    InsuranceId = "INS1002",
                    CreatedDate = new DateTime(2026, 1, 1)
                }
            );
        }
    }
}