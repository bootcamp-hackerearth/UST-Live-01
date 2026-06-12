using System.Collections.Generic;
using System.Threading.Tasks;
using Healthaxis2.Shared.DTOs;

namespace Healthaxis2.Web.Services.Interfaces
{
    public interface IHealthRecordService
    {
        Task<List<HealthRecordDto>> GetByPatient(int patientId);
        Task<bool> Create(HealthRecordDto dto);
    }
}