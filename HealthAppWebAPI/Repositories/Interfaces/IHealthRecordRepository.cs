using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthAppWebAPI.Repositories.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IHealthRecordRepository
    {
        Task<List<HealthRecord>> GetAllAsync();

        Task<HealthRecord> GetByIdAsync(int id);

        Task<HealthRecord> GetByAppointmentIdAsync(int appointmentId);

        Task AddAsync(HealthRecord record);
        Task<List<HealthRecord>> GetByPatientIdAsync(int patientId);
    }
}
