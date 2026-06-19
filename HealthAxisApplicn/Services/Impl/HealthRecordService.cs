using AutoMapper;
using HealthAxisApplicn.Dto.HealthRecords;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Repositories;

namespace HealthAxisApplicn.Services.Impl
{
    public class HealthRecordService(IHealthRecordRepository repository, IAppointmentRepository appointmentRepository, IMapper mapper) : IHealthRecordService
    {
        public async Task<HealthRecordDto> CreateAsync(CreateHealthRecordDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Diagnosis))
                throw new Exception("Diagnosis is required");

            if (string.IsNullOrWhiteSpace(dto.Prescription))
                throw new Exception("Prescription is required");

            var appointment = await appointmentRepository.GetByIdAsync(dto.AppointmentId);

            if (appointment == null)
                throw new Exception("Invalid Appointment");

            if (appointment.Status != "Completed")
                throw new Exception("Health record can only be created after appointment is completed");

            var record = new HealthRecord
            {
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                VisitDate = dto.VisitDate,
                Diagnosis = dto.Diagnosis,
                Prescription = dto.Prescription,
                Notes = dto.Notes
            };

            var saved = await repository.CreateAsync(record);

            return mapper.Map<HealthRecordDto>(saved);
        }

        public async Task<List<HealthRecordDto>> GetAllAsync()
        {
            return mapper.Map<List<HealthRecordDto>>(await repository.GetAllAsync());
        }

        public async Task<HealthRecordDto?> GetByIdAsync(int id)
        {
            var healthRecord = await repository.GetByIdAsync(id);
            return mapper.Map<HealthRecordDto?>(healthRecord);
        }

        public async Task<List<HealthRecordDto>> GetRecordsByPatientIdAsync(int patientId)
        {
            return mapper.Map<List<HealthRecordDto>>(await repository.GetRecordsByPatientIdAsync(patientId));
        }

        public async Task<List<HealthRecordDto>> GetRecordsByDoctorIdAsync(int doctorId)
        {
            return mapper.Map<List<HealthRecordDto>>(await repository.GetRecordsByDoctorIdAsync(doctorId));
        }

        public async Task<List<HealthRecordDto>> GetRecordsByDoctorNameAsync(string doctorName)
        {
            return mapper.Map<List<HealthRecordDto>>(await repository.GetRecordsByDoctorNameAsync(doctorName));
        }

        public async Task<List<HealthRecordDto>> GetRecordsByPatientNameAsync(string patientName)
        {
            return mapper.Map<List<HealthRecordDto>>(await repository.GetRecordsByPatientNameAsync(patientName));
        }

    }
}
