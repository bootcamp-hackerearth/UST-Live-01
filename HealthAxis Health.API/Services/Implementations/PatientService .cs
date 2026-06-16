using AutoMapper;
using HealthAxisHealth.Shared.DTOs.CommonDtos;
using HealthAxisHealth.Shared.DTOs.HealthRecordDtos;
using HealthAxisHealth.Shared.DTOs.PatientDtos;
using HealthAxisHealth.API.Exceptions;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Services.Interfaces;
using HealthAxisHealth.API.UnitOfWork;

namespace HealthAxisHealth.API.Services.Implementations
{
    public class PatientService : IPatientService
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public PatientService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        public async Task<PatientDto?> GetByIdAsync(
            int patientId)
        {
            Patient? patient =
                await _unitOfWork.Patients
                    .GetByIdAsync(patientId);

            if (patient == null)
            {
                throw new NotFoundException(
                    "Patient not found.");
            }

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto?> GetByUserIdAsync(
            int userId)
        {
            Patient? patient =
                await _unitOfWork.Patients
                    .GetByUserIdAsync(userId);

            if (patient == null)
            {
                throw new NotFoundException(
                    "Patient not found.");
            }

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<PagedResultDto<PatientDto>> GetPagedAsync(
            PaginationParams pagination)
        {
            PagedResultDto<Patient> result =
                await _unitOfWork.Patients
                    .GetPagedAsync(pagination);

            return new PagedResultDto<PatientDto>
            {
                Items = _mapper.Map<List<PatientDto>>(result.Items),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalRecords = result.TotalRecords
            };
        }

        public async Task UpdateAsync(
            int patientId,
            UpdatePatientDto updatePatientDto)
        {
            Patient? patient =
                await _unitOfWork.Patients
                    .GetByIdAsync(patientId);

            if (patient == null)
            {
                throw new NotFoundException(
                    "Patient not found.");
            }

            await ValidateAndUpdateUserEmailAsync(
                patient,
                updatePatientDto.Email);

            _mapper.Map(updatePatientDto, patient);

            await _unitOfWork.Patients.UpdateAsync(patient);

            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateByUserIdAsync(
            int userId,
            UpdatePatientDto updatePatientDto)
        {
            Patient? patient =
                await _unitOfWork.Patients
                    .GetByUserIdAsync(userId);

            if (patient == null)
            {
                throw new NotFoundException(
                    "Patient not found.");
            }

            await ValidateAndUpdateUserEmailAsync(
                patient,
                updatePatientDto.Email);

            _mapper.Map(updatePatientDto, patient);

            await _unitOfWork.Patients.UpdateAsync(patient);

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<HealthRecordDto>>
            GetHealthRecordsAsync(
                int patientId)
        {
            Patient? patient =
                await _unitOfWork.Patients
                    .GetPatientWithHealthRecordsAsync(
                        patientId);

            if (patient == null)
            {
                throw new NotFoundException(
                    "Patient not found.");
            }

            return _mapper.Map<List<HealthRecordDto>>(
                patient.HealthRecords);
        }

        public async Task<IEnumerable<HealthRecordDto>>
            GetHealthRecordsByUserIdAsync(
                int userId)
        {
            Patient? patient =
                await _unitOfWork.Patients
                    .GetByUserIdAsync(userId);

            if (patient == null)
            {
                throw new NotFoundException(
                    "Patient not found.");
            }

            patient =
                await _unitOfWork.Patients
                    .GetPatientWithHealthRecordsAsync(
                        patient.PatientId);

            if (patient == null)
            {
                throw new NotFoundException(
                    "Patient not found.");
            }

            return _mapper.Map<List<HealthRecordDto>>(
                patient.HealthRecords);
        }

        #endregion

        #region Private Methods

        private async Task ValidateAndUpdateUserEmailAsync(
            Patient patient,
            string email)
        {
            User? existingUser =
                await _unitOfWork.Users
                    .GetByEmailAsync(email);

            if (existingUser != null &&
                existingUser.UserId != patient.UserId)
            {
                throw new BadRequestException(
                    "Email is already registered.");
            }

            User? user =
                await _unitOfWork.Users
                    .GetByIdAsync(patient.UserId);

            if (user == null)
            {
                throw new NotFoundException(
                    "User not found.");
            }

            user.Email = email;

            await _unitOfWork.Users.UpdateAsync(user);
        }

        #endregion
    }
}
