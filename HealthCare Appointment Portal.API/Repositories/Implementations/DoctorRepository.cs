using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Repositories
{
    public class DoctorRepository
        : Repository<Doctor>,
          IDoctorRepository
    {
        public DoctorRepository(
            ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Doctor>>
            GetDoctorsBySpecialisationAsync(
                Specialisation specialisation)
        {
            return await _dbSet
                .Where(d =>
                    d.Specialisation ==
                    specialisation)
                .ToListAsync();
        }
    }
}