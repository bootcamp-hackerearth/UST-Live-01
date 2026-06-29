    using HealthAxisApplicn.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    namespace HealthAxisApplicn.Data
    {
        public class AppDbContext : IdentityDbContext<ApplicationUser>
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {

            }

            public DbSet<Patient> Patients { get; set; }
            public DbSet<Doctor> Doctors { get; set; }
            public DbSet<Appointment> Appointments { get; set; }
            public DbSet<HealthRecord> HealthRecords { get; set; }
            public DbSet<RefreshToken> RefreshTokens { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<HealthRecord>()
                    .HasOne(hr => hr.Doctor)
                    .WithMany()
                    .HasForeignKey(hr => hr.DoctorId)
                    .OnDelete(DeleteBehavior.NoAction);

                modelBuilder.Entity<HealthRecord>()
                    .HasOne(hr => hr.Patient)
                    .WithMany()
                    .HasForeignKey(hr => hr.PatientId)
                    .OnDelete(DeleteBehavior.NoAction);
                
                modelBuilder.Entity<HealthRecord>()
                    .HasOne(hr => hr.Appointment)
                    .WithOne() 
                    .HasForeignKey<HealthRecord>(hr => hr.AppointmentId);

                base.OnModelCreating(modelBuilder);
                modelBuilder.Entity<ApplicationUser>()
                    .HasOne(u => u.Patient)
                    .WithOne(p => p.User)
                    .HasForeignKey<Patient>(p => p.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                modelBuilder.Entity<ApplicationUser>()
                    .HasOne(u => u.Doctor)
                    .WithOne(d => d.User)
                    .HasForeignKey<Doctor>(d => d.UserId)
                    .OnDelete(DeleteBehavior.NoAction);


            }
        }
    }