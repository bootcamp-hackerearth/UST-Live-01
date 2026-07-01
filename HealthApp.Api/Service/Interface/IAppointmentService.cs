using HealthApp.Shared.Dto;

namespace HealthApp.Api.Service.Interface
{
    public interface IAppointmentService
    {

        Task<object> Add(AppointmentDto dto, string identityUserId);

        Task<AppointmentDto> GetAppointmentById(int id);

        Task<(List<AppointmentDto> Items, int TotalCount)>
            GetPagedAppointments(int pageNumber, int pageSize);

        Task<(List<AppointmentDto> Items, int TotalCount)>
            GetAppointmentsByPatientAndDoctorPaged( int? patientId, int? doctorId,
                int pageNumber, int pageSize);

        Task<AppointmentDto> CancelAppointment(int appointmentId, string reason);
        Task<AppointmentDto> ConfirmAppointment(int appointmentId);
        Task<AppointmentDto> CompleteAppointment(int appointmentId);

        Task<List<string>> CheckDoctorAvailability(int doctorId, DateTime date);

        Task<bool> IsSlotBooked(int doctorId, DateTime date, string timeSlot);

        Task<List<AppointmentDto>> GetUpcomingAppointmentsByDoctor(int doctorId,
            DateTime fromDate,DateTime toDate);


        Task<List<AppointmentDto>> GetAppointmentsByUserAsync(string identityUserId);
        Task<List<AppointmentDto>> GetAppointmentsByDoctorAsync(string identityUserId);

    }
}