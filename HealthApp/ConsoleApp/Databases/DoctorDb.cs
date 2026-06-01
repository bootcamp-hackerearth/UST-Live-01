using System;
using System.Collections.Generic;
using HealthApp.ConsoleApp.Models;

namespace HealthApp.ConsoleApp.Databases
{
    public class DoctorDb
    {
        public List<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}