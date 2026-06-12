using Healthaxis2.Models;
using System.Data.Entity;

namespace Healthaxis2.Data
{
    public class AppDbContext : DbContext
    {
        // ✅ MUST MATCH Web.config connection name
        public AppDbContext() : base("name=HealthAxis2Connection")
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ TABLE MAPPING (match SQL)
            modelBuilder.Entity<Patient>().ToTable("patients");
            modelBuilder.Entity<Doctor>().ToTable("doctors");
            modelBuilder.Entity<Appointment>().ToTable("appointments");
            modelBuilder.Entity<HealthRecord>().ToTable("HealthRecords");

            // ✅ RELATIONSHIPS

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