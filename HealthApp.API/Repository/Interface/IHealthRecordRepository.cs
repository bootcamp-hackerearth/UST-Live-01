using HealthApp.API.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthApp.API.Repository.Interface
{
    public interface IHealthRecordRepository
    {
        Task AddAsync(HealthRecord record);

        Task<List<HealthRecord>> GetAllAsync();
    }
}