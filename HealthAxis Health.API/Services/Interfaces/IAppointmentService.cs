using HealthAxisHealth.API.DTOs.AppointmentDtos;
using HealthAxisHealth.API.DTOs.CommonDtos;
using HealthAxisHealth.API.Helpers;

namespace HealthAxisHealth.API.Services.Interfaces
{
    public interface IAppointmentService
    {
        #region Methods

        Task<PagedResultDto<AppointmentDto>>
            GetPagedAsync(
                PaginationParams pagination);

        Task<IEnumerable<AppointmentDto>>
            GetByPatientIdAsync(
                int patientId);

        Task<IEnumerable<AppointmentDto>>
            GetByDoctorIdAsync(
                int doctorId);

        Task<AppointmentDto?>
            GetByIdAsync(
                int appointmentId);

        Task<IEnumerable<AppointmentDto>>
            GetDoctorAppointmentsByDateAsync(
                int doctorId,
                DateTime date);

        Task<int>
            CreateAsync(
                int userId,
                CreateAppointmentDto dto);

        Task UpdateStatusAsync(
            int appointmentId,
            UpdateAppointmentStatusDto dto);

        Task DeleteAsync(
            int appointmentId);

        #endregion
    }
}
