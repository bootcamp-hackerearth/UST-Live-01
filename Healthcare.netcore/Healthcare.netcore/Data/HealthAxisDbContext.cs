using HealthAxis.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.Data
{
    public class HealthAxisDbContext : DbContext
    {
        public HealthAxisDbContext(
            DbContextOptions<HealthAxisDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<HealthRecord> HealthRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Admin User Seed Data
            modelBuilder.Entity<User>()
                .HasData(
                    new User
                    {
                        UserId = 1,
                        FullName = "System Admin",
                        Email = "admin@healthaxis.com",
                        PasswordHash = "Admin123",
                        Role = "Admin",
                        IsActive = true
                    }
                );

            // Doctor Seed Data
            modelBuilder.Entity<Doctor>()
                .HasData(
                    new Doctor
                    {
                        DoctorId = 1,
                        FullName = "Dr. Raj Kumar",
                        Specialisation = "Cardiology",
                        YearsOfExperience = 10,
                        ConsultationFee = 800,
                        IsActive = true
                    },
                    new Doctor
                    {
                        DoctorId = 2,
                        FullName = "Dr. Priya Sharma",
                        Specialisation = "Dermatology",
                        YearsOfExperience = 8,
                        ConsultationFee = 600,
                        IsActive = true
                    }
                );
        }
    }
}