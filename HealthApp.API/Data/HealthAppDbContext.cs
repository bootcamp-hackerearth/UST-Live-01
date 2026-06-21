using HealthApp.API.Identity;
using HealthApp.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.API.Data;

public class HealthAppDbContext : IdentityDbContext<ApplicationUser>
{
    public HealthAppDbContext(DbContextOptions<HealthAppDbContext> options) : base(options) { }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<HealthRecord> HealthRecords => Set<HealthRecord>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient).WithMany(p => p.Appointments).HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor).WithMany(d => d.Appointments).HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<HealthRecord>()
            .HasOne(h => h.Patient).WithMany(p => p.HealthRecords).HasForeignKey(h => h.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<HealthRecord>()
            .HasOne(h => h.Doctor).WithMany(d => d.HealthRecords).HasForeignKey(h => h.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<HealthRecord>()
            .HasOne(h => h.Appointment).WithOne(a => a.HealthRecord).HasForeignKey<HealthRecord>(h => h.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Patient>().Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()");
        modelBuilder.Entity<Doctor>().Property(d => d.CreatedDate).HasDefaultValueSql("GETDATE()");
        modelBuilder.Entity<Appointment>().Property(a => a.CreatedDate).HasDefaultValueSql("GETDATE()");
        modelBuilder.Entity<HealthRecord>().Property(h => h.CreatedDate).HasDefaultValueSql("GETDATE()");
    }
}
