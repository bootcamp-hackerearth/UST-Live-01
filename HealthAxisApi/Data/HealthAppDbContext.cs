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
            
            base.OnModelCreating(modelBuilder);

            

            
            modelBuilder.Entity<HealthRecord>()
                .HasOne(hr => hr.Patient)
                .WithMany()
                .HasForeignKey(hr => hr.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            
            modelBuilder.Entity<HealthRecord>()
                .HasOne(hr => hr.Doctor)
                .WithMany()
                .HasForeignKey(hr => hr.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            
            modelBuilder.Entity<HealthRecord>()
                .HasOne(hr => hr.Appointment)
                .WithMany()
                .HasForeignKey(hr => hr.AppointmentId)
                .OnDelete(DeleteBehavior.NoAction);

            
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}