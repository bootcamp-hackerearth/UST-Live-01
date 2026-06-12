using System.Collections.Generic;
using Healthaxis2.Models;

namespace Healthaxis2.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        List<Doctor> GetAll();
        Doctor GetById(int id);
        void Add(Doctor doctor);
        void Update(Doctor doctor);
        void Delete(int id);
    }
}
