using AutoMapper;
using HealthAxis.API.Exceptions;
using HealthAxis.API.Models;
using HealthAxis.API.Repositories.Interfaces;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.HealthRecord;
using HealthAxis.Shared.DTOs.Patient;

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

            var totalRecords = patients.Count;

            var pagedPatients = patients
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            var patientDtos = _mapper.Map<IEnumerable<PatientDto>>(pagedPatients);

            return new PagedResponse<PatientDto>
            {
                Items = patientDtos.ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
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
                Items = patientDtos.ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
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

        public async Task<PatientDto> AddAsync(CreatePatientDto dto)
        {
            if (dto.DateOfBirth.Date > DateTime.Today)
            {
                throw new ValidationException("Date of birth cannot be in the future.");
            }

            if (dto.DateOfBirth.Date < new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Unspecified))
            {
                throw new ValidationException("Date of birth must be after 01 Jan 1900.");
            }

            var existingPatients = await _patientRepository.GetAllAsync();

            var emailExists = existingPatients.Any(patient =>
                patient.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase));

            if (emailExists)
            {
                throw new ValidationException("Patient email already exists.");
            }

            var phoneExists = existingPatients.Any(patient =>
                patient.PhoneNumber == dto.PhoneNumber);

            if (phoneExists)
            {
                throw new ValidationException("Patient phone number already exists.");
            }

            var patient = new Patient
            {
                UserId = null,
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                InsuranceId = null,
                CreatedDate = DateTime.Today
            };

            var savedPatient = await _patientRepository.AddAsync(patient);

            return _mapper.Map<PatientDto>(savedPatient);
        }

        public async Task<PatientDto> UpdateAsync(int id, UpdatePatientDto dto)
        {
            if (dto.DateOfBirth.Date > DateTime.Today)
            {
                throw new ValidationException("Date of birth cannot be in the future.");
            }

            if (dto.DateOfBirth.Date < new DateTime(1900, 1, 1,0,0,0,DateTimeKind.Unspecified))
            {
                throw new ValidationException("Date of birth must be after 01 Jan 1900.");
            }

            var patient = await _patientRepository.GetByIdAsync(id);

            if (patient == null)
            {
                throw new NotFoundException("Patient not found");
            }

            var existingPatients = await _patientRepository.GetAllAsync();

            var emailExists = existingPatients.Any(existingPatient =>
                existingPatient.PatientId != id &&
                existingPatient.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase));

            if (emailExists)
            {
                throw new ValidationException("Patient email already exists.");
            }

            var phoneExists = existingPatients.Any(existingPatient =>
                existingPatient.PatientId != id &&
                existingPatient.PhoneNumber == dto.PhoneNumber);

            if (phoneExists)
            {
                throw new ValidationException("Patient phone number already exists.");
            }

            patient.FullName = dto.FullName;
            patient.DateOfBirth = dto.DateOfBirth;
            patient.Gender = dto.Gender;
            patient.PhoneNumber = dto.PhoneNumber;
            patient.Email = dto.Email;
            patient.InsuranceId = null;

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
        public async Task<PatientDto?> GetByUserIdAsync(string userId)
        {
            var patients = await _patientRepository.GetAllAsync();

            var patient = patients.FirstOrDefault(p => p.UserId == userId);

            if (patient == null)
            {
                throw new NotFoundException("Patient profile not found.");
            }

            return _mapper.Map<PatientDto>(patient);
        }
    }
}