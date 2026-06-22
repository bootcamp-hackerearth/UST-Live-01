using S3_HealthAxis.Shared.DTOs.Appointment;

namespace S3_HealthAxisApi.Services.Interface
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDto>> GetAllAsync();

        Task<AppointmentDetailsDto?> GetByIdAsync(int id);

        Task<IEnumerable<PatientAppointmentHistoryDto>> GetPatientHistoryAsync(int patientId);

        Task<IEnumerable<DoctorScheduleItemDto>> GetDoctorTodayScheduleAsync(int doctorId);

        Task<IEnumerable<DoctorScheduleItemDto>> GetDoctorWeekScheduleAsync(
            int doctorId,
            DateOnly startDate,
            DateOnly endDate);

        Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);

        Task<IEnumerable<DoctorScheduleItemDto>> GetDoctorUpcomingScheduleAsync(int doctorId);

        Task UpdateAsync(int id, UpdateAppointmentDto dto);
        Task UpdateStatusAsync(int id, UpdateAppointmentStatusDto dto);

        Task ConfirmAsync(int id);

        Task CompleteAsync(int id);

        Task CancelAsync(int id, CancelAppointmentDto dto);
    }
}