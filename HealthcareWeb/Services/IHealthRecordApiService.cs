using SharedClasses.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthcareWeb.Services
{
    public interface IHealthRecordApiService
    {
        Task<List<HealthRecordDto>> GetAllAsync();
        Task<HealthRecordDto> GetByIdAsync(int id);
        Task<List<HealthRecordDto>> GetByPatientAsync(int patientId);
        Task<List<HealthRecordDto>> GetByDoctorAsync(int doctorId);
        Task<List<HealthRecordDto>> GetByAppointmentAsync(int appointmentId);
        Task<HealthRecordDto> AddAsync(AddHealthRecordDto dto);
        Task<HealthRecordDto> UpdateAsync(int id, UpdateHealthRecordDto dto);
        Task<HealthRecordDto> DeleteAsync(int id);
    }
}