using HealthAxis.Shared.DTOs.DoctorLeaves;
using HealthAxis.Shared.Enums;
using HealthAxisCore_Api.Data;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Repositories.Interface;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisCore_Api.Services.Implementations
{
    public class DoctorLeaveService : IDoctorLeaveService
    {
        private readonly IDoctorLeaveRepository _doctorLeaveRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly HealthAppDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly ILogger<DoctorLeaveService> _logger;

        public DoctorLeaveService(
            IDoctorLeaveRepository doctorLeaveRepository,
            IDoctorRepository doctorRepository,
            HealthAppDbContext context,
            ICacheService cacheService,
            ILogger<DoctorLeaveService> logger)
        {
            _doctorLeaveRepository = doctorLeaveRepository;
            _doctorRepository = doctorRepository;
            _context = context;
            _cacheService = cacheService;
            _logger = logger;
        }

        public async Task<DoctorLeaveDto> CreateLeaveAsync(
            int doctorId,
            CreateMyDoctorLeaveDto dto)
        {
            if (dto.StartDate.Date < DateTime.Today)
            {
                throw new BusinessRuleException(
                    "Leave start date cannot be in the past.");
            }

            if (dto.EndDate.Date < dto.StartDate.Date)
            {
                throw new BusinessRuleException(
                    "Leave end date cannot be before start date.");
            }

            var doctor =
                await _doctorRepository.GetByIdAsync(doctorId);

            if (doctor == null)
            {
                throw new EntityNotFoundException(
                    "Doctor not found.");
            }

            var hasOverlap =
                await _doctorLeaveRepository.HasOverlappingLeaveAsync(
                    doctorId,
                    dto.StartDate,
                    dto.EndDate);

            if (hasOverlap)
            {
                throw new BusinessRuleException(
                    "An overlapping leave already exists.");
            }

            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var leave = new DoctorLeave
                {
                    DoctorId = doctorId,
                    StartDate = dto.StartDate.Date,
                    EndDate = dto.EndDate.Date,
                    Reason = dto.Reason,
                    CreatedDate = DateTime.Now
                };

                await _doctorLeaveRepository.AddAsync(leave);

                doctor.IsOnLeave = true;

                var affectedAppointments =
                    await _context.Appointments
                        .Where(a =>
                            a.DoctorId == doctorId &&
                            a.ScheduledDate.Date >= dto.StartDate.Date &&
                            a.ScheduledDate.Date <= dto.EndDate.Date &&
                            (
                                a.Status == AppointmentStatus.Pending ||
                                a.Status == AppointmentStatus.Confirmed
                            ))
                        .ToListAsync();

                foreach (var appointment in affectedAppointments)
                {
                    appointment.Status =
                        AppointmentStatus.DoctorUnavailable;

                    appointment.CancellationReason =
                        "Doctor unavailable due to leave";

                    var notification = new Notification
                    {
                        PatientId = appointment.PatientId,
                        DoctorId = null,
                        AppointmentId = appointment.AppointmentId,

                        Message =
                            $"Your appointment with Dr. {doctor.DoctorName} " +
                            $"scheduled on {appointment.ScheduledDate:dd-MMM-yyyy} " +
                            $"has been cancelled because the doctor is unavailable. " +
                            $"Please book another available doctor if required.",

                        IsRead = false,

                        CreatedDate = DateTime.Now
                    };

                    await _context.Notifications.AddAsync(
                        notification);
                }

                await _doctorLeaveRepository.SaveChangesAsync();

                await transaction.CommitAsync();

                await _cacheService.RemoveAsync(
                    "available-doctors");

                _logger.LogInformation(
                    "Doctor leave created. DoctorId: {DoctorId}, StartDate: {StartDate}, EndDate: {EndDate}",
                    doctorId,
                    dto.StartDate,
                    dto.EndDate);

                return new DoctorLeaveDto
                {
                    LeaveId = leave.LeaveId,
                    DoctorId = leave.DoctorId,
                    DoctorName = doctor.DoctorName,
                    StartDate = leave.StartDate,
                    EndDate = leave.EndDate,
                    Reason = leave.Reason,
                    CreatedDate = leave.CreatedDate
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<DoctorLeaveDto>> GetDoctorLeavesAsync(
            int doctorId)
        {
            var leaves =
                await _doctorLeaveRepository.GetByDoctorIdAsync(
                    doctorId);

            return leaves
                .Select(x => new DoctorLeaveDto
                {
                    LeaveId = x.LeaveId,
                    DoctorId = x.DoctorId,
                    DoctorName = x.Doctor?.DoctorName ?? string.Empty,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Reason = x.Reason,
                    CreatedDate = x.CreatedDate
                })
                .ToList();
        }
    }
}