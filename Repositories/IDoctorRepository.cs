using System.Collections.Generic;
using HealthcareMvcApp.Enums;
using HealthcareMvcApp.Models;

namespace HealthcareMvcApp.Repositories
{
    public interface IDoctorRepository
    {
        void Add(Doctor doctor);

        Doctor GetById(int doctorId);

        List<Doctor> GetAll();

        List<Doctor> GetAllActive();

        List<Doctor> GetBySpecialisation(Specialisation specialisation);

        List<Doctor> GetActiveBySpecialisation(Specialisation specialisation);

        bool Update(Doctor doctor);
    }
}