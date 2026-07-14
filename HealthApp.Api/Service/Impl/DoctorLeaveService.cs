using AutoMapper;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Model;
using HealthApp.Api.Repository.Interface;
using HealthApp.Api.Service.Interface;
using HealthApp.Shared.Dto;

namespace HealthApp.Api.Service.Impl
{
    public class DoctorLeaveService : IDoctorLeaveService
    {
        private readonly IDoctorLeaveRepository _leaveRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DoctorLeaveService> _logger;

        public DoctorLeaveService(
            IDoctorLeaveRepository leaveRepository,
            IDoctorRepository doctorRepository,
            IMapper mapper,
            ILogger<DoctorLeaveService> logger)
        {
            _leaveRepository = leaveRepository;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DoctorLeaveDto> CreateMyLeaveAsync(
            DoctorLeaveCreateDto dto,
            string identityUserId)
        {
            ValidateLeaveDto(dto);

            var doctor = await _doctorRepository
                .GetByIdentityUserIdAsync(identityUserId);

            if (doctor == null)
                throw new EntityNotFoundException("Doctor", 0);

            var startDate = dto.StartDate!.Value.Date;
            var endDate = dto.EndDate!.Value.Date;

            var isOverlap = await _leaveRepository
                .HasOverlappingLeaveAsync(
                    doctor.DoctorId,
                    startDate,
                    endDate);

            if (isOverlap)
                throw new BusinessRuleException(
                    "Leave overlaps with an existing leave range.");

            var leave = new DoctorLeave
            {
                DoctorId = doctor.DoctorId,
                StartDate = startDate,
                EndDate = endDate,
                Reason = dto.Reason.Trim(),
                CreatedDate = DateTime.UtcNow
            };

            var saved = await _leaveRepository.AddAsync(leave);

            _logger.LogInformation(
                "Doctor leave created. DoctorId: {DoctorId}, Start: {StartDate}, End: {EndDate}",
                doctor.DoctorId,
                startDate,
                endDate);

            return MapLeave(saved, doctor.FullName);
        }

        public async Task<List<DoctorLeaveDto>> GetMyLeavesAsync(
            string identityUserId)
        {
            var doctor = await _doctorRepository
                .GetByIdentityUserIdAsync(identityUserId);

            if (doctor == null)
                throw new EntityNotFoundException("Doctor", 0);

            var leaves = await _leaveRepository
                .GetByDoctorIdAsync(doctor.DoctorId);

            return leaves
                .Select(x => MapLeave(x, doctor.FullName))
                .ToList();
        }

        public async Task<List<DoctorLeaveDto>> GetLeavesByDoctorIdAsync(
            int doctorId)
        {
            if (doctorId <= 0)
                throw new BusinessRuleException("Invalid doctor id.");

            var doctor = await _doctorRepository.getbyidAsync(doctorId);

            if (doctor == null)
                throw new EntityNotFoundException("Doctor", doctorId);

            var leaves = await _leaveRepository
                .GetByDoctorIdAsync(doctorId);

            return leaves
                .Select(x => MapLeave(x, doctor.FullName))
                .ToList();
        }

        public async Task<bool> IsDoctorOnLeaveAsync(int doctorId,
            DateTime date)
        {
            if (doctorId <= 0)
                throw new BusinessRuleException("Invalid doctor id.");

            return await _leaveRepository
                .IsDoctorOnLeaveAsync(doctorId, date.Date);
        }

        private static void ValidateLeaveDto(DoctorLeaveCreateDto dto)
        {
            if (dto == null)
                throw new BusinessRuleException("Leave data is required.");

            if (!dto.StartDate.HasValue)
                throw new BusinessRuleException("Start date is required.");

            if (!dto.EndDate.HasValue)
                throw new BusinessRuleException("End date is required.");

            if (dto.StartDate.Value.Date < DateTime.Today)
                throw new BusinessRuleException("Leave start date cannot be in the past.");

            if (dto.EndDate.Value.Date < dto.StartDate.Value.Date)
                throw new BusinessRuleException("Leave end date cannot be before start date.");

            if (string.IsNullOrWhiteSpace(dto.Reason))
                throw new BusinessRuleException("Reason is required.");
        }

        private static DoctorLeaveDto MapLeave(
            DoctorLeave leave,
            string doctorName)
        {
            return new DoctorLeaveDto
            {
                DoctorLeaveId = leave.DoctorLeaveId,
                DoctorId = leave.DoctorId,
                DoctorName = doctorName,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                Reason = leave.Reason,
                CreatedDate = leave.CreatedDate
            };
        }

        public async Task<(List<DoctorLeaveDto> Items, int TotalCount)> GetAllLeaveDoctorAsync(
     int pageNumber,
     int pageSize)
        {
            if (pageNumber <= 0)
                throw new BusinessRuleException("Invalid page number");

            if (pageSize <= 0)
                throw new BusinessRuleException("Invalid page size");

            var (items, totalCount) = await _leaveRepository.GetAllLeaveDoctorAsync(
                pageNumber,
                pageSize);

            var result = items.Select(l => new DoctorLeaveDto
            {
                DoctorLeaveId = l.DoctorLeaveId,
                DoctorId = l.DoctorId,
                DoctorName = l.Doctor?.FullName ?? string.Empty,
                StartDate = l.StartDate,
                EndDate = l.EndDate,
                Reason = l.Reason,
                CreatedDate = l.CreatedDate
            }).ToList();

            return (result, totalCount);
        }

    }
}