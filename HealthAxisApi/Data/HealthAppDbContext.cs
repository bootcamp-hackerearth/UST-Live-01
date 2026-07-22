using HealthAxisCore_Api.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisCore_Api.Data
{
    public sealed class HealthAppDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public HealthAppDbContext(
            DbContextOptions<HealthAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients =>
            Set<Patient>();

        public DbSet<Doctor> Doctors =>
            Set<Doctor>();

        public DbSet<Appointment> Appointments =>
            Set<Appointment>();

        public DbSet<HealthRecord> HealthRecords =>
            Set<HealthRecord>();

        public DbSet<Notification> Notifications =>
            Set<Notification>();

        public DbSet<RefreshToken> RefreshTokens =>
            Set<RefreshToken>();

        public DbSet<DoctorLeave> DoctorLeaves =>
            Set<DoctorLeave>();

        public DbSet<OutboxMessage> OutboxMessages =>
            Set<OutboxMessage>();

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureRefreshToken(modelBuilder);
            ConfigureHealthRecord(modelBuilder);
            ConfigureAppointment(modelBuilder);
            ConfigureDoctorLeave(modelBuilder);
            ConfigureOutboxMessage(modelBuilder);
        }

        private static void ConfigureRefreshToken(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshToken>()
                .HasOne(refreshToken =>
                    refreshToken.User)
                .WithMany(user =>
                    user.RefreshTokens)
                .HasForeignKey(refreshToken =>
                    refreshToken.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureHealthRecord(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HealthRecord>()
                .HasOne(healthRecord =>
                    healthRecord.Patient)
                .WithMany()
                .HasForeignKey(healthRecord =>
                    healthRecord.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HealthRecord>()
                .HasOne(healthRecord =>
                    healthRecord.Doctor)
                .WithMany()
                .HasForeignKey(healthRecord =>
                    healthRecord.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<HealthRecord>()
                .HasOne(healthRecord =>
                    healthRecord.Appointment)
                .WithMany()
                .HasForeignKey(healthRecord =>
                    healthRecord.AppointmentId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private static void ConfigureAppointment(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>()
                .HasOne(appointment =>
                    appointment.Patient)
                .WithMany()
                .HasForeignKey(appointment =>
                    appointment.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Appointment>()
                .HasOne(appointment =>
                    appointment.Doctor)
                .WithMany()
                .HasForeignKey(appointment =>
                    appointment.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        private static void ConfigureDoctorLeave(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DoctorLeave>()
                .HasOne(doctorLeave =>
                    doctorLeave.Doctor)
                .WithMany(doctor =>
                    doctor.DoctorLeaves)
                .HasForeignKey(doctorLeave =>
                    doctorLeave.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureOutboxMessage(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OutboxMessage>(
                entity =>
                {
                    entity.ToTable("OutboxMessages");

                    entity.HasKey(outboxMessage =>
                        outboxMessage.OutboxMessageId);

                    entity.Property(outboxMessage =>
                            outboxMessage.EventId)
                        .IsRequired();

                    entity.Property(outboxMessage =>
                            outboxMessage.EventType)
                        .HasMaxLength(250)
                        .IsRequired();

                    entity.Property(outboxMessage =>
                            outboxMessage.Payload)
                        .HasColumnType("nvarchar(max)")
                        .IsRequired();

                    entity.Property(outboxMessage =>
                            outboxMessage.Status)
                        .HasMaxLength(30)
                        .IsRequired();

                    entity.Property(outboxMessage =>
                            outboxMessage.RetryCount)
                        .HasDefaultValue(0)
                        .IsRequired();

                    entity.Property(outboxMessage =>
                            outboxMessage.CreatedDate)
                        .IsRequired();

                    entity.Property(outboxMessage =>
                            outboxMessage.PublishedDate)
                        .IsRequired(false);

                    entity.Property(outboxMessage =>
                            outboxMessage.LastAttemptDate)
                        .IsRequired(false);

                    entity.Property(outboxMessage =>
                            outboxMessage.NextRetryDate)
                        .IsRequired(false);

                    entity.Property(outboxMessage =>
                            outboxMessage.ErrorMessage)
                        .HasMaxLength(2000)
                        .IsRequired(false);

                    entity.HasIndex(outboxMessage =>
                            outboxMessage.EventId)
                        .IsUnique();

                    entity.HasIndex(outboxMessage => new
                    {
                        outboxMessage.Status,
                        outboxMessage.NextRetryDate,
                        outboxMessage.CreatedDate
                    })
                        .HasDatabaseName(
                            "IX_OutboxMessages_Publishing");
                });
        }
    }
}