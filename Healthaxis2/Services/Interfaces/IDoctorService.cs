using System.Collections.Generic;
using Healthaxis2.Models;

namespace Healthaxis2.Services.Interfaces
{
    public interface IDoctorService
    {
        List<Doctor> GetAll();
        Doctor GetById(int id);
        Doctor Create(Doctor doctor);
        void Update(int id, Doctor doctor);
        void Delete(int id);
    }
}