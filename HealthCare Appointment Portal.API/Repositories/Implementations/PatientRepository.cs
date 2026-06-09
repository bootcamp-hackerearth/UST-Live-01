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
        public class PatientRepository
            : Repository<Patient>,
              IPatientRepository
        {
            public PatientRepository(
                ApplicationDbContext context)
                : base(context)
            {
            }

            public async Task<Patient>
                GetPatientByEmailAsync(
                    string email)
            {
                return await _dbSet
                    .FirstOrDefaultAsync(
                        p => p.Email == email);
            }

        }
    }