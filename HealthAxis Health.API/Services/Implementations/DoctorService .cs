using AutoMapper;
using HealthAxisHealth.API.DTOs.CommonDtos;
using HealthAxisHealth.API.DTOs.DoctorDtos;
using HealthAxisHealth.API.Enums;
using HealthAxisHealth.API.Exceptions;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Services.Interfaces;
using HealthAxisHealth.API.UnitOfWork;

namespace HealthAxisHealth.API.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public DoctorService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public async Task<PagedResultDto<DoctorDto>> GetPagedAsync(
            PaginationParams pagination)
        {
            PagedResultDto<Doctor> result =
                await _unitOfWork.Doctors.GetPagedAsync(pagination);

            return new PagedResultDto<DoctorDto>
            {
                Items = _mapper.Map<List<DoctorDto>>(result.Items),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalRecords = result.TotalRecords
            };
        }

        public async Task<DoctorDto?> GetByIdAsync(
            int doctorId)
        {
            Doctor? doctor =
                await _unitOfWork.Doctors.GetByIdAsync(doctorId);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found.");
            }

            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task<DoctorDto?> GetByUserIdAsync(
            int userId)
        {
            Doctor? doctor =
                await _unitOfWork.Doctors.GetByUserIdAsync(userId);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found.");
            }

            return _mapper.Map<DoctorDto>(doctor);
        }

        public async Task<IEnumerable<DoctorDto>> GetBySpecialisationAsync(
            string specialisation)
        {
            if (!Enum.TryParse(
                specialisation,
                true,
                out Specialisation parsedSpecialisation))
            {
                return Enumerable.Empty<DoctorDto>();
            }

            IEnumerable<Doctor> doctors =
                await _unitOfWork.Doctors
                    .GetBySpecialisationAsync(parsedSpecialisation);

            return _mapper.Map<List<DoctorDto>>(doctors);
        }

        public async Task<IEnumerable<DoctorAvailabilityDto>>
            GetAvailabilityAsync(int doctorId)
        {
            Doctor? doctor =
                await _unitOfWork.Doctors
                    .GetDoctorWithAppointmentsAsync(doctorId);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found.");
            }

            return doctor.Appointments
                .Select(a => new DoctorAvailabilityDto
                {
                    Date = a.ScheduledDate,
                    TimeSlot = a.TimeSlot
                })
                .ToList();
        }

        public async Task UpdateAsync(
            int doctorId,
            UpdateDoctorDto updateDoctorDto)
        {
            Doctor? doctor =
                await _unitOfWork.Doctors.GetByIdAsync(doctorId);

            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found.");
            }

            _mapper.Map(updateDoctorDto, doctor);

            _unitOfWork.Doctors.Update(doctor);

            await _unitOfWork.CommitAsync();
        }

        #endregion
    }
}
