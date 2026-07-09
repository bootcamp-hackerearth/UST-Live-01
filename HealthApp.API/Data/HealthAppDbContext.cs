using HealthApp.API.Identity;
using HealthApp.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.API.Data;

public class HealthAppDbContext : IdentityDbContext<ApplicationUser>
{
    private const string SqlDefaultCurrentDate = "GETDATE()";

    public HealthAppDbContext(DbContextOptions<HealthAppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<HealthRecord> HealthRecords => Set<HealthRecord>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Appointment>()
            .HasOne(appointment => appointment.Patient)
            .WithMany(patient => patient.Appointments)
            .HasForeignKey(appointment => appointment.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Appointment>()
            .HasOne(appointment => appointment.Doctor)
            .WithMany(doctor => doctor.Appointments)
            .HasForeignKey(appointment => appointment.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<HealthRecord>()
            .HasOne(healthRecord => healthRecord.Patient)
            .WithMany(patient => patient.HealthRecords)
            .HasForeignKey(healthRecord => healthRecord.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<HealthRecord>()
            .HasOne(healthRecord => healthRecord.Doctor)
            .WithMany(doctor => doctor.HealthRecords)
            .HasForeignKey(healthRecord => healthRecord.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<HealthRecord>()
            .HasOne(healthRecord => healthRecord.Appointment)
            .WithOne(appointment => appointment.HealthRecord)
            .HasForeignKey<HealthRecord>(healthRecord => healthRecord.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Notification>()
            .HasOne(notification => notification.Doctor)
            .WithMany()
            .HasForeignKey(notification => notification.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Notification>()
            .HasOne(notification => notification.Appointment)
            .WithMany()
            .HasForeignKey(notification => notification.AppointmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Patient>()
            .Property(patient => patient.CreatedDate)
            .HasDefaultValueSql(SqlDefaultCurrentDate);

        builder.Entity<Doctor>()
            .Property(doctor => doctor.CreatedDate)
            .HasDefaultValueSql(SqlDefaultCurrentDate);

        builder.Entity<Appointment>()
            .Property(appointment => appointment.CreatedDate)
            .HasDefaultValueSql(SqlDefaultCurrentDate);

        builder.Entity<HealthRecord>()
            .Property(healthRecord => healthRecord.CreatedDate)
            .HasDefaultValueSql(SqlDefaultCurrentDate);

        builder.Entity<Notification>()
            .Property(notification => notification.CreatedDate)
            .HasDefaultValueSql(SqlDefaultCurrentDate);
    }
}
