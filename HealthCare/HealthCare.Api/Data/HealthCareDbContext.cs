using HealthCare.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthCare.Api.Data
{
    public class HealthCareDbContext : DbContext
    {
        public HealthCareDbContext(DbContextOptions<HealthCareDbContext> options) : base(options)
        {
        }

        public DbSet<Patient>Patients {  get; set; }
        public DbSet<Doctor> Doctors {  get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
        
    }
}
