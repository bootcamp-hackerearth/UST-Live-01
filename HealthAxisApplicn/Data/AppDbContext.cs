using HealthAxisApplicn.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisApplicn.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HealthRecord>()
                .HasOne(hr => hr.Doctor)
                .WithMany()
                .HasForeignKey(hr => hr.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HealthRecord>()
                .HasOne(hr => hr.Patient)
                .WithMany()
                .HasForeignKey(hr => hr.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Patient>()
            .HasData(
                new Patient { PatientId = 1, PatientName = "Nakul", DateOfBirth = new DateTime(1990, 12, 03), Email = "nakul@gmail.com", Gender = "Male", PhoneNo = "9887756746", IsActive=true },
                new Patient { PatientId = 2, PatientName = "Nelwyn", DateOfBirth = new DateTime(1995, 08, 04), Email = "nelwyn@gmail.com", Gender = "Male", PhoneNo = "9778864535", IsActive = true },
                new Patient { PatientId = 3, PatientName = "Vikram", DateOfBirth = new DateTime(1978, 01, 07), Email = "vikram@gmail.com", Gender = "Male", PhoneNo = "9335252312", IsActive = true }
                );

            modelBuilder.Entity<Doctor>()
            .HasData(
                new Doctor { DoctorId = 1, DoctorName = "Ranjith Govind", YearsOfExperience = 3, Specialisation = "Cardiologist", ConsultationFee = 1000, IsActive = true },
                new Doctor { DoctorId = 2, DoctorName = "Rahul Nambiar", YearsOfExperience = 4, Specialisation = "Endocrinologist", ConsultationFee = 1200, IsActive = true },
                new Doctor { DoctorId = 3, DoctorName = "Atif Aslam", YearsOfExperience = 2, Specialisation = "Dermatologist", ConsultationFee = 1100, IsActive = true }
                );


        }
    }
}