using HealthApp.Api.Models;
using HealthApp.Shared.Constants;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Data
{
    public class HealthAppDbContext : IdentityDbContext<ApplicationUser>
    {
        public HealthAppDbContext(
            DbContextOptions<HealthAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<HealthRecord> HealthRecords { get; set; }

        public DbSet<DoctorLeave> DoctorLeaves { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(
            ModelBuilder builder)
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

            builder.Entity<DoctorLeave>()
                .HasOne(leave => leave.Doctor)
                .WithMany(doctor => doctor.DoctorLeaves)
                .HasForeignKey(leave => leave.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            ConfigureOutboxMessage(builder);

            builder.SeedData();
        }

        private static void ConfigureOutboxMessage(
            ModelBuilder builder)
        {
            var outbox = builder.Entity<OutboxMessage>();

            outbox.HasKey(message => message.OutboxMessageId);

            outbox.Property(message => message.EventId)
                .IsRequired();

            outbox.HasIndex(message => message.EventId)
                .IsUnique();

            outbox.Property(message => message.EventType)
                .HasMaxLength(200)
                .IsRequired();

            outbox.Property(message => message.Payload)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            outbox.Property(message => message.Status)
                .HasMaxLength(50)
                .HasDefaultValue(OutboxMessageStatuses.Pending)
                .IsRequired();

            outbox.Property(message => message.RetryCount)
                .HasDefaultValue(0);

            outbox.Property(message => message.CreatedAtUtc)
                .IsRequired();

            outbox.Property(message => message.ErrorMessage)
                .HasMaxLength(2000);

            outbox.HasIndex(message => new
            {
                message.Status,
                message.NextAttemptAtUtc,
                message.CreatedAtUtc
            });
        }
    }
}
