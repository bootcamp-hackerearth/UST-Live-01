using HealthAppWebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HealthAppWebApi.App_Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("HealthDb")
        {
        }

        

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<HealthRecord> HealthRecords { get; set; }
    }
}