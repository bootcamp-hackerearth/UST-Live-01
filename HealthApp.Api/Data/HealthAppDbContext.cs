using HealthApp.Api.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthApp.Api.Data
{
    public class HealthAppDbContext : IdentityDbContext<IdentityUser>
    {
        public HealthAppDbContext(DbContextOptions<HealthAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<DoctorLeave> DoctorLeaves { get; set; }
        public DbSet<Notification> notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>()
                .HasIndex(p => p.Email)
                .IsUnique();

            modelBuilder.Entity<Patient>()
                .HasData(new Patient
                    {
                        PatientId = 1,
                        FullName = "John Mathew",
                        DateOfBirth = new DateTime(1990, 1, 1),
                        Gender = "Male",
                        Email = "johnmathew@gmail.com",
                        PhoneNumber = "8796534521",
                        InsuranceId = "INS001"
                    },
                    new Patient
                    {
                        PatientId = 2,
                        FullName = "Ramesh Kumar",
                        DateOfBirth = new DateTime(1985, 4, 12),
                        Gender = "Male",
                        Email = "rameshk@gmail.com",
                        PhoneNumber = "9876543210",
                        InsuranceId = "INS1002"
                    },

                    new Patient
                    {
                        PatientId = 3,
                        FullName = "Priya Sharma",
                        DateOfBirth = new DateTime(1992, 9, 25),
                        Gender = "Female",
                        Email = "priya@gmail.com",
                        PhoneNumber = "8765432109",
                        InsuranceId = "INS1003"
                    },

                    new Patient
                    {
                        PatientId = 4,
                        FullName = "Arun Raj",
                        DateOfBirth = new DateTime(2000, 1, 8),
                        Gender = "Male",
                        Email = "arunraj@gmail.com",
                        PhoneNumber = "7654321098",
                        InsuranceId = "INS1004"
                    },

                    new Patient
                    {
                        PatientId = 5,
                        FullName = "Meena Devi",
                        DateOfBirth = new DateTime(2001, 11, 30),
                        Gender = "Female",
                        Email = "meenu@gmail.com",
                        PhoneNumber = "6543210987",
                        InsuranceId = "INS1005"
                    }
                );


            modelBuilder.Entity<Doctor>()
                .HasIndex(d => d.Email)
                .IsUnique();

            modelBuilder.Entity<Doctor>()
                .HasData(
                    new Doctor
                    {
                        DoctorId = 1,
                        FullName = "Vignesh Kumar",
                        Specialisation = "Orthopedic",
                        PracticeStartDate = new DateTime(2002, 11, 30),
                        Email = "Vignesh@gmail.com",
                        ConsultationFee = 1500,
                        IsActive = true,
                        DoctorPhoneNumber="1234567890"

                    },

                    new Doctor
                    {
                        DoctorId = 2,
                        FullName = "Sneha Paul",
                        Specialisation = "Gynecologist",
                        PracticeStartDate = new DateTime(2004, 01, 01),
                        Email = "paul@gmail.com",
                        ConsultationFee = 1800,
                        IsActive = true,
                        DoctorPhoneNumber="6789012345"
                    },

                    new Doctor
                    {
                        DoctorId = 3,
                        FullName = "Hari Narayanan",
                        Specialisation = "ENT",
                        PracticeStartDate = new DateTime(2012, 10, 03),
                        Email = "hari@gmail.com",
                        ConsultationFee = 900,
                        IsActive = true,
                        DoctorPhoneNumber = "1278906784"
                    },

                    new Doctor
                    {
                        DoctorId = 4,
                        FullName = "Martin Smith",
                        Specialisation = "Psychiatrist",
                        PracticeStartDate = new DateTime(2009, 05, 30),
                        Email = "smith@gmail.com",
                        ConsultationFee = 2000,
                        IsActive = false,
                        DoctorPhoneNumber="0987654321"
                        
                    },

                    new Doctor
                    {
                        DoctorId = 5,
                        FullName = "Bharath Raj",
                        Specialisation = "Cardiologist",
                        PracticeStartDate = new DateTime(2010, 10, 01),
                        Email = "raj@gmail.com",
                        ConsultationFee = 3000,
                        IsActive = true,
                        DoctorPhoneNumber="1234509874"
                    }
                );
        }
    }
}

