using HealthCare_Appointment_Portal.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace HealthCare_Appointment_Portal.Data
{
    public class ApplicationDbContext
        : DbContext
    {
        public ApplicationDbContext()
            : base("HealthCareDb")
        {
        }

        public DbSet<Patient> Patients
        {
            get;
            set;
        }

        public DbSet<Doctor> Doctors
        {
            get;
            set;
        }

        public DbSet<Appointment> Appointments
        {
            get;
            set;
        }

        public DbSet<HealthRecord> HealthRecords
        {
            get;
            set;
        }

        public DbSet<Insurance> Insurances
        {
            get;
            set;
        }

        public DbSet<User> Users
        {
            get;
            set;
        }


        protected override void OnModelCreating(
        DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }
}
}