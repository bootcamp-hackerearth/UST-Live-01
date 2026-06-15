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

            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(x => x.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Doctor>()
                .Property(x => x.Specialisation)
                .HasConversion<string>();

            modelBuilder.Entity<Appointment>()
                .Property(x => x.Status)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
                .HasOne(x => x.Doctor)
                .WithOne(x => x.User)
                .HasForeignKey<Doctor>(x => x.UserId);

            modelBuilder.Entity<User>()
                .HasOne(x => x.Patient)
                .WithOne(x => x.User)
                .HasForeignKey<Patient>(x => x.UserId);
        }
    }
}