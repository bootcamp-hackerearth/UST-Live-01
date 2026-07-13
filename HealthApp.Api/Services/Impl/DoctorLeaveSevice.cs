using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Models;
using HealthApp.Api.Repositories.Interfaces;
using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Dtos;

namespace HealthApp.Api.Services.Impl
{
    public class DoctorLeaveService : IDoctorLeaveService
    {
        private readonly IDoctorLeaveRepository _doctorLeaveRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public DoctorLeaveService(
            IDoctorLeaveRepository doctorLeaveRepository,
            IDoctorRepository doctorRepository,
            IMapper mapper)
        {
            _doctorLeaveRepository = doctorLeaveRepository;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
        }

        public async Task<DoctorLeaveDto> CreateLeaveAsync(
            int doctorId,
            DoctorLeaveCreateDto dto,
            CancellationToken ct = default)
        {
            ValidateDoctorId(doctorId);

            if (dto == null)
            {
                throw new InvalidRequestException(
                    "Doctor leave data is required.");
            }

            ValidateLeave(dto);

            var doctor = await GetDoctorAsync(doctorId, ct);

            var hasOverlap = await _doctorLeaveRepository
                .HasOverlappingLeaveAsync(
                    doctorId,
                    dto.StartDate,
                    dto.EndDate,
                    ct);

            if (hasOverlap)
            {
                throw new BusinessRuleViolationException(
                    "The selected leave dates overlap with an existing leave record.");
            }

            var doctorLeave = _mapper.Map<DoctorLeave>(dto);

            doctorLeave.DoctorId = doctorId;
            doctorLeave.Doctor = doctor;
            doctorLeave.Reason = dto.Reason.Trim();
            doctorLeave.CreatedAtUtc = DateTime.UtcNow;

            var createdLeave = await _doctorLeaveRepository.Add(
                doctorLeave,
                ct);

            return _mapper.Map<DoctorLeaveDto>(createdLeave);
        }

        public async Task<IEnumerable<DoctorLeaveDto>> GetDoctorLeavesAsync(
            int doctorId,
            CancellationToken ct = default)
        {
            ValidateDoctorId(doctorId);

            await GetDoctorAsync(doctorId, ct);

            var leaves = await _doctorLeaveRepository.GetByDoctorIdAsync(
                doctorId,
                ct);

            return _mapper.Map<IEnumerable<DoctorLeaveDto>>(leaves);
        }

        public async Task<DoctorLeaveDto?> GetLeaveForDateAsync(
            int doctorId,
            DateOnly date,
            CancellationToken ct = default)
        {
            ValidateDoctorId(doctorId);

            await GetDoctorAsync(doctorId, ct);

            var leave = await _doctorLeaveRepository.GetLeaveForDateAsync(
                doctorId,
                date,
                ct);

            return leave == null
                ? null
                : _mapper.Map<DoctorLeaveDto>(leave);
        }

        public async Task<bool> IsDoctorOnLeaveAsync(
            int doctorId,
            DateOnly date,
            CancellationToken ct = default)
        {
            ValidateDoctorId(doctorId);

            await GetDoctorAsync(doctorId, ct);

            return await _doctorLeaveRepository.IsDoctorOnLeaveAsync(
                doctorId,
                date,
                ct);
        }

        private async Task<Doctor> GetDoctorAsync(
            int doctorId,
            CancellationToken ct)
        {
            var doctor = await _doctorRepository.GetByIdAsync(
                doctorId,
                ct);

            if (doctor == null)
            {
                throw new EntityNotFoundException("Doctor", doctorId);
            }

            return doctor;
        }

        private static void ValidateDoctorId(int doctorId)
        {
            if (doctorId <= 0)
            {
                throw new InvalidRequestException(
                    "Valid doctor id is required.");
            }
        }

        private static void ValidateLeave(DoctorLeaveCreateDto dto)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (dto.StartDate < today)
            {
                throw new BusinessRuleViolationException(
                    "Leave start date cannot be in the past.");
            }

            if (dto.EndDate < dto.StartDate)
            {
                throw new BusinessRuleViolationException(
                    "Leave end date cannot be before the start date.");
            }

            if (string.IsNullOrWhiteSpace(dto.Reason))
            {
                throw new InvalidRequestException(
                    "Leave reason is required.");
            }

            var reason = dto.Reason.Trim();

            if (reason.Length < 3)
            {
                throw new InvalidRequestException(
                    "Leave reason must be at least 3 characters long.");
            }

            if (reason.Length > 500)
            {
                throw new InvalidRequestException(
                    "Leave reason cannot exceed 500 characters.");
            }
        }
    }
}