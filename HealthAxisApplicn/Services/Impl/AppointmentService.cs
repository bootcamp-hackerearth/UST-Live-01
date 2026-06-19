using AutoMapper;
using HealthAxisApplicn.Dto.Appointments;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Repositories;

namespace HealthAxisApplicn.Services.Impl
{
    public class AppointmentService(IAppointmentRepository repository, IMapper mapper) : IAppointmentService
    {
        public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto entity)
        {
            var appointment = mapper.Map<Appointment>(entity);
            var savedEntity = await repository.CreateAsync(appointment);
            return mapper.Map<AppointmentDto>(savedEntity);
        }

        public async Task<bool> DeleteAppointmentAsync(int appointmentId)
        {
            var deleted = await repository.DeleteAsync(appointmentId);
            return deleted;
        }

        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            return mapper.Map<List<AppointmentDto>>(await repository.GetAllAsync());
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByDoctorIdAsync(int doctorId)
        {
            return mapper.Map<List<AppointmentDto>>(await repository.GetAppointmentsByDoctorIdAsync(doctorId));
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByPatientIdAsync(int patientId)
        {
            return mapper.Map<List<AppointmentDto>>(await repository.GetAppointmentsByPatientIdAsync(patientId));
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByDoctorNameAsync(string doctorName)
        {
            return mapper.Map<List<AppointmentDto>>(await repository.GetAppointmentsByDoctorNameAsync(doctorName));
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByPatientNameAsync(string patientName)
        {
            return mapper.Map<List<AppointmentDto>>(await repository.GetAppointmentsByPatientNameAsync(patientName));
        }

        public async Task<AppointmentDto?> GetByIdAsync(int id)
        {
            var appointment = await repository.GetByIdAsync(id);
            return mapper.Map<AppointmentDto?>(appointment);
        }

        public async Task<AppointmentDto?> UpdateAsync(int id, UpdateAppointmentStatusDto entity)
        {

            var existing = await repository.GetByIdAsync(id);

            if (existing == null)
                return null;

            if (existing.Status == "Completed")
                throw new Exception("Completed appointment cannot be modified");

            if (entity.Status == "Cancelled" && string.IsNullOrWhiteSpace(entity.CancellationReason))
                throw new Exception("Cancellation reason is required");

            existing.Status = entity.Status;
            existing.CancellationReason = entity.CancellationReason;

            var updated = await repository.UpdateAsync(id, existing);

            return mapper.Map<AppointmentDto?>(updated);
        }
    }
}
