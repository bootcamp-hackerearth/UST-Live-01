using HealthApp.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Data
{
    public class HealthAppDbContext : IdentityDbContext<ApplicationUser>
    {
        public HealthAppDbContext(DbContextOptions<HealthAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasOne(user => user.Patient)
                .WithOne(patient => patient.User)
                .HasForeignKey<ApplicationUser>(user => user.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>()
                .HasOne(user => user.Doctor)
                .WithOne(doctor => doctor.User)
                .HasForeignKey<ApplicationUser>(user => user.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}