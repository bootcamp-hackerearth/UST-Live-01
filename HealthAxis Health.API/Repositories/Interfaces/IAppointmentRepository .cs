using HealthAxisHealth.API.DTOs.CommonDtos;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;

namespace HealthAxisHealth.API.Repositories.Interfaces
{
    public interface IAppointmentRepository :
        IRepository<Appointment>
    {
        #region Methods

        Task<IEnumerable<Appointment>>
            GetByPatientIdAsync(
                int patientId);

        Task<IEnumerable<Appointment>>
            GetByDoctorIdAsync(
                int doctorId);

        Task<IEnumerable<Appointment>>
            GetDoctorAppointmentsByDateAsync(
                int doctorId,
                DateTime date);

        Task<Appointment?>
            GetAppointmentWithDetailsAsync(
                int appointmentId);

        Task<bool>
            IsSlotAvailableAsync(
                int doctorId,
                DateTime scheduledDate,
                string timeSlot);

        Task<PagedResultDto<Appointment>>
            GetPagedAsync(
                PaginationParams pagination);

        #endregion
    }
}
