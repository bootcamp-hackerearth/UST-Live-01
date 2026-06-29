using AutoMapper;
using HealthAxisApplicn.Dto.HealthRecords;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Repositories;

namespace HealthAxisApplicn.Services.Impl
{
    public class HealthRecordService(IHealthRecordRepository repository, IAppointmentRepository appointmentRepository, IMapper mapper) : IHealthRecordService
    {
        public async Task<HealthRecordDto?> UpdateAsync(int id, UpdateHealthRecordDto dto)
        {
            var record = await repository.GetByIdAsync(id);

            if (record == null)
                return null;
            
            var appointment = await appointmentRepository.GetByIdAsync(record.AppointmentId);

            if (appointment?.Status != "Completed")
                throw new Exception("Cannot update record before appointment is completed");


            if (string.IsNullOrWhiteSpace(dto.Diagnosis))
                throw new Exception("Diagnosis is required");

            if (string.IsNullOrWhiteSpace(dto.Prescription))
                throw new Exception("Prescription is required");

            record.Diagnosis = dto.Diagnosis;
            record.Prescription = dto.Prescription;
            record.Notes = dto.Notes;

            var updated = await repository.UpdateAsync(id, record);

            return mapper.Map<HealthRecordDto>(updated);
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

        public async Task CreateFromAppointment(Appointment appointment)
        {
            var exists = await repository.ExistsForAppointmentAsync(appointment.AppointmentId);

            if (exists)
                return; 

            var record = new HealthRecord
            {
                PatientId = appointment.PatientId,
                DoctorId = appointment.DoctorId,
                AppointmentId = appointment.AppointmentId,
                VisitDate = appointment.ScheduledDate
            };

            await repository.CreateAsync(record);
        }

    }
}
