using AutoMapper;
using HealthCareApp.Dtos;
using HealthCareApp.Enums;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using SharedClasses.Dtos;

namespace HealthCareApp.Services
{
    public class HealthRecordService(
        IHealthRecordRepository healthRecordRepository,
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository,
        IAppointmentRepository appointmentRepository,
        IMapper mapper) : IHealthRecordService
    {
        public async Task<List<HealthRecordDto>> GetAllHealthRecordsAsync()
        {
            var healthRecords = await healthRecordRepository.GetAllAsync();

            return mapper.Map<List<HealthRecordDto>>(healthRecords);
        }

        public async Task<HealthRecordDto> GetHealthRecordByIdAsync(int healthRecordId)
        {
            var healthRecord = await healthRecordRepository.GetByIdAsync(healthRecordId);

            if (healthRecord is null)
            {
                throw new Exception("Health record not found.");
            }

            return mapper.Map<HealthRecordDto>(healthRecord);
        }

        public async Task<List<HealthRecordDto>> GetHealthRecordsByPatientIdAsync(int patientId)
        {
            var patient = await patientRepository.GetByIdAsync(patientId);

            if (patient is null)
            {
                throw new Exception("Patient not found.");
            }

            var healthRecords = await healthRecordRepository.GetByPatientIdAsync(patientId);

            return mapper.Map<List<HealthRecordDto>>(healthRecords);
        }

        public async Task<List<HealthRecordDto>> GetHealthRecordsByDoctorIdAsync(int doctorId)
        {
            var doctor = await doctorRepository.GetByIdAsync(doctorId);

            if (doctor is null)
            {
                throw new Exception("Doctor not found.");
            }

            var healthRecords = await healthRecordRepository.GetByDoctorIdAsync(doctorId);

            return mapper.Map<List<HealthRecordDto>>(healthRecords);
        }

        public async Task<List<HealthRecordDto>> GetHealthRecordsByAppointmentIdAsync(int appointmentId)
        {
            var appointment = await appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment is null)
            {
                throw new Exception("Appointment not found.");
            }

            var healthRecords = await healthRecordRepository.GetByAppointmentIdAsync(appointmentId);

            return mapper.Map<List<HealthRecordDto>>(healthRecords);
        }

        public async Task<HealthRecordDto> AddHealthRecordAsync(AddHealthRecordDto dto)
        {
            var patient = await patientRepository.GetByIdAsync(dto.PatientId);

            if (patient is null)
            {
                throw new Exception("Patient not found.");
            }

            var appointment = await appointmentRepository.GetByIdAsync(dto.AppointmentId);

            if (appointment is null)
            {
                throw new Exception("Appointment not found.");
            }

            if (appointment.PatientId != dto.PatientId)
            {
                throw new Exception("Appointment does not belong to the selected patient.");
            }

            if (dto.DoctorId is not null)
            {
                var doctor = await doctorRepository.GetByIdAsync(dto.DoctorId.Value);

                if (doctor is null)
                {
                    throw new Exception("Doctor not found.");
                }

                if (appointment.DoctorId != dto.DoctorId.Value)
                {
                    throw new Exception("Appointment does not belong to the selected doctor.");
                }
            }

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new Exception("Health record cannot be added for a cancelled appointment.");
            }

            if (appointment.Status == AppointmentStatus.Pending)
            {
                throw new Exception("Health record cannot be added for a pending appointment.");
            }

            var healthRecordExists = await healthRecordRepository.ExistsByAppointmentIdAsync(dto.AppointmentId);

            if (healthRecordExists)
            {
                throw new Exception("Health record already exists for this appointment.");
            }

            if (dto.VisitDate.Date > DateTime.Today)
            {
                throw new Exception("Visit date cannot be a future date.");
            }

            var healthRecord = mapper.Map<HealthRecord>(dto);

            healthRecord.CreatedDate = DateTime.Now;

            var savedHealthRecord = await healthRecordRepository.CreateAsync(healthRecord);

            appointment.Status = AppointmentStatus.Completed;

            await appointmentRepository.UpdateAsync(appointment.AppointmentId, appointment);

            return mapper.Map<HealthRecordDto>(savedHealthRecord);
        }

        public async Task<HealthRecordDto> UpdateHealthRecordAsync(int healthRecordId, UpdateHealthRecordDto dto)
        {
            var existingHealthRecord = await healthRecordRepository.GetByIdAsync(healthRecordId);

            if (existingHealthRecord is null)
            {
                throw new Exception("Health record not found.");
            }

            if (dto.VisitDate.Date > DateTime.Today)
            {
                throw new Exception("Visit date cannot be a future date.");
            }

            mapper.Map(dto, existingHealthRecord);

            existingHealthRecord.HealthRecordId = healthRecordId;

            var updatedHealthRecord = await healthRecordRepository.UpdateAsync(healthRecordId, existingHealthRecord);

            if (updatedHealthRecord is null)
            {
                throw new Exception("Health record not found.");
            }

            return mapper.Map<HealthRecordDto>(updatedHealthRecord);
        }

        public async Task<HealthRecordDto> DeleteHealthRecordAsync(int healthRecordId)
        {
            var deletedHealthRecord = await healthRecordRepository.DeleteAsync(healthRecordId);

            if (deletedHealthRecord is null)
            {
                throw new Exception("Health record not found.");
            }

            return mapper.Map<HealthRecordDto>(deletedHealthRecord);
        }
    }
}