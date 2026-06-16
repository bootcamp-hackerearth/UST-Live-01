using HealthAxis.API.Enums;
using HealthAxis.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.Data
{
    public class HealthAxisDbContext : DbContext
    {
        public HealthAxisDbContext(DbContextOptions<HealthAxisDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<HealthRecord> HealthRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureUser(modelBuilder);
            ConfigurePatient(modelBuilder);
            ConfigureDoctor(modelBuilder);
            ConfigureAppointment(modelBuilder);
            ConfigureHealthRecord(modelBuilder);
            SeedData(modelBuilder);
        }

        private static void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(user => user.UserId);

                entity.Property(user => user.UserCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(user => user.Email)
                    .HasMaxLength(100);

                entity.Property(user => user.PasswordHash)
                    .HasMaxLength(256);

                entity.Property(user => user.Role)
                    .IsRequired()
                    .HasConversion<int>();

                entity.Property(user => user.ReferenceId)
                    .IsRequired();

                entity.Property(user => user.IsActive)
                    .IsRequired();

                entity.HasIndex(user => user.UserCode)
                    .IsUnique();

                entity.HasIndex(user => user.Email)
                    .IsUnique();
            });
        }

        private static void ConfigurePatient(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(patient => patient.PatientId);

                entity.Property(patient => patient.FullName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(patient => patient.DateOfBirth)
                    .IsRequired();

                entity.Property(patient => patient.Gender)
                    .IsRequired()
                    .HasConversion<int>();

                entity.Property(patient => patient.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(patient => patient.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(patient => patient.CreatedDate)
                    .IsRequired();

                entity.HasIndex(patient => patient.Email)
                    .IsUnique();

                entity.HasIndex(patient => patient.PhoneNumber)
                    .IsUnique();
            });
        }

        private static void ConfigureDoctor(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(doctor => doctor.DoctorId);

                entity.Property(doctor => doctor.FullName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(doctor => doctor.Specialisation)
                    .IsRequired()
                    .HasConversion<int>();

                entity.Property(doctor => doctor.YearsOfExperience)
                    .IsRequired();

                entity.Property(doctor => doctor.ConsultationFee)
                    .IsRequired();

                entity.Property(doctor => doctor.IsActive)
                    .IsRequired();
            });
        }

        private static void ConfigureAppointment(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(appointment => appointment.AppointmentId);

                entity.Property(appointment => appointment.PatientId)
                    .IsRequired();

                entity.Property(appointment => appointment.DoctorId)
                    .IsRequired();

                entity.Property(appointment => appointment.ScheduledDate)
                    .IsRequired();

                entity.Property(appointment => appointment.TimeSlot)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(appointment => appointment.Status)
                    .IsRequired()
                    .HasConversion<int>();

                entity.Property(appointment => appointment.CancellationReason)
                    .HasMaxLength(250);

                entity.HasOne(appointment => appointment.Patient)
                    .WithMany(patient => patient.Appointments)
                    .HasForeignKey(appointment => appointment.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(appointment => appointment.Doctor)
                    .WithMany(doctor => doctor.Appointments)
                    .HasForeignKey(appointment => appointment.DoctorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void ConfigureHealthRecord(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HealthRecord>(entity =>
            {
                entity.HasKey(record => record.RecordId);

                entity.Property(record => record.AppointmentId)
                    .IsRequired();

                entity.Property(record => record.PatientId)
                    .IsRequired();

                entity.Property(record => record.DoctorId)
                    .IsRequired();

                entity.Property(record => record.VisitDate)
                    .IsRequired();

                entity.Property(record => record.Diagnosis)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(record => record.Prescription)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(record => record.Notes)
                    .HasMaxLength(1000);

                entity.HasOne(record => record.Appointment)
                    .WithMany(appointment => appointment.HealthRecords)
                    .HasForeignKey(record => record.AppointmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(record => record.Patient)
                    .WithMany(patient => patient.HealthRecords)
                    .HasForeignKey(record => record.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(record => record.Doctor)
                    .WithMany(doctor => doctor.HealthRecords)
                    .HasForeignKey(record => record.DoctorId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>().HasData(
                new Doctor
                {
                    DoctorId = 1,
                    FullName = "Dr. Meera Nair",
                    Specialisation = Specialisation.Cardiology,
                    YearsOfExperience = 10,
                    ConsultationFee = 800,
                    IsActive = true
                },
                new Doctor
                {
                    DoctorId = 2,
                    FullName = "Dr. Arjun Menon",
                    Specialisation = Specialisation.GeneralMedicine,
                    YearsOfExperience = 7,
                    ConsultationFee = 600,
                    IsActive = true
                }
            );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    UserCode = "ADM001",
                    Email = "admin@healthaxis.com",
                    PasswordHash = "CHANGE_THIS_HASH_LATER",
                    Role = Role.Admin,
                    ReferenceId = 0,
                    IsActive = true
                }
            );
        }
    }
}