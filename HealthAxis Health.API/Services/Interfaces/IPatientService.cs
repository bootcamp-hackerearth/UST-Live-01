using HealthAxisHealth.API.DTOs.CommonDtos;
using HealthAxisHealth.API.DTOs.HealthRecordDtos;
using HealthAxisHealth.API.DTOs.PatientDtos;
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
