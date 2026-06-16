using HealthAxisHealth.Shared.DTOs.CommonDtos;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;

namespace HealthAxisHealth.API.Repositories.Interfaces
{
    public interface IAppointmentRepository :
        IRepository<Appointment>
    {
        #region Methods

        Task<IEnumerable<Appointment>> GetByPatientIdAsync(
            int patientId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Appointment>> GetByDoctorIdAsync(
            int doctorId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Appointment>> GetDoctorAppointmentsByDateAsync(
            int doctorId,
            DateTime date,
            CancellationToken cancellationToken = default);

        Task<Appointment?> GetAppointmentWithDetailsAsync(
            int appointmentId,
            CancellationToken cancellationToken = default);

        Task<bool> IsSlotAvailableAsync(
            int doctorId,
            DateTime scheduledDate,
            string timeSlot,
            CancellationToken cancellationToken = default);

        Task<PagedResultDto<Appointment>> GetPagedAsync(
            PaginationParams pagination,
            CancellationToken cancellationToken = default);

        #endregion
    }
}