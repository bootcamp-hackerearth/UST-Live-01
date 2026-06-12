using HealthCareApi.Data.Configurations;
using HealthCareApi.Data.Seed;
using HealthCareApi.Models;
using HealthCareApi.Models.Views;
using System.Data.Entity;

namespace HealthCareApi.Data.Context
{
    public class HealthCareDbContext : DbContext
    {
        public HealthCareDbContext()
            : base("name=HealthCareConnection")   // matches Web.config connection string
        {
            Database.SetInitializer(new HealthCareDbInitializer());
        }

        // Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<DoctorAvailableSlot> DoctorAvailableSlots { get; set; }
        public DbSet<DoctorLeave> DoctorLeaves { get; set; }

        // Views (read-only)
        //public DbSet<VwPatientAppointment> VwPatientAppointments { get; set; }
        //public DbSet<VwDoctorSchedule> VwDoctorSchedules { get; set; }
        //public DbSet<VwPatientHealthHistory> VwPatientHealthHistories { get; set; }
        //public DbSet<VwDoctorProfile> VwDoctorProfiles { get; set; }
        //public DbSet<VwPatientProfile> VwPatientProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new PatientConfiguration());
            modelBuilder.Configurations.Add(new DoctorConfiguration());
            modelBuilder.Configurations.Add(new AppointmentConfiguration());
            modelBuilder.Configurations.Add(new HealthRecordConfiguration());
            //modelBuilder.Configurations.Add(new DoctorAvailableSlotConfiguration());
            //modelBuilder.Configurations.Add(new DoctorLeaveConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}