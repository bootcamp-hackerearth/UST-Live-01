using HealthAppMVC.Models;
using HealthAppWebAPI.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthAppMVC.Services.Interface
{

    public interface IHealthRecordService
    {
        Task<IEnumerable<HealthRecordDto>> GetAllAsync();
        Task<HealthRecordDto> GetByIdAsync(int id);
        Task AddHealthRecordAsync(CreateHealthRecordDto dto);

        Task<List<HealthRecordDto>> GetPatientHistoryAsync(int patientId);
    }
}