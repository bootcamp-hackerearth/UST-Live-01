using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Interfaces
{
    public interface IDoctorRepository
        : IRepository<Doctor>
    {
        Task<IEnumerable<Doctor>>
            GetDoctorsBySpecialisationAsync(
                Specialisation specialisation);
    }
}