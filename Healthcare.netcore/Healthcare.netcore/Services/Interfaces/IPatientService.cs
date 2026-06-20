using HealthAxis.API.DTOs;

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

        Task<PatientDto> UpdateAsync(int id, UpdatePatientDto dto);

        Task<IEnumerable<HealthRecordDto>> GetHealthRecordsAsync(int patientId);
    }
}