using HealthAxis.Api.Models;
using System.Collections.Generic;

namespace HealthAxis.Api.Repositories
{
    public interface IDoctorRepository
    {
        List<Doctor> GetAll();

        Doctor GetById(int id);

        List<Doctor> GetBySpecialisation(string specialisation);

        void Add(Doctor doctor);

        void Update(Doctor doctor);

        void Save();
    }
}