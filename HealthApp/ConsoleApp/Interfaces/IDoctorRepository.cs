using HealthApp.ConsoleApp.Models;

namespace HealthApp.ConsoleApp.Interfaces
{
    public interface IDoctorRepository
    {
        string AddDoctor(Doctor doctor);
        Doctor? GetDoctorById(int id);
        List<Doctor> GetDoctorsBySpecialisation(string specialisation);
        Doctor UpdateDoctor(Doctor existingDoctor, Doctor doctor);
        List<Doctor> GetAllDoctors();
    }
}