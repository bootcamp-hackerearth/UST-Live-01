using HealthApp.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthApp.Service.Interface
{
    public interface IHealthRecordApiService
    {
        Task<List<HealthRecordDto>> GetAll();
        Task<List<HealthRecordDto>> GetByPatient(int patientId);
        Task<List<HealthRecordDto>> GetByDoctorAndPatient(int doctorId, int patientId);
        Task Create(HealthRecordDto dto);
    }
}