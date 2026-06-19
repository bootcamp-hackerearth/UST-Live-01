using HealthApp.Api.Model;
using HospitalManagementAPI.Model;
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
                        YearsOfExperience = 14,
                        Email = "Vignesh@gmail.com",
                        ConsultationFee = 1500,
                        IsActive = true
                    },

                    new Doctor
                    {
                        DoctorId = 2,
                        FullName = "Sneha Paul",
                        Specialisation = "Gynecologist",
                        YearsOfExperience = 20,
                        Email = "paul@gmail.com",
                        ConsultationFee = 1800,
                        IsActive = true
                    },

                    new Doctor
                    {
                        DoctorId = 3,
                        FullName = "Hari Narayanan",
                        Specialisation = "ENT",
                        YearsOfExperience = 9,
                        Email = "hari@gmail.com",
                        ConsultationFee = 900,
                        IsActive = true
                    },

                    new Doctor
                    {
                        DoctorId = 4,
                        FullName = "Martin Smith",
                        Specialisation = "Psychiatrist",
                        YearsOfExperience = 11,
                        Email = "smith@gmail.com",
                        ConsultationFee = 2000,
                        IsActive = false
                    },

                    new Doctor
                    {
                        DoctorId = 5,
                        FullName = "Bharath Raj",
                        Specialisation = "Cardiologist",
                        YearsOfExperience = 25,
                        Email = "raj@gmail.com",
                        ConsultationFee = 3000,
                        IsActive = true
                    }
                );
        }
    }
}

