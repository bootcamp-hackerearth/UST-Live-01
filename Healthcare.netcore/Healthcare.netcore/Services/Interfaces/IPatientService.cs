using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Patient;
using HealthAxis.Shared.DTOs.HealthRecord;

namespace HealthAxis.API.Services.Interfaces
{
    public interface IPatientService
    {
        Task<PagedResponse<PatientDto>> GetPagedAsync(PaginationParams paginationParams);

        Task<PagedResponse<PatientDto>> GetDoctorPatientsAsync(
            string doctorUserId,
            PaginationParams paginationParams);

        Task<bool> IsPatientOwnerAsync(int patientId, string userId);

        Task<PatientDto?> GetByIdAsync(int id);

        Task<PatientDto> AddAsync(CreatePatientDto dto);

        Task<PatientDto> UpdateAsync(int id, UpdatePatientDto dto);

        Task<IEnumerable<HealthRecordDto>> GetHealthRecordsAsync(int patientId);
        Task<PatientDto?> GetByUserIdAsync(string userId);

    }
}