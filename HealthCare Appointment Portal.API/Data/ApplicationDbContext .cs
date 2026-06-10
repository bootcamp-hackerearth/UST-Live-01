using HealthCare_Appointment_Portal.Models;
using System.Data.Common;
using System.Data.Entity;

namespace HealthCare_Appointment_Portal.Data
{
    public class ApplicationDbContext
        : DbContext
    {
        public ApplicationDbContext()
            : base("HealthCareDb")
        {
        }
        public ApplicationDbContext(DbConnection connection)
        : base(connection, true)
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
            modelBuilder.Conventions.Remove<
                System.Data.Entity.ModelConfiguration.Conventions.OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
