using HealthAxis.API.Models;
using HealthAxis.Shared.Enums;
using HealthAxis.Shared.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<HealthRecord> HealthRecords { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureAppointmentRelationships(builder);
            ConfigureHealthRecordRelationships(builder);
            ConfigureNotification(builder);

            builder.Entity<Doctor>()
                .Property(doctor => doctor.ConsultationFee)
                .HasPrecision(10, 2);

            builder.Entity<Patient>()
                .HasIndex(patient => patient.Email)
                .IsUnique();

            builder.Entity<Patient>()
                .HasOne(patient => patient.User)
                .WithOne()
                .HasForeignKey<Patient>(patient => patient.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Patient>()
                .Property(patient => patient.Gender)
                .HasConversion<string>()
                .HasMaxLength(10);

            builder.Entity<Doctor>()
                .Property(doctor => doctor.Specialisation)
                .HasConversion<string>()
                .HasMaxLength(50);

            builder.Entity<Appointment>()
                .Property(appointment => appointment.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Entity<Appointment>()
                .Property(appointment => appointment.CancellationReason)
                .HasMaxLength(
                    ValidationLimits.CancellationReasonLength);

            ConfigureAppointmentIndexes(builder);

            builder.Entity<Doctor>()
                .HasOne(doctor => doctor.User)
                .WithOne()
                .HasForeignKey<Doctor>(doctor => doctor.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            SeedData(builder);
        }

        private static void ConfigureAppointmentRelationships(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>()
                .HasOne(appointment => appointment.Patient)
                .WithMany(patient => patient.Appointments)
                .HasForeignKey(appointment => appointment.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(appointment => appointment.Doctor)
                .WithMany(doctor => doctor.Appointments)
                .HasForeignKey(appointment => appointment.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void ConfigureAppointmentIndexes(
            ModelBuilder modelBuilder)
        {
            const string activeAppointmentFilter =
                "[Status] IN ('Pending', 'Confirmed')";

            modelBuilder.Entity<Appointment>()
                .HasIndex(appointment => new
                {
                    appointment.DoctorId,
                    appointment.ScheduledDate,
                    appointment.TimeSlot
                })
                .HasDatabaseName(
                    "UX_Appointments_Doctor_Slot_Active")
                .IsUnique()
                .HasFilter(activeAppointmentFilter);

            modelBuilder.Entity<Appointment>()
                .HasIndex(appointment => new
                {
                    appointment.PatientId,
                    appointment.ScheduledDate,
                    appointment.TimeSlot
                })
                .HasDatabaseName(
                    "UX_Appointments_Patient_Slot_Active")
                .IsUnique()
                .HasFilter(activeAppointmentFilter);
        }

        private static void ConfigureHealthRecordRelationships(
            ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HealthRecord>()
                .HasKey(
                    healthRecord =>
                        healthRecord.HealthRecordId);

            modelBuilder.Entity<HealthRecord>()
                .Property(
                    healthRecord =>
                        healthRecord.HealthRecordId)
                .HasColumnName("RecordId");

            modelBuilder.Entity<HealthRecord>()
                .HasOne(healthRecord => healthRecord.Patient)
                .WithMany(patient => patient.HealthRecords)
                .HasForeignKey(
                    healthRecord =>
                        healthRecord.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HealthRecord>()
                .HasOne(healthRecord => healthRecord.Doctor)
                .WithMany(doctor => doctor.HealthRecords)
                .HasForeignKey(
                    healthRecord =>
                        healthRecord.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(appointment => appointment.HealthRecord)
                .WithOne(healthRecord => healthRecord.Appointment)
                .HasForeignKey<HealthRecord>(
                    healthRecord =>
                        healthRecord.AppointmentId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private static void ConfigureNotification(
     ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>()
                .HasKey(
                    notification =>
                        notification.NotificationId);

            modelBuilder.Entity<Notification>()
                .Property(notification => notification.Title)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<Notification>()
                .Property(notification => notification.Message)
                .HasMaxLength(1000)
                .IsRequired();

            modelBuilder.Entity<Notification>()
                .Property(
                    notification =>
                        notification.NotificationType)
                .HasMaxLength(50);

            modelBuilder.Entity<Notification>()
                .HasIndex(
                    notification =>
                        notification.CreatedDate);

            modelBuilder.Entity<Notification>()
                .HasIndex(
                    notification =>
                        notification.PatientId);

            modelBuilder.Entity<Notification>()
                .HasIndex(
                    notification =>
                        notification.DoctorId);

            modelBuilder.Entity<Notification>()
                .HasIndex(notification => new
                {
                    notification.AppointmentId,
                    notification.DoctorId,
                    notification.NotificationType
                })
                .HasDatabaseName(
                    "UX_Notifications_Appointment_Doctor_Type")
                .IsUnique()
                .HasFilter(
                    "[AppointmentId] IS NOT NULL AND " +
                    "[DoctorId] IS NOT NULL");
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>().HasData(
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Dr Arjun Reddy",
                    Specialisation =
                        Specialisation.Cardiology,
                    YearsOfExperience = 12,
                    ConsultationFee = 850m,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = 2,
                    FullName = "Dr Navya ",
                    Specialisation =
                        Specialisation.Dermatology,
                    YearsOfExperience = 8,
                    ConsultationFee = 700m,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = 3,
                    FullName = "Dr Rohit Shetty",
                    Specialisation =
                        Specialisation.Neurology,
                    YearsOfExperience = 15,
                    ConsultationFee = 1200m,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = 4,
                    FullName = "Dr Priya Kumar",
                    Specialisation =
                        Specialisation.Pediatrics,
                    YearsOfExperience = 6,
                    ConsultationFee = 650m,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = 5,
                    FullName = "Dr Siddharth Rao",
                    Specialisation =
                        Specialisation.Orthopedics,
                    YearsOfExperience = 10,
                    ConsultationFee = 900m,
                    IsActive = true
                });
        }
    }
}