using AutoMapper;
using HealthAxisHealth.Shared.DTOs.CommonDtos;
using HealthAxisHealth.Shared.DTOs.HealthRecordDtos;
using HealthAxisHealth.Shared.Enums;
using HealthAxisHealth.API.Exceptions;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Services.Interfaces;
using HealthAxisHealth.API.UnitOfWork;

namespace HealthAxisHealth.API.Services.Implementations
{
    public class HealthRecordService : IHealthRecordService
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public HealthRecordService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public async Task<PagedResultDto<HealthRecordDto>>
            GetPagedAsync(
                PaginationParams pagination)
        {
            PagedResultDto<HealthRecord> result =
                await _unitOfWork.HealthRecords
                    .GetPagedAsync(pagination);

            return new PagedResultDto<HealthRecordDto>
            {
                Items =
                    _mapper.Map<List<HealthRecordDto>>(
                        result.Items),

                PageNumber =
                    result.PageNumber,

                PageSize =
                    result.PageSize,

                TotalRecords =
                    result.TotalRecords
            };
        }

        public async Task<HealthRecordDto?>
            GetByIdAsync(
                int recordId)
        {
            HealthRecord? record =
                await _unitOfWork.HealthRecords
                    .GetHealthRecordWithDetailsAsync(
                        recordId);

            if (record == null)
            {
                throw new NotFoundException(
                    "Health record not found.");
            }

            return _mapper.Map<HealthRecordDto>(
                record);
        }

        public async Task<IEnumerable<HealthRecordDto>>
            GetByPatientIdAsync(
                int patientId)
        {
            IEnumerable<HealthRecord> records =
                await _unitOfWork.HealthRecords
                    .GetByPatientIdAsync(patientId);

            return _mapper.Map<List<HealthRecordDto>>(
                records);
        }

        public async Task<IEnumerable<HealthRecordDto>>
            GetByDoctorIdAsync(
                int doctorId)
        {
            IEnumerable<HealthRecord> records =
                await _unitOfWork.HealthRecords
                    .GetByDoctorIdAsync(doctorId);

            return _mapper.Map<List<HealthRecordDto>>(
                records);
        }

        public async Task<int>
            CreateAsync(
                CreateHealthRecordDto dto)
        {
            Appointment? appointment =
                await _unitOfWork.Appointments
                    .GetByIdAsync(
                        dto.AppointmentId);

            if (appointment == null)
            {
                throw new NotFoundException(
                    "Appointment not found.");
            }

            HealthRecord? existingRecord =
                await _unitOfWork.HealthRecords
                    .GetByAppointmentIdAsync(
                        dto.AppointmentId);

            if (existingRecord != null)
            {
                throw new BadRequestException(
                    "A health record already exists for this appointment.");
            }

            if (appointment.Status ==
                AppointmentStatus.Cancelled)
            {
                throw new BadRequestException(
                    "Cannot create a health record for a cancelled appointment.");
            }

            if (appointment.Status !=
                AppointmentStatus.Confirmed)
            {
                throw new BadRequestException(
                    "Health records can only be created for confirmed appointments.");
            }

            if (appointment.ScheduledDate.Date >
                DateTime.Today)
            {
                throw new BadRequestException(
                    "Health records can only be created on or after the appointment date.");
            }

            HealthRecord healthRecord =
                _mapper.Map<HealthRecord>(
                    dto);

            healthRecord.PatientId =
                appointment.PatientId;

            healthRecord.DoctorId =
                appointment.DoctorId;

            healthRecord.VisitDate =
                DateTime.UtcNow;

            await _unitOfWork.HealthRecords
                .AddAsync(
                    healthRecord);

            appointment.Complete();

            await _unitOfWork.Appointments
                .UpdateAsync(
                    appointment);

            await _unitOfWork.CommitAsync();

            return healthRecord.RecordId;
        }

        #endregion
    }
}
