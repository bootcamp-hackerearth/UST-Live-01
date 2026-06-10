using HealthAppWebAPI.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthAppWebAPI.Services.Interfaces
{

    public interface IHealthRecordService
    {
        Task<List<HealthRecordDto>> GetAllAsync();

        Task<HealthRecordDto> GetByIdAsync(int id);

        Task AddAsync(CreateHealthRecordDto dto);

        Task<List<HealthRecordDto>> GetPatientHistoryAsync(int patientId);
    }

}
