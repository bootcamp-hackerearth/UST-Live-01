using HealthCareApp.Exceptions;
using HealthCareApp.Models;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services.Interface;
using HealthCareApp.Shared.Dtos.DoctorLeaves;
using HealthCareApp.Shared.Dtos.Notifications;

namespace HealthCareApp.Services.Impl
{
    public class DoctorLeaveService : IDoctorLeaveService
    {
        private const string DoctorEntityName = "Doctor";

        private readonly IDoctorLeaveRepository doctorLeaveRepository;

        private readonly IDoctorRepository doctorRepository;

        private readonly ICacheService cacheService;

        private readonly ILogger<DoctorLeaveService> logger;

        public DoctorLeaveService(
            IDoctorLeaveRepository doctorLeaveRepository,
            IDoctorRepository doctorRepository,
            ICacheService cacheService,
            ILogger<DoctorLeaveService> logger)
        {
            this.doctorLeaveRepository = doctorLeaveRepository;
            this.doctorRepository = doctorRepository;
            this.cacheService = cacheService;
            this.logger = logger;
        }

        public async Task<DoctorLeaveDto> CreateMyDoctorLeaveAsync(
            CreateMyDoctorLeaveDto dto,
            string identityUserId)
        {
            if (dto is null)
            {
                throw new BusinessRuleException("Doctor leave details are required.");
            }

            var doctor = await GetLoggedInDoctorAsync(identityUserId);

            ValidateLeaveDates(
                dto.StartDate,
                dto.EndDate);

            var startDate = dto.StartDate.Date;

            var endDate = dto.EndDate.Date;

            var hasOverlappingLeave = await doctorLeaveRepository.HasOverlappingLeaveAsync(
                doctor.DoctorId,
                startDate,
                endDate);

            if (hasOverlappingLeave)
            {
                throw new ConflictException(
                    "You already have leave scheduled during the selected date range.");
            }

            var doctorLeave = new DoctorLeave
            {
                DoctorId = doctor.DoctorId,
                StartDate = startDate,
                EndDate = endDate,
                Reason = dto.Reason.Trim(),
                CreatedDate = DateTime.Now
            };

            var savedDoctorLeave = await doctorLeaveRepository.CreateAsync(doctorLeave);

            await TryRemoveAvailabilityCacheForLeaveDatesAsync(
                savedDoctorLeave.DoctorId,
                savedDoctorLeave.StartDate,
                savedDoctorLeave.EndDate);

            logger.LogInformation(
                "Doctor leave created successfully. DoctorId: {DoctorId}, StartDate: {StartDate}, EndDate: {EndDate}",
                savedDoctorLeave.DoctorId,
                savedDoctorLeave.StartDate.ToString("yyyy-MM-dd"),
                savedDoctorLeave.EndDate.ToString("yyyy-MM-dd"));

            return MapToDto(
                savedDoctorLeave,
                doctor.DoctorName);
        }

        public async Task<List<DoctorLeaveDto>> GetMyDoctorLeavesAsync(
            string identityUserId)
        {
            var doctor = await GetLoggedInDoctorAsync(identityUserId);

            var doctorLeaves = await doctorLeaveRepository.GetLeavesByDoctorIdAsync(
                doctor.DoctorId);

            return doctorLeaves
                .OrderByDescending(doctorLeave => doctorLeave.StartDate)
                .Select(doctorLeave => MapToDto(
                    doctorLeave,
                    doctor.DoctorName))
                .ToList();
        }

        public async Task<List<DoctorLeaveDto>> GetDoctorLeavesByDoctorIdAsync(
            int doctorId)
        {
            ValidateDoctorId(doctorId);

            var doctor = await doctorRepository.GetByIdAsync(doctorId);

            if (doctor is null)
            {
                throw new EntityNotFoundException(
                    DoctorEntityName,
                    doctorId);
            }

            var doctorLeaves = await doctorLeaveRepository.GetLeavesByDoctorIdAsync(
                doctorId);

            return doctorLeaves
                .OrderByDescending(doctorLeave => doctorLeave.StartDate)
                .Select(doctorLeave => MapToDto(
                    doctorLeave,
                    doctor.DoctorName))
                .ToList();
        }

        public async Task<bool> IsDoctorOnLeaveAsync(
            int doctorId,
            DateTime date)
        {
            ValidateDoctorId(doctorId);

            return await doctorLeaveRepository.IsDoctorOnLeaveAsync(
                doctorId,
                date.Date);
        }

        private async Task<Doctor> GetLoggedInDoctorAsync(string identityUserId)
        {
            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                throw new BusinessRuleException("Invalid logged-in doctor.");
            }

            var doctor = await doctorRepository.GetByIdentityUserIdAsync(identityUserId);

            if (doctor is null)
            {
                throw new EntityNotFoundException(
                    "Doctor profile for logged-in user",
                    0);
            }

            return doctor;
        }

        private async Task TryRemoveAvailabilityCacheForLeaveDatesAsync(
            int doctorId,
            DateTime startDate,
            DateTime endDate)
        {
            try
            {
                await RemoveAvailabilityCacheForLeaveDatesAsync(
                    doctorId,
                    startDate,
                    endDate);
            }
            catch (Exception ex)
            {
                logger.LogWarning(
                    ex,
                    "Doctor leave was created, but availability cache removal failed. DoctorId: {DoctorId}, StartDate: {StartDate}, EndDate: {EndDate}",
                    doctorId,
                    startDate.ToString("yyyy-MM-dd"),
                    endDate.ToString("yyyy-MM-dd"));
            }
        }

        private async Task RemoveAvailabilityCacheForLeaveDatesAsync(
            int doctorId,
            DateTime startDate,
            DateTime endDate)
        {
            var currentDate = startDate.Date;

            while (currentDate <= endDate.Date)
            {
                var cacheKey = BuildDoctorAvailabilityCacheKey(
                    doctorId,
                    currentDate);

                await cacheService.RemoveAsync(cacheKey);

                logger.LogInformation(
                    "Doctor availability cache removed due to doctor leave. CacheKey: {CacheKey}",
                    cacheKey);

                currentDate = currentDate.AddDays(1);
            }
        }

        private static string BuildDoctorAvailabilityCacheKey(
            int doctorId,
            DateTime date)
        {
            return $"doctors:{doctorId}:availability:{date:yyyy-MM-dd}";
        }

        private static DoctorLeaveDto MapToDto(
            DoctorLeave doctorLeave,
            string doctorName)
        {
            return new DoctorLeaveDto
            {
                DoctorLeaveId = doctorLeave.DoctorLeaveId,
                DoctorId = doctorLeave.DoctorId,
                DoctorName = doctorName,
                StartDate = doctorLeave.StartDate.ToString("yyyy-MM-dd"),
                EndDate = doctorLeave.EndDate.ToString("yyyy-MM-dd"),
                Reason = doctorLeave.Reason,
                CreatedDate = doctorLeave.CreatedDate.ToString("yyyy-MM-dd")
            };
        }

        private static void ValidateDoctorId(int doctorId)
        {
            if (doctorId <= 0)
            {
                throw new BusinessRuleException("Please provide a valid doctor reference.");
            }
        }

        private static void ValidateLeaveDates(
            DateTime startDate,
            DateTime endDate)
        {
            if (startDate.Date < DateTime.Today)
            {
                throw new BusinessRuleException("Leave start date cannot be in the past.");
            }

            if (endDate.Date < startDate.Date)
            {
                throw new BusinessRuleException("Leave end date cannot be before start date.");
            }
        }
    }
}