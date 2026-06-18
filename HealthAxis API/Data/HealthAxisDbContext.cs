using HealthAxis.API.Enums;
using HealthAxis.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthAxis.API.Data
{
    public class HealthAxisDbContext : IdentityDbContext<IdentityUser>
    {
        public HealthAxisDbContext(DbContextOptions<HealthAxisDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<HealthRecord> HealthRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(patient => patient.PatientId);

                entity.Property(patient => patient.FullName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(patient => patient.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(patient => patient.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(patient => patient.Gender)
                    .HasConversion<int>();

                entity.HasIndex(patient => patient.Email)
                    .IsUnique();
            });

            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasKey(doctor => doctor.DoctorId);

                entity.Property(doctor => doctor.FullName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(doctor => doctor.Specialisation)
                    .HasConversion<int>();

                entity.Property(doctor => doctor.ConsultationFee)
                    .HasPrecision(18, 2);
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.HasKey(appointment => appointment.AppointmentId);

                entity.Property(appointment => appointment.TimeSlot)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(appointment => appointment.Status)
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

            modelBuilder.Entity<HealthRecord>(entity =>
            {
                entity.HasKey(record => record.RecordId);

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
        }
    }
}