using S3_HealthAxis.Shared.DTOs.Appointment;

namespace S3_HealthAxis.Blazor.Services
{
    public interface IAppointmentService
    {
        Task<List<AppointmentDetailsDto>?> GetAllAsync();
        Task<AppointmentDetailsDto?> GetByIdAsync(int id);
        Task<List<PatientAppointmentHistoryDto>?> GetPatientHistoryAsync(int patientId);
        Task<List<DoctorScheduleItemDto>?> GetDoctorTodayScheduleAsync(int doctorId);
        Task<List<DoctorScheduleItemDto>?> GetDoctorWeekScheduleAsync(int doctorId, DateOnly startDate, DateOnly endDate);
        Task<List<DoctorScheduleItemDto>?> GetDoctorUpcomingScheduleAsync(int doctorId);

        Task<AppointmentDto?> CreateAsync(CreateAppointmentDto dto);
        Task<bool> UpdateAsync(int id, UpdateAppointmentDto dto);
        Task<bool> UpdateStatusAsync(int id, UpdateAppointmentStatusDto dto);

        Task<bool> ConfirmAsync(int id);
        Task<bool> CompleteAsync(int id);
        Task<bool> CancelAsync(int id, CancelAppointmentDto dto);
    }
}