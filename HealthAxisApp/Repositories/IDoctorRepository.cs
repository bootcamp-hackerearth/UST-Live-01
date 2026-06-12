using HealthAxisApp.Data;
using System.Collections.Generic;

namespace HealthAxisApp.Repositories
{
    public interface IDoctorRepository
    {

        IEnumerable<Doctor> GetAll(
            string specialisation = null,
            string searchText = null,
            bool activeOnly = false);


        Doctor GetById(int id);

        Doctor Add(Doctor doctor);

        bool Update(Doctor doctor);

        bool ToggleStatus(int id);

        int GetUpcomingAppointmentCount(int doctorId);
    }
}
