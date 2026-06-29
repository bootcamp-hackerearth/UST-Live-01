using Microsoft.EntityFrameworkCore;
using HealthCare.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace HealthCare.Api.Data
{
    public class HealthCareDbContext : IdentityDbContext<IdentityUser>
    {
        public HealthCareDbContext(DbContextOptions<HealthCareDbContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<Doctor> Doctors => Set<Doctor>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<HealthRecord> HealthRecords => Set<HealthRecord>();
        public DbSet<DoctorLeaves> DoctorLeaves => Set<DoctorLeaves>();
        public DbSet<AvailableSlots> AvailableSlots => Set<AvailableSlots>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointment>()
            .HasIndex(a => new { a.DoctorId, a.ScheduledDate, a.TimeSlot })
                .IsUnique()
                .HasFilter("[Status] != 'Cancelled'")
                .HasDatabaseName("UQ_Appointments_Doctor_Date_Slot");


            modelBuilder.Entity<Appointment>()
                .HasIndex(a => new { a.DoctorId, a.ScheduledDate })
                .HasDatabaseName("IX_Appointments_Doctor_Date");

            modelBuilder.Entity<Appointment>()
                .HasIndex(a => new { a.PatientId, a.ScheduledDate })
                .HasDatabaseName("IX_Appointments_Patient_Date");

            modelBuilder.Entity<HealthRecord>()
                 .HasIndex(hr => new { hr.PatientId, hr.VisitDate })
                 .HasDatabaseName("IX_HealthRecords_Patient_VisitDate");


            modelBuilder.Entity<Doctor>()
                .HasIndex(d => new { d.Specialisation, d.IsActive })
                .HasDatabaseName("IX_Doctors_Specialisation_IsActive");

            modelBuilder.Entity<DoctorLeaves>()
                .HasIndex(l => new { l.DoctorId, l.LeaveDate })
                .HasDatabaseName("IX_Leaves_Doctor_Date");


            modelBuilder.Entity<Patient>()
                .HasOne(p => p.User)
                .WithOne(u => u.Patient)
                .HasForeignKey<Patient>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Doctor>()
               .HasOne(d => d.User)
               .WithOne()
               .HasForeignKey<Doctor>(d => d.UserId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HealthRecord>()
                .HasOne(hr => hr.Appointment)
                .WithOne(a => a.HealthRecord)
                .HasForeignKey<HealthRecord>(hr => hr.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HealthRecord>()
                .HasOne(hr => hr.Patient)
                .WithMany(p => p.HealthRecords)
                .HasForeignKey(hr => hr.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HealthRecord>()
                .HasOne(hr => hr.Doctor)
                .WithMany(d => d.HealthRecords)
                .HasForeignKey(hr => hr.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);
        }

     
    }
}
