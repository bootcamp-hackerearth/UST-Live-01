using HealthAxisCore_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisCore_Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Doctor>()
                .HasData(
                    new Doctor { DoctorId = 1, DoctorName = "Arjun Nair", Specialisation = "Cardiologist", YearsOfExperience = 12, ConsultationFee = 800, IsActive = true },
                    new Doctor { DoctorId = 2, DoctorName = "Meera Pillai", Specialisation = "Dermatologist", YearsOfExperience = 8, ConsultationFee = 600, IsActive = true },
                    new Doctor { DoctorId = 3, DoctorName = "Rahul Menon", Specialisation = "Neurologist", YearsOfExperience = 15, ConsultationFee = 1200, IsActive = true },
                    new Doctor { DoctorId = 4, DoctorName = "Anjali Nair", Specialisation = "Gynecologist", YearsOfExperience = 10, ConsultationFee = 700, IsActive = true },
                    new Doctor { DoctorId = 5, DoctorName = "Vivek Kumar", Specialisation = "OrthopedicSurgeon", YearsOfExperience = 18, ConsultationFee = 1500, IsActive = false },
                    new Doctor { DoctorId = 6, DoctorName = "Sneha Reddy", Specialisation = "Pediatrician", YearsOfExperience = 6, ConsultationFee = 500, IsActive = true },
                    new Doctor { DoctorId = 7, DoctorName = "Amit Sharma", Specialisation = "Psychiatrist", YearsOfExperience = 9, ConsultationFee = 900, IsActive = true },
                    new Doctor { DoctorId = 8, DoctorName = "Priya Iyer", Specialisation = "Endocrinologist", YearsOfExperience = 11, ConsultationFee = 1100, IsActive = true },
                    new Doctor { DoctorId = 9, DoctorName = "Karthik Das", Specialisation = "Oncologist", YearsOfExperience = 14, ConsultationFee = 1300, IsActive = false },
                    new Doctor { DoctorId = 10, DoctorName = "Rohit Verma", Specialisation = "GeneralPractitioner", YearsOfExperience = 5, ConsultationFee = 300, IsActive = true }
                );


            modelBuilder.Entity<Patient>()
                .HasData(
                    new Patient { PatientId = 1, PatientName = "Anu Nair", DateOfBirth = new DateTime(1998, 5, 12), Gender = "Female", Email = "anu.nair@example.com", PhoneNumber = "9876543210", InsuranceID = "INS1001" },
                    new Patient { PatientId = 2, PatientName = "Rahul Menon", DateOfBirth = new DateTime(1992, 9, 23), Gender = "Male", Email = "rahul.menon@example.com", PhoneNumber = "9876543211", InsuranceID = "INS1002" },
                    new Patient { PatientId = 3, PatientName = "Meera Pillai", DateOfBirth = new DateTime(2000, 2, 14), Gender = "Female", Email = "meera.pillai@example.com", PhoneNumber = "9876543212", InsuranceID = "INS1003" },
                    new Patient { PatientId = 4, PatientName = "Arjun Kumar", DateOfBirth = new DateTime(1985, 11, 30), Gender = "Male", Email = "arjun.kumar@example.com", PhoneNumber = "9876543213", InsuranceID = "INS1004" },
                    new Patient { PatientId = 5, PatientName = "Sneha Reddy", DateOfBirth = new DateTime(1995, 7, 19), Gender = "Female", Email = "sneha.reddy@example.com", PhoneNumber = "9876543214", InsuranceID = "INS1005" },
                    new Patient { PatientId = 6, PatientName = "Vivek Sharma", DateOfBirth = new DateTime(1988, 3, 5), Gender = "Male", Email = "vivek.sharma@example.com", PhoneNumber = "9876543215", InsuranceID = "INS1006" },
                    new Patient { PatientId = 7, PatientName = "Priya Iyer", DateOfBirth = new DateTime(1999, 12, 1), Gender = "Female", Email = "priya.iyer@example.com", PhoneNumber = "9876543216", InsuranceID = "INS1007" },
                    new Patient { PatientId = 8, PatientName = "Kiran Das", DateOfBirth = new DateTime(1993, 6, 25), Gender = "Male", Email = "kiran.das@example.com", PhoneNumber = "9876543217", InsuranceID = "INS1008" },
                    new Patient { PatientId = 9, PatientName = "Alex Joseph", DateOfBirth = new DateTime(1990, 8, 10), Gender = "Other", Email = "alex.joseph@example.com", PhoneNumber = "9876543218", InsuranceID = "INS1009" },
                    new Patient { PatientId = 10, PatientName = "Sam Thomas", DateOfBirth = new DateTime(1997, 4, 3), Gender = "Transgender", Email = "sam.thomas@example.com", PhoneNumber = "9876543219", InsuranceID = "INS1010" }
                );
        }
    }
}