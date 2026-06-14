using HealthAxisHealth.API.DTOs.CommonDtos;
using HealthAxisHealth.API.DTOs.HealthRecordDtos;
using HealthAxisHealth.API.Helpers;

namespace HealthAxisHealth.API.Services.Interfaces
{
    public interface IHealthRecordService
    {
        #region Methods

        Task<PagedResultDto<HealthRecordDto>>
            GetPagedAsync(
                PaginationParams pagination);

        Task<HealthRecordDto?>
            GetByIdAsync(
                int recordId);

        Task<IEnumerable<HealthRecordDto>>
            GetByPatientIdAsync(
                int patientId);

        Task<IEnumerable<HealthRecordDto>>
            GetByDoctorIdAsync(
                int doctorId);

        Task<int>
            CreateAsync(
                CreateHealthRecordDto dto);

        #endregion
    }
}
