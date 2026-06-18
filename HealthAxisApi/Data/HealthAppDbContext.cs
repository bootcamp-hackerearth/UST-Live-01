using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HealthAxisCore_Api.Models;

namespace HealthAxisCore_Api.Data
{
    // ✅ IMPORTANT: Inherit from IdentityDbContext<ApplicationUser>
    public class HealthAppDbContext : IdentityDbContext<ApplicationUser>
    {
        public HealthAppDbContext(DbContextOptions<HealthAppDbContext> options)
            : base(options)
        {
        }

        // ✅ REMOVE old User DbSet (Identity handles users)
        // public DbSet<User> Users { get; set; }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ✅ VERY IMPORTANT: Call Identity configuration FIRST
            base.OnModelCreating(modelBuilder);

            // ✅ Fix multiple cascade paths issue

            // HealthRecord → Patient
            modelBuilder.Entity<HealthRecord>()
                .HasOne(hr => hr.Patient)
                .WithMany()
                .HasForeignKey(hr => hr.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            // HealthRecord → Doctor
            modelBuilder.Entity<HealthRecord>()
                .HasOne(hr => hr.Doctor)
                .WithMany()
                .HasForeignKey(hr => hr.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            // HealthRecord → Appointment
            modelBuilder.Entity<HealthRecord>()
                .HasOne(hr => hr.Appointment)
                .WithMany()
                .HasForeignKey(hr => hr.AppointmentId)
                .OnDelete(DeleteBehavior.NoAction);

            // Appointment → Patient
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            // Appointment → Doctor
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}