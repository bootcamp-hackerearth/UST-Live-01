using AutoMapper;
using HealthAxis.API.DTOs;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;

namespace HealthAxis.API.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<Patient> _patientRepository;
        private readonly IRepository<Doctor> _doctorRepository;
        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IRepository<HealthRecord> _healthRecordRepository;
        private readonly IMapper _mapper;

        public PatientService(
            IRepository<Patient> patientRepository,
            IRepository<Doctor> doctorRepository,
            IRepository<Appointment> appointmentRepository,
            IRepository<HealthRecord> healthRecordRepository,
            IMapper mapper)
        {
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _healthRecordRepository = healthRecordRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<PatientDto>> GetPagedAsync(PaginationParams paginationParams)
        {
            var patients = await _patientRepository.GetAllAsync();

            var pageNumber = paginationParams.PageNumber <= 0 ? 1 : paginationParams.PageNumber;
            var pageSize = paginationParams.PageSize <= 0 ? 10 : paginationParams.PageSize;

            var totalRecords = patients.Count();

            var pagedPatients = patients
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var patientDtos = _mapper.Map<IEnumerable<PatientDto>>(pagedPatients);

            return new PagedResponse<PatientDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = patientDtos
            };
        }

        public async Task<PagedResponse<PatientDto>> GetDoctorPatientsAsync(
            string doctorUserId,
            PaginationParams paginationParams)
        {
            var doctors = await _doctorRepository.GetAllAsync();

            var doctor = doctors.FirstOrDefault(d => d.UserId == doctorUserId);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor profile not found");
            }

            var appointments = await _appointmentRepository.GetAllAsync();

            var patientIds = appointments
                .Where(a => a.DoctorId == doctor.DoctorId)
                .Select(a => a.PatientId)
                .Distinct()
                .ToList();

            var patients = await _patientRepository.GetAllAsync();

            var doctorPatients = patients
                .Where(p => patientIds.Contains(p.PatientId))
                .ToList();

            var pageNumber = paginationParams.PageNumber <= 0 ? 1 : paginationParams.PageNumber;
            var pageSize = paginationParams.PageSize <= 0 ? 10 : paginationParams.PageSize;

            var totalRecords = doctorPatients.Count;

            var pagedPatients = doctorPatients
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var patientDtos = _mapper.Map<IEnumerable<PatientDto>>(pagedPatients);

            return new PagedResponse<PatientDto>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = patientDtos
            };
        }

        public async Task<bool> IsPatientOwnerAsync(int patientId, string userId)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found");
            }

            return patient.UserId == userId;
        }

        public async Task<PatientDto?> GetByIdAsync(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found");
            }

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto> UpdateAsync(int id, UpdatePatientDto dto)
        {
            var patient = await _patientRepository.GetByIdAsync(id);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found");
            }

            _mapper.Map(dto, patient);

            await _patientRepository.UpdateAsync(id, patient, CancellationToken.None);

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<IEnumerable<HealthRecordDto>> GetHealthRecordsAsync(int patientId)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found");
            }

            var records = await _healthRecordRepository.GetAllAsync();

            var patientRecords = records
                .Where(r => r.PatientId == patientId);

            return _mapper.Map<IEnumerable<HealthRecordDto>>(patientRecords);
        }
    }
}