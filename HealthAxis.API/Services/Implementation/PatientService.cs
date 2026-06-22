using AutoMapper;
using HealthAxis.API.DTO.HealthRecordDtos;
using HealthAxis.API.DTO.PatientDtos;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;

namespace HealthAxis.API.Services.Implementation
{
    public class PatientService(
     IPatientRepository patientRepository,
     IHealthRecordRepository healthRecordRepository,
     IAppointmentRepository appointmentRepository,
     IMapper mapper) : IPatientService
    {
        public async Task<List<PatientDto>> GetAllAsync()
        {
            return mapper.Map<List<PatientDto>>(
                await patientRepository.GetAllAsync());
        }

        public async Task<PatientDto?> GetByIdAsync(int id)
        {
            var patient = await patientRepository.GetByIdAsync(id);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found");
            }

            return mapper.Map<PatientDto>(patient);
        }
        public async Task<PatientDto?> GetByUserIdAsync(string userId)
        {
            var patients = await patientRepository.GetAllAsync();

            var patient = patients.FirstOrDefault(p => p.UserId == userId);

            if (patient == null)
            {
                throw new NotFoundException("Patient profile not found");
            }

            return mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto?> UpdateAsync(int id, UpdatePatientDto patientDto)
        {
            var existingPatient = await patientRepository.GetByIdAsync(id);

            if (existingPatient == null)
            {
                throw new NotFoundException("Patient not found");
            }

            var patients = await patientRepository.GetAllAsync();

            var emailExists = patients.Any(p =>
                p.PatientId != id &&
                p.Email.Equals(patientDto.Email, StringComparison.OrdinalIgnoreCase));

            if (emailExists)
            {
                throw new ValidationException("Email already registered");
            }

            var phoneExists = patients.Any(p =>
                p.PatientId != id &&
                p.PhoneNumber == patientDto.PhoneNumber);

            if (phoneExists)
            {
                throw new ValidationException("Phone number already registered");
            }

            existingPatient.FullName = patientDto.FullName;
            existingPatient.DateOfBirth = patientDto.DateOfBirth;
            existingPatient.Gender = patientDto.Gender;
            existingPatient.PhoneNumber = patientDto.PhoneNumber;
            existingPatient.Email = patientDto.Email;

            var updated = await patientRepository.UpdateAsync(id, existingPatient);

            return mapper.Map<PatientDto>(updated);
        }
        public async Task<List<HealthRecordDto>> GetHealthRecordsByPatientIdAsync(int patientId)
        {
            var patient =await patientRepository.GetByIdAsync(patientId);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found");
            }

            var healthRecords = await healthRecordRepository.GetAllAsync();

            var patientHealthRecords =healthRecords .Where(record => record.PatientId == patientId) .ToList();

            return mapper.Map<List<HealthRecordDto>>(patientHealthRecords);
        }
        public async Task<List<PatientDto>> GetPatientsForDoctorAsync(int doctorId)
        {
            var appointments = await appointmentRepository.GetAllAsync();

            var patientIds = appointments
                .Where(a => a.DoctorId == doctorId)
                .Select(a => a.PatientId)
                .Distinct()
                .ToList();

            var patients = await patientRepository.GetAllAsync();

            var doctorPatients = patients
                .Where(p => patientIds.Contains(p.PatientId))
                .ToList();

            return mapper.Map<List<PatientDto>>(doctorPatients);
        }
        public async Task<PatientDto?> GetPatientForDoctorAsync(int doctorId, int patientId)
        {
            var appointments = await appointmentRepository.GetAllAsync();

            var hasAppointmentWithDoctor = appointments.Any(a =>
                a.DoctorId == doctorId &&
                a.PatientId == patientId);

            if (!hasAppointmentWithDoctor)
            {
                return null;
            }

            var patient = await patientRepository.GetByIdAsync(patientId);

            if (patient == null)
            {
                return null;
            }

            return mapper.Map<PatientDto>(patient);
        }
    }
}