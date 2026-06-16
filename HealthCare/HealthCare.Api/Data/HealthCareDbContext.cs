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

        public DbSet<User> Users  => Set<User>();
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

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.User)
                .WithOne(u => u.Patient)
                .HasForeignKey<Patient>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Doctor>()
               .HasOne(d => d.User)
               .WithOne(u => u.Doctor)
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

            // -- Seed data --
            SeedData(modelBuilder);
        }

        public static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User { UserId = 1 ,Email="admin@demo.com",PasswordHash="$12QWER.1Fds",Role="Admin", CreatedDate = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) });
            modelBuilder.Entity<User>().HasData(new User { UserId = 2, Email = "patient@demo.com", PasswordHash = "yX9v14Eexx1Fds", Role = "Patient" , CreatedDate = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) });
            modelBuilder.Entity<User>().HasData(new User { UserId = 3, Email = "doctort@demo.com", PasswordHash = "yX9v14Eexx1Fds", Role = "Doctor" , CreatedDate = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero) });

            modelBuilder.Entity<Doctor>().HasData(new Doctor {DoctorId=1,UserId=3,FullName="Dr. Arun Kumar",Specialisation="Cardiology",YearsOfExperience=3,ConsultationFee=500,IsActive=true});

            modelBuilder.Entity<Patient>().HasData(new Patient {PatientId=1,UserId=2,FullName="Abishek",DateOfBirth=new DateOnly(2003,4,7),Gender="Male",PhoneNumber="9876543210" });

        }
    }
}
