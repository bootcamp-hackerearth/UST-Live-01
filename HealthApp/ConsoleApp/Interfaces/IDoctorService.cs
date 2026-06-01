using System;
using System.Collections.Generic;
using HealthApp.ConsoleApp.Models;

namespace HealthApp.ConsoleApp.Interfaces
{
    // Service interface for doctor-related operations
    public interface IDoctorService
    {
        string AddDoctor(Doctor doctor);
        Doctor GetDoctorById(int id);
        List<Doctor> GetDoctorsBySpecialisation(string specialisation);
        Doctor UpdateDoctor(Doctor doctor);
        List<Doctor> GetAllDoctors();
    }
}