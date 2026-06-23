using HealthAxis.API.DTOs.Admin;
using HealthAxis.API.DTOs.Appointments;
using HealthAxis.API.DTOs.CommonDtos;
using HealthAxis.API.DTOs.Doctors;
using HealthAxis.API.DTOs.HealthRecords;
using HealthAxis.API.DTOs.Patients;

namespace HealthAxis.API.Services
{
    public interface IAdminService
    {
        Task<PagedResultDto<PatientReadDto>> GetPatientsAsync(
            PaginationQueryDto pagination,
            CancellationToken ct = default);

        Task<PatientReadDto> UpdatePatientAsync(
            int patientId,
            PatientUpdateDto dto,
            CancellationToken ct = default);

        Task<PagedResultDto<DoctorReadDto>> GetDoctorsAsync(
            PaginationQueryDto pagination,
            CancellationToken ct = default);

        Task<DoctorReadDto> CreateDoctorAsync(
            AdminDoctorCreateDto dto,
            CancellationToken ct = default);

        Task<DoctorReadDto> UpdateDoctorAsync(
            int doctorId,
            DoctorUpdateDto dto,
            CancellationToken ct = default);

        Task<HealthRecordReadDto> UpdateHealthRecordAsync(
            int recordId,
            HealthRecordUpdateDto dto,
            CancellationToken ct = default);

        Task<PagedResultDto<AppointmentReportDto>> GetAppointmentReportAsync(
            PaginationQueryDto pagination,
            CancellationToken ct = default);
    }
}
