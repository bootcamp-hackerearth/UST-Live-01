using AutoMapper;
using HealthAxis.API.DTOs.HealthRecords;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories;

namespace HealthAxis.API.Services
{
    public class HealthRecordService
        : Service<HealthRecord, HealthRecordReadDto, HealthRecordCreateDto, HealthRecordUpdateDto>,
          IHealthRecordService
    {
        private readonly IHealthRecordRepository _healthRecordRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public HealthRecordService(
            IHealthRecordRepository healthRecordRepository,
            IDoctorRepository doctorRepository,
            IPatientRepository patientRepository,
            IMapper mapper)
            : base(healthRecordRepository, mapper)
        {
            _healthRecordRepository = healthRecordRepository;
            _doctorRepository = doctorRepository;
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<List<HealthRecordReadDto>> GetByPatientIdAsync(
            int patientId,
            CancellationToken ct = default)
        {
            List<HealthRecord> healthRecords =
                await _healthRecordRepository.GetAllAsync(ct);

            List<Doctor> doctors =
                await _doctorRepository.GetAllAsync(ct);

            Patient? patient =
                await _patientRepository.GetByIdAsync(
                    patientId,
                    ct);

            List<HealthRecordReadDto> result =
                healthRecords
                    .Where(record =>
                        record.PatientId == patientId)
                    .OrderByDescending(record =>
                        record.VisitDate)
                    .Select(record =>
                    {
                        Doctor? doctor =
                            doctors.FirstOrDefault(doctor =>
                                doctor.DoctorId == record.DoctorId);

                        return new HealthRecordReadDto
                        {
                            HealthRecordId = record.RecordId,

                            PatientId = record.PatientId,
                            PatientName = patient?.FullName ?? "Unknown Patient",

                            DoctorId = record.DoctorId,
                            DoctorName = doctor?.FullName ?? "Unknown Doctor",
                            Specialisation = doctor?.Specialisation ?? default,

                            AppointmentId = record.AppointmentId,
                            VisitDate = record.VisitDate,
                            Diagnosis = record.Diagnosis,
                            Prescription = record.Prescription,
                            Notes = record.Notes
                        };
                    })
                    .ToList();

            return result;
        }
    }
}
