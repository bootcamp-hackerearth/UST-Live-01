using HealthAxisCore_Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisCore_Api.Data
{
    public class AppDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(
            DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<HealthRecord> HealthRecords { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<AdminHandoffCode> AdminHandoffCodes { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<CancelledAppointmentArchive>
            CancelledAppointmentArchives
        { get; set; }

        protected override void OnModelCreating(
            ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ====================================================
            // ApplicationUser relationships
            // ====================================================

            builder.Entity<ApplicationUser>()
                .HasOne(user => user.Patient)
                .WithOne()
                .HasForeignKey<ApplicationUser>(
                    user => user.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>()
                .HasOne(user => user.Doctor)
                .WithOne()
                .HasForeignKey<ApplicationUser>(
                    user => user.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>()
                .HasMany(user => user.RefreshTokens)
                .WithOne(
                    refreshToken =>
                        refreshToken.ApplicationUser)
                .HasForeignKey(
                    refreshToken =>
                        refreshToken.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ====================================================
            // Appointment relationships
            // ====================================================

            builder.Entity<Appointment>()
                .HasOne(appointment => appointment.Patient)
                .WithMany()
                .HasForeignKey(
                    appointment => appointment.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Appointment>()
                .HasOne(appointment => appointment.Doctor)
                .WithMany()
                .HasForeignKey(
                    appointment => appointment.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // ====================================================
            // Appointment field configuration
            // ====================================================

            // CHANGED:
            // This matches the deployed SQL Server column:
            //
            // TimeSlot NVARCHAR(20) NOT NULL
            //
            // Without HasMaxLength, EF Core would treat the string
            // as NVARCHAR(MAX), which cannot be used as an index key.
            builder.Entity<Appointment>()
                .Property(appointment => appointment.TimeSlot)
                .HasMaxLength(20)
                .IsRequired();

            // ====================================================
            // Appointment unique indexes
            // ====================================================

            // CHANGED:
            // Prevents a doctor from having more than one
            // appointment on the same date and time slot.
            //
            // The explicit database name matches the index already
            // created in the deployed SQL Server database.
            builder.Entity<Appointment>()
                .HasIndex(appointment => new
                {
                    appointment.DoctorId,
                    appointment.ScheduledDate,
                    appointment.TimeSlot
                })
                .IsUnique()
                .HasDatabaseName(
                    "UX_Appointments_DoctorId_ScheduledDate_TimeSlot");

            // CHANGED:
            // Prevents a patient from having more than one
            // appointment on the same date and time slot.
            //
            // The explicit database name matches the index already
            // created in the deployed SQL Server database.
            builder.Entity<Appointment>()
                .HasIndex(appointment => new
                {
                    appointment.PatientId,
                    appointment.ScheduledDate,
                    appointment.TimeSlot
                })
                .IsUnique()
                .HasDatabaseName(
                    "UX_Appointments_PatientId_ScheduledDate_TimeSlot");

            // ====================================================
            // HealthRecord relationships
            // ====================================================

            builder.Entity<HealthRecord>()
                .HasOne(healthRecord => healthRecord.Patient)
                .WithMany()
                .HasForeignKey(
                    healthRecord => healthRecord.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<HealthRecord>()
                .HasOne(healthRecord => healthRecord.Doctor)
                .WithMany()
                .HasForeignKey(
                    healthRecord => healthRecord.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<HealthRecord>()
                .HasOne(healthRecord => healthRecord.Appointment)
                .WithMany()
                .HasForeignKey(
                    healthRecord =>
                        healthRecord.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // ====================================================
            // Notification relationships
            // ====================================================

            builder.Entity<Notification>()
                .HasOne(notification => notification.Doctor)
                .WithMany()
                .HasForeignKey(
                    notification => notification.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Notification>()
                .HasOne(
                    notification =>
                        notification.Appointment)
                .WithMany()
                .HasForeignKey(
                    notification =>
                        notification.AppointmentId)
                .OnDelete(DeleteBehavior.SetNull);

            // ====================================================
            // Cancelled appointment archive indexes
            // ====================================================

            builder.Entity<CancelledAppointmentArchive>()
                .HasIndex(
                    archive =>
                        archive.OriginalAppointmentId);

            builder.Entity<CancelledAppointmentArchive>()
                .HasIndex(
                    archive => archive.PatientId);

            builder.Entity<CancelledAppointmentArchive>()
                .HasIndex(
                    archive => archive.DoctorId);

            builder.Entity<CancelledAppointmentArchive>()
                .HasIndex(
                    archive => archive.ScheduledDate);
        }
    }
}