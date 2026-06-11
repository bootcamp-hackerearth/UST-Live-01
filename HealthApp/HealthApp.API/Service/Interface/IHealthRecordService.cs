using HealthApp.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthApp.API.Service.Interface
{
    public interface IHealthRecordService
    {
        Task AddRecord(HealthRecordDto dto);

        Task<List<HealthRecordDto>> GetAllRecords();

        Task<List<HealthRecordDto>> GetPatientRecords(int patientId);

        Task<List<HealthRecordDto>> GetHealthRecordsByDoctor(int doctorId, int patientId);
    }
}