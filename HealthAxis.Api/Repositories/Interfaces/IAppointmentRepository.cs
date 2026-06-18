using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.Dtos;

namespace HealthAxisCore_Api.Repositories.Interfaces
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<List<Appointment>> GetAppointmentsAsync(int? patientId, int? doctorId, DateTime? date, CancellationToken ct = default);
        Task<Appointment?> GetDetailsAsync(int appointmentId, CancellationToken ct = default);
        Task<List<AppointmentReportDto>> GetAppointmentReportAsync(CancellationToken ct = default);
    }
}
