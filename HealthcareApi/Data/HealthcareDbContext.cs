using System.Data.Entity;
using HealthcareApi.Models;

namespace HealthcareApi.Data
{
    public class HealthcareDbContext : DbContext
    {
        public HealthcareDbContext()
            : base("HealthcareDbContext")
        {
        }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<HealthRecord> HealthRecords { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>()
                .HasKey(p => p.PatientId);

            modelBuilder.Entity<Doctor>()
                .HasKey(d => d.DoctorId);

            modelBuilder.Entity<Appointment>()
                .HasKey(a => a.AppointmentId);

            modelBuilder.Entity<HealthRecord>()
                .HasKey(h => h.HealthRecordId);

            modelBuilder.Entity<Appointment>()
                .HasRequired(a => a.Patient)
                .WithMany()
                .HasForeignKey(a => a.PatientId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Appointment>()
                .HasRequired(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.DoctorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HealthRecord>()
                .HasRequired(h => h.Patient)
                .WithMany()
                .HasForeignKey(h => h.PatientId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HealthRecord>()
                .HasRequired(h => h.Doctor)
                .WithMany()
                .HasForeignKey(h => h.DoctorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HealthRecord>()
                .HasRequired(h => h.Appointment)
                .WithMany()
                .HasForeignKey(h => h.AppointmentId)
                .WillCascadeOnDelete(false);
        }
    }
}
