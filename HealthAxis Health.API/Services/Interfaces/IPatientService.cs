using HealthAxisHealth.Shared.DTOs.CommonDtos;
using HealthAxisHealth.Shared.DTOs.HealthRecordDtos;
using HealthAxisHealth.Shared.DTOs.PatientDtos;
using HealthAxisHealth.API.Helpers;

namespace HealthAxisHealth.API.Services.Interfaces
{
    public interface IPatientService
    {
        #region Methods

        Task<PatientDto?> GetByIdAsync(
            int patientId);

        Task<PatientDto?> GetByUserIdAsync(
            int userId);

        Task<PagedResultDto<PatientDto>>
            GetPagedAsync(
                PaginationParams pagination);

        Task UpdateAsync(
            int patientId,
            UpdatePatientDto updatePatientDto);

        Task UpdateByUserIdAsync(
            int userId,
            UpdatePatientDto updatePatientDto);

        Task<IEnumerable<HealthRecordDto>>
            GetHealthRecordsAsync(
                int patientId);

        Task<IEnumerable<HealthRecordDto>>
            GetHealthRecordsByUserIdAsync(
                int userId);

        #endregion
    }
}
