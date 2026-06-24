using AutoMapper;
using HealthCareApp.Exceptions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Shared.Dtos.HealthRecords;
using HealthCareApp.Shared.Enums;

namespace HealthCareApp.Services
{
    public class HealthRecordService(
        IHealthRecordRepository healthRecordRepository,
        IPatientRepository patientRepository,
        IDoctorRepository doctorRepository,
        IAppointmentRepository appointmentRepository,
        IMapper mapper) : IHealthRecordService
    {
        private const string HealthRecordEntityName = "HealthRecord";
        private const string PatientEntityName = "Patient";
        private const string DoctorEntityName = "Doctor";
        private const string AppointmentEntityName = "Appointment";
        private const string HealthRecordDetailsRequiredMessage = "Health record details are required.";

        public async Task<List<HealthRecordDto>> GetAllHealthRecordsAsync()
        {
            var healthRecords = await healthRecordRepository.GetAllAsync();

            return mapper.Map<List<HealthRecordDto>>(healthRecords);
        }

        public async Task<HealthRecordDto> GetHealthRecordByIdAsync(int healthRecordId)
        {
            ValidateHealthRecordId(healthRecordId);

            var healthRecord = await healthRecordRepository.GetByIdAsync(healthRecordId);

            if (healthRecord is null)
            {
                throw new EntityNotFoundException(HealthRecordEntityName, healthRecordId);
            }

            return mapper.Map<HealthRecordDto>(healthRecord);
        }

        public async Task<List<HealthRecordDto>> GetHealthRecordsByPatientIdAsync(int patientId)
        {
            await ValidatePatientExistsAsync(patientId);

            var healthRecords = await healthRecordRepository.GetByPatientIdAsync(patientId);

            return mapper.Map<List<HealthRecordDto>>(healthRecords);
        }

        public async Task<List<HealthRecordDto>> GetHealthRecordsByDoctorIdAsync(int doctorId)
        {
            await ValidateDoctorExistsAsync(doctorId);

            var healthRecords = await healthRecordRepository.GetByDoctorIdAsync(doctorId);

            return mapper.Map<List<HealthRecordDto>>(healthRecords);
        }

        public async Task<List<HealthRecordDto>> GetHealthRecordsByAppointmentIdAsync(int appointmentId)
        {
            await GetAppointmentEntityByIdAsync(appointmentId);

            var healthRecords = await healthRecordRepository.GetByAppointmentIdAsync(appointmentId);

            return mapper.Map<List<HealthRecordDto>>(healthRecords);
        }

        public async Task<HealthRecordDto> AddHealthRecordAsync(AddHealthRecordDto dto)
        {
            if (dto is null)
            {
                throw new HealthRecordRuleException(HealthRecordDetailsRequiredMessage);
            }

            ValidatePatientId(dto.PatientId);
            ValidateAppointmentId(dto.AppointmentId);

            var appointment = await ValidateAndGetAppointmentForHealthRecordAsync(dto);

            ValidateAppointmentForHealthRecordCreation(appointment, dto.VisitDate);

            var healthRecordExists = await healthRecordRepository.ExistsByAppointmentIdAsync(dto.AppointmentId);

            if (healthRecordExists)
            {
                throw new ConflictException("Health record already exists for this appointment.");
            }

            ValidateHealthRecordText(dto.Diagnosis, dto.Prescription, dto.Notes);

            var healthRecord = mapper.Map<HealthRecord>(dto);

            healthRecord.PatientId = appointment.PatientId;
            healthRecord.DoctorId = appointment.DoctorId;
            healthRecord.AppointmentId = appointment.AppointmentId;
            healthRecord.CreatedDate = DateTime.Now;

            var savedHealthRecord = await healthRecordRepository.CreateAsync(healthRecord);

            appointment.Status = AppointmentStatus.Completed;

            await appointmentRepository.UpdateAsync(appointment.AppointmentId, appointment);

            return mapper.Map<HealthRecordDto>(savedHealthRecord);
        }

        public async Task<HealthRecordDto> UpdateHealthRecordAsync(int healthRecordId, UpdateHealthRecordDto dto)
        {
            ValidateHealthRecordId(healthRecordId);

            if (dto is null)
            {
                throw new HealthRecordRuleException(HealthRecordDetailsRequiredMessage);
            }

            var existingHealthRecord = await healthRecordRepository.GetByIdAsync(healthRecordId);

            if (existingHealthRecord is null)
            {
                throw new EntityNotFoundException(HealthRecordEntityName, healthRecordId);
            }

            var appointment = await appointmentRepository.GetByIdAsync(existingHealthRecord.AppointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, existingHealthRecord.AppointmentId);
            }

            if (appointment.ScheduledDate.Date > DateTime.Today)
            {
                throw new HealthRecordRuleException("Health record cannot be updated before the appointment date.");
            }

            if (dto.VisitDate.Date != appointment.ScheduledDate.Date)
            {
                throw new HealthRecordRuleException("Visit date must match the appointment scheduled date.");
            }

            ValidateHealthRecordText(dto.Diagnosis, dto.Prescription, dto.Notes);

            mapper.Map(dto, existingHealthRecord);

            existingHealthRecord.HealthRecordId = healthRecordId;

            var updatedHealthRecord = await healthRecordRepository.UpdateAsync(
                healthRecordId,
                existingHealthRecord);

            if (updatedHealthRecord is null)
            {
                throw new EntityNotFoundException(HealthRecordEntityName, healthRecordId);
            }

            return mapper.Map<HealthRecordDto>(updatedHealthRecord);
        }

        public async Task<HealthRecordDto> DeleteHealthRecordAsync(int healthRecordId)
        {
            ValidateHealthRecordId(healthRecordId);

            var deletedHealthRecord = await healthRecordRepository.DeleteAsync(healthRecordId);

            if (deletedHealthRecord is null)
            {
                throw new EntityNotFoundException(HealthRecordEntityName, healthRecordId);
            }

            return mapper.Map<HealthRecordDto>(deletedHealthRecord);
        }

        public async Task<List<HealthRecordDto>> GetMyHealthRecordsForPatientAsync(string identityUserId)
        {
            var patient = await GetLoggedInPatientAsync(identityUserId);

            var healthRecords = await healthRecordRepository.GetByPatientIdAsync(patient.PatientId);

            return mapper.Map<List<HealthRecordDto>>(healthRecords);
        }

        public async Task<HealthRecordDto> GetHealthRecordByIdForPatientAsync(
            int healthRecordId,
            string identityUserId)
        {
            ValidateHealthRecordId(healthRecordId);

            var patient = await GetLoggedInPatientAsync(identityUserId);

            var healthRecord = await healthRecordRepository.GetByIdAsync(healthRecordId);

            if (healthRecord is null)
            {
                throw new EntityNotFoundException(HealthRecordEntityName, healthRecordId);
            }

            if (healthRecord.PatientId != patient.PatientId)
            {
                throw new ForbiddenAccessException("Patients can access only their own health records.");
            }

            return mapper.Map<HealthRecordDto>(healthRecord);
        }

        public async Task<List<HealthRecordDto>> GetMyHealthRecordsForDoctorAsync(string identityUserId)
        {
            var doctor = await GetLoggedInDoctorAsync(identityUserId);

            var healthRecords = await healthRecordRepository.GetByDoctorIdAsync(doctor.DoctorId);

            return mapper.Map<List<HealthRecordDto>>(healthRecords);
        }

        public async Task<HealthRecordDto> GetHealthRecordByIdForDoctorAsync(
            int healthRecordId,
            string identityUserId)
        {
            ValidateHealthRecordId(healthRecordId);

            var doctor = await GetLoggedInDoctorAsync(identityUserId);

            var healthRecord = await healthRecordRepository.GetByIdAsync(healthRecordId);

            if (healthRecord is null)
            {
                throw new EntityNotFoundException(HealthRecordEntityName, healthRecordId);
            }

            if (healthRecord.DoctorId != doctor.DoctorId)
            {
                throw new ForbiddenAccessException("Doctors can access only their own health records.");
            }

            return mapper.Map<HealthRecordDto>(healthRecord);
        }

        public async Task<List<HealthRecordDto>> GetHealthRecordsByAppointmentIdForDoctorAsync(
            int appointmentId,
            string identityUserId)
        {
            ValidateAppointmentId(appointmentId);

            var doctor = await GetLoggedInDoctorAsync(identityUserId);

            var appointment = await appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, appointmentId);
            }

            if (appointment.DoctorId != doctor.DoctorId)
            {
                throw new ForbiddenAccessException("Doctors can access health records only for their own appointments.");
            }

            var healthRecords = await healthRecordRepository.GetByAppointmentIdAsync(appointmentId);

            return mapper.Map<List<HealthRecordDto>>(healthRecords);
        }

        public async Task<HealthRecordDto> AddHealthRecordForDoctorAsync(
            AddHealthRecordDto dto,
            string identityUserId)
        {
            if (dto is null)
            {
                throw new HealthRecordRuleException(HealthRecordDetailsRequiredMessage);
            }

            var doctor = await GetLoggedInDoctorAsync(identityUserId);

            ValidateAppointmentId(dto.AppointmentId);

            var appointment = await appointmentRepository.GetByIdAsync(dto.AppointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, dto.AppointmentId);
            }

            if (appointment.DoctorId != doctor.DoctorId)
            {
                throw new ForbiddenAccessException("Doctors can add health records only for their own appointments.");
            }

            if (dto.PatientId != appointment.PatientId)
            {
                throw new HealthRecordRuleException("Health record patient must match the appointment patient.");
            }

            if (dto.DoctorId is not null && dto.DoctorId.Value != doctor.DoctorId)
            {
                throw new ForbiddenAccessException("Health record doctor must match the logged-in doctor.");
            }

            dto.DoctorId = doctor.DoctorId;

            return await AddHealthRecordAsync(dto);
        }

        public async Task<HealthRecordDto> UpdateHealthRecordForDoctorAsync(
            int healthRecordId,
            UpdateHealthRecordDto dto,
            string identityUserId)
        {
            ValidateHealthRecordId(healthRecordId);

            if (dto is null)
            {
                throw new HealthRecordRuleException(HealthRecordDetailsRequiredMessage);
            }

            var doctor = await GetLoggedInDoctorAsync(identityUserId);

            var healthRecord = await healthRecordRepository.GetByIdAsync(healthRecordId);

            if (healthRecord is null)
            {
                throw new EntityNotFoundException(HealthRecordEntityName, healthRecordId);
            }

            if (healthRecord.DoctorId != doctor.DoctorId)
            {
                throw new ForbiddenAccessException("Doctors can update only their own health records.");
            }

            return await UpdateHealthRecordAsync(healthRecordId, dto);
        }

        private async Task<Appointment> ValidateAndGetAppointmentForHealthRecordAsync(AddHealthRecordDto dto)
        {
            var patient = await patientRepository.GetByIdAsync(dto.PatientId);

            if (patient is null)
            {
                throw new EntityNotFoundException(PatientEntityName, dto.PatientId);
            }

            var appointment = await appointmentRepository.GetByIdAsync(dto.AppointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, dto.AppointmentId);
            }

            if (appointment.PatientId != dto.PatientId)
            {
                throw new HealthRecordRuleException("Appointment does not belong to the selected patient.");
            }

            if (dto.DoctorId is not null)
            {
                await ValidateDoctorForHealthRecordAsync(dto.DoctorId.Value, appointment);
            }

            return appointment;
        }

        private async Task ValidateDoctorForHealthRecordAsync(int doctorId, Appointment appointment)
        {
            ValidateDoctorId(doctorId);

            var doctor = await doctorRepository.GetByIdAsync(doctorId);

            if (doctor is null)
            {
                throw new EntityNotFoundException(DoctorEntityName, doctorId);
            }

            if (appointment.DoctorId != doctorId)
            {
                throw new HealthRecordRuleException("Appointment does not belong to the selected doctor.");
            }
        }

        private static void ValidateAppointmentForHealthRecordCreation(
            Appointment appointment,
            DateTime visitDate)
        {
            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                throw new HealthRecordRuleException("Health record cannot be added for a cancelled appointment.");
            }

            if (appointment.Status == AppointmentStatus.Pending)
            {
                throw new HealthRecordRuleException("Health record cannot be added for a pending appointment.");
            }

            if (appointment.Status == AppointmentStatus.Completed)
            {
                throw new HealthRecordRuleException("Health record already exists or appointment is already completed.");
            }

            if (appointment.Status != AppointmentStatus.Confirmed)
            {
                throw new HealthRecordRuleException("Health record can be added only for confirmed appointments.");
            }

            if (appointment.ScheduledDate.Date > DateTime.Today)
            {
                throw new HealthRecordRuleException("Health record cannot be added before the appointment date.");
            }

            if (visitDate.Date != appointment.ScheduledDate.Date)
            {
                throw new HealthRecordRuleException("Visit date must match the appointment scheduled date.");
            }
        }

        private async Task<Patient> GetLoggedInPatientAsync(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                throw new BusinessRuleException("Invalid logged-in user.");
            }

            var patient = await patientRepository.GetByIdentityUserIdAsync(identityUserId);

            if (patient is null)
            {
                throw new EntityNotFoundException("Patient profile for logged-in user", 0);
            }

            return patient;
        }

        private async Task<Doctor> GetLoggedInDoctorAsync(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                throw new BusinessRuleException("Invalid logged-in user.");
            }

            var doctor = await doctorRepository.GetByIdentityUserIdAsync(identityUserId);

            if (doctor is null)
            {
                throw new EntityNotFoundException("Doctor profile for logged-in user", 0);
            }

            return doctor;
        }

        private async Task<Appointment> GetAppointmentEntityByIdAsync(int appointmentId)
        {
            ValidateAppointmentId(appointmentId);

            var appointment = await appointmentRepository.GetByIdAsync(appointmentId);

            if (appointment is null)
            {
                throw new EntityNotFoundException(AppointmentEntityName, appointmentId);
            }

            return appointment;
        }

        private async Task ValidatePatientExistsAsync(int patientId)
        {
            ValidatePatientId(patientId);

            var patient = await patientRepository.GetByIdAsync(patientId);

            if (patient is null)
            {
                throw new EntityNotFoundException(PatientEntityName, patientId);
            }
        }

        private async Task ValidateDoctorExistsAsync(int doctorId)
        {
            ValidateDoctorId(doctorId);

            var doctor = await doctorRepository.GetByIdAsync(doctorId);

            if (doctor is null)
            {
                throw new EntityNotFoundException(DoctorEntityName, doctorId);
            }
        }

        private static void ValidateHealthRecordId(int healthRecordId)
        {
            if (healthRecordId <= 0)
            {
                throw new HealthRecordRuleException("Please provide a valid health record reference.");
            }
        }

        private static void ValidatePatientId(int patientId)
        {
            if (patientId <= 0)
            {
                throw new HealthRecordRuleException("Please provide a valid patient reference.");
            }
        }

        private static void ValidateDoctorId(int doctorId)
        {
            if (doctorId <= 0)
            {
                throw new HealthRecordRuleException("Please provide a valid doctor reference.");
            }
        }

        private static void ValidateAppointmentId(int appointmentId)
        {
            if (appointmentId <= 0)
            {
                throw new HealthRecordRuleException("Please provide a valid appointment reference.");
            }
        }

        private static void ValidateHealthRecordText(
            string diagnosis,
            string prescription,
            string? notes)
        {
            if (string.IsNullOrWhiteSpace(diagnosis))
            {
                throw new HealthRecordRuleException("Diagnosis details are required.");
            }

            if (diagnosis.Trim().Length > 500)
            {
                throw new HealthRecordRuleException("Diagnosis details must not exceed 500 characters.");
            }

            if (string.IsNullOrWhiteSpace(prescription))
            {
                throw new HealthRecordRuleException("Prescription details are required.");
            }

            if (prescription.Trim().Length > 500)
            {
                throw new HealthRecordRuleException("Prescription details must not exceed 500 characters.");
            }

            if (!string.IsNullOrWhiteSpace(notes) && notes.Trim().Length > 1000)
            {
                throw new HealthRecordRuleException("Additional notes must not exceed 1000 characters.");
            }
        }
    }
}
