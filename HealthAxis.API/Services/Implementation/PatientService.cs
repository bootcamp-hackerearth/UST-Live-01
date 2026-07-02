using AutoMapper;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Implementations;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.Shared.DTO.CommonDtos;
using HealthAxis.Shared.DTO.HealthRecordDtos;
using HealthAxis.Shared.DTO.PatientDtos;
using HealthAxis.Shared.Utilities;

namespace HealthAxis.API.Services.Implementation
{
    public class PatientService(
     IPatientRepository patientRepository,
     IDoctorRepository doctorRepository,
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
        public async Task<List<HealthRecordDto>> GetHealthRecordsByPatientIdAsync(
    int patientId)
        {
            var patient = await patientRepository.GetByIdAsync(patientId);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found.");
            }

            var healthRecords = await healthRecordRepository.GetAllAsync();
            var doctors = await doctorRepository.GetAllAsync();

            var patientRecords = healthRecords
                .Where(record => record.PatientId == patientId)
                .OrderByDescending(record => record.VisitDate)
                .ToList();

            return patientRecords.Select(record =>
            {
                var doctor = doctors.FirstOrDefault(
                    doctorItem => doctorItem.DoctorId == record.DoctorId);

                return new HealthRecordDto
                {
                    HealthRecordId = record.RecordId,
                    RecordId = record.RecordId,
                    AppointmentId = record.AppointmentId,

                    PatientId = record.PatientId,
                    PatientName = patient.FullName,

                    DoctorId = record.DoctorId,
                    DoctorName = doctor?.FullName ?? "Doctor not assigned",
                    Specialisation = doctor?.Specialisation.ToString() ?? "Not assigned",

                    VisitDate = record.VisitDate,
                    Diagnosis = record.Diagnosis,
                    Prescription = record.Prescription,
                    Notes = record.Notes,
                    UpdatedDate = record.UpdatedDate
                };
            }).ToList();
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
    //    public async Task<PagedResponseDto<PatientDto>> GetPagedAsync(
    //PaginationQueryDto paginationQuery)
    //    {
    //        var totalRecords = await patientRepository.CountAsync();

    //        var patients = await patientRepository.GetPagedAsync(
    //            paginationQuery,
    //            patient => patient.PatientId);

    //        var patientDtos = mapper.Map<List<PatientDto>>(patients);

    //        return PagedResponseFactory.Create(
    //            patientDtos,
    //            paginationQuery,
    //            totalRecords);
    //    }
    }
}