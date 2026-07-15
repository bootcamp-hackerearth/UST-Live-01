using AutoMapper;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.Shared.DTO.HealthRecordDtos;
using HealthAxis.Shared.DTO.PatientDtos;

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
            var patients =
                await patientRepository.GetAllAsync();

            return mapper.Map<List<PatientDto>>(
                patients);
        }

        public async Task<PatientDto?> GetByIdAsync(
            int id)
        {
            var patient =
                await patientRepository.GetByIdAsync(id);

            if (patient is null)
            {
                throw new NotFoundException(
                    "Patient not found");
            }

            return mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto?> GetByUserIdAsync(
            string userId)
        {
            var patients =
                await patientRepository.GetAllAsync();

            var patient =
                patients.FirstOrDefault(
                    patientItem =>
                        patientItem.UserId == userId);

            if (patient is null)
            {
                throw new NotFoundException(
                    "Patient profile not found");
            }

            return mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto?> UpdateAsync(
            int id,
            UpdatePatientDto patientDto)
        {
            ArgumentNullException.ThrowIfNull(
                patientDto);

            var existingPatient =
                await patientRepository.GetByIdAsync(id);

            if (existingPatient is null)
            {
                throw new NotFoundException(
                    "Patient not found");
            }

            var patients =
                await patientRepository.GetAllAsync();

            var emailExists =
                patients.Any(patient =>
                    patient.PatientId != id &&
                    patient.Email.Equals(
                        patientDto.Email,
                        StringComparison.OrdinalIgnoreCase));

            if (emailExists)
            {
                throw new ValidationExceptions(
                    "Email already registered");
            }

            var phoneExists =
                patients.Any(patient =>
                    patient.PatientId != id &&
                    patient.PhoneNumber ==
                    patientDto.PhoneNumber);

            if (phoneExists)
            {
                throw new ValidationExceptions(
                    "Phone number already registered");
            }

            existingPatient.FullName =
                patientDto.FullName;

            existingPatient.DateOfBirth =
                patientDto.DateOfBirth;

            existingPatient.Gender =
                patientDto.Gender;

            existingPatient.PhoneNumber =
                patientDto.PhoneNumber;

            existingPatient.Email =
                patientDto.Email;

            var updatedPatient =
                await patientRepository.UpdateAsync(
                    id,
                    existingPatient);

            return mapper.Map<PatientDto>(
                updatedPatient);
        }

        public async Task<List<HealthRecordDto>>
            GetHealthRecordsByPatientIdAsync(
                int patientid)
        {
            var patient =
                await patientRepository.GetByIdAsync(
                    patientid);

            if (patient is null)
            {
                throw new NotFoundException(
                    "Patient not found.");
            }

            var healthRecords =
                await healthRecordRepository.GetAllAsync();

            var doctors =
                await doctorRepository.GetAllAsync();

            var patientRecords =
                healthRecords
                    .Where(record =>
                        record.PatientId == patientid)
                    .OrderByDescending(record =>
                        record.VisitDate)
                    .ToList();

            return patientRecords
                .Select(record =>
                {
                    var doctor =
                        doctors.FirstOrDefault(
                            doctorItem =>
                                doctorItem.DoctorId ==
                                record.DoctorId);

                    return new HealthRecordDto
                    {
                        HealthRecordId =
                            record.HealthRecordId,

                        RecordId =
                            record.HealthRecordId,

                        AppointmentId =
                            record.AppointmentId,

                        PatientId =
                            record.PatientId,

                        PatientName =
                            patient.FullName,

                        DoctorId =
                            record.DoctorId,

                        DoctorName =
                            doctor?.FullName ??
                            "Doctor not assigned",

                        Specialisation =
                            doctor?.Specialisation
                                .ToString() ??
                            "Not assigned",

                        VisitDate =
                            record.VisitDate,

                        CreatedAt =
                            record.CreatedAt,

                        Diagnosis =
                            record.Diagnosis,

                        Prescription =
                            record.Prescription,

                        Notes =
                            record.Notes,

                        UpdatedDate =
                            record.UpdatedDate
                    };
                })
                .ToList();
        }

        public async Task<List<PatientDto>>
            GetPatientsForDoctorAsync(
                int doctorId)
        {
            var appointments =
                await appointmentRepository.GetAllAsync();

            var patientIds =
                appointments
                    .Where(appointment =>
                        appointment.DoctorId ==
                        doctorId)
                    .Select(appointment =>
                        appointment.PatientId)
                    .Distinct()
                    .ToList();

            var patients =
                await patientRepository.GetAllAsync();

            var doctorPatients =
                patients
                    .Where(patient =>
                        patientIds.Contains(
                            patient.PatientId))
                    .ToList();

            return mapper.Map<List<PatientDto>>(
                doctorPatients);
        }

        public async Task<PatientDto?>
            GetPatientForDoctorAsync(
                int doctorId,
                int patientId)
        {
            var appointments =
                await appointmentRepository.GetAllAsync();

            var hasAppointmentWithDoctor =
                appointments.Any(appointment =>
                    appointment.DoctorId ==
                    doctorId &&
                    appointment.PatientId ==
                    patientId);

            if (!hasAppointmentWithDoctor)
            {
                return null;
            }

            var patient =
                await patientRepository.GetByIdAsync(
                    patientId);

            if (patient is null)
            {
                return null;
            }

            return mapper.Map<PatientDto>(patient);
        }
    }
}