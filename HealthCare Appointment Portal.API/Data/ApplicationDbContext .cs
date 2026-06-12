using System.Data.Entity;
using HealthCare_Appointment_Portal.Models;

namespace HealthCare_Appointment_Portal.Data
{
    public class ApplicationDbContext
        : DbContext
    {
        public ApplicationDbContext()
            : base("HealthCareDb")
        {
        }

        public virtual DbSet<Patient>
            Patients
        {
            get;
            set;
        }

        public virtual DbSet<Doctor>
            Doctors
        {
            get;
            set;
        }

        public virtual DbSet<Appointment>
            Appointments
        {
            get;
            set;
        }

        public virtual DbSet<HealthRecord>
            HealthRecords
        {
            get;
            set;
        }

        public virtual DbSet<Insurance>
            Insurances
        {
            get;
            set;
        }

        public virtual DbSet<User>
            Users
        {
            get;
            set;
        }

        protected override void OnModelCreating(
            DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HealthRecord>()
                .HasRequired(h => h.Doctor)
                .WithMany(d => d.HealthRecords)
                .HasForeignKey(h => h.DoctorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HealthRecord>()
                .HasRequired(h => h.Patient)
                .WithMany(p => p.HealthRecords)
                .HasForeignKey(h => h.PatientId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(
                modelBuilder);
        }
    }
}