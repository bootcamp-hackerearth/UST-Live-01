using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.InsuranceDtos;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Services
{
    public class InsuranceService
        : IInsuranceService
    {
        private readonly IUnitOfWork
            _unitOfWork;

        private readonly IMapper
            _mapper;

        public InsuranceService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork =
                unitOfWork;

            _mapper =
                mapper;
        }

        public async Task<
            IEnumerable<InsuranceDto>>
            GetAllInsurancesAsync()
        {
            var insurances =
                await _unitOfWork
                    .Insurances
                    .GetAllAsync();

            return _mapper.Map<
                IEnumerable<InsuranceDto>>(
                    insurances);
        }

        public async Task<InsuranceDto>
            GetInsuranceByIdAsync(
                int insuranceId)
        {
            var insurance =
                await _unitOfWork
                    .Insurances
                    .GetByIdAsync(
                        insuranceId);

            if (insurance == null)
            {
                throw new InsuranceNotFoundException();
            }

            return _mapper.Map<
                InsuranceDto>(
                    insurance);
        }

        public async Task<
            IEnumerable<InsuranceDto>>
            GetInsurancesByPatientAsync(
                int patientId)
        {
            var insurances =
                await _unitOfWork
                    .Insurances
                    .GetInsurancesByPatientAsync(
                        patientId);

            return _mapper.Map<
                IEnumerable<InsuranceDto>>(
                    insurances);
        }

        public async Task<
            IEnumerable<InsuranceDto>>
            GetInsurancesByStatusAsync(
                Enums.InsuranceStatus status)
        {
            var insurances =
                await _unitOfWork
                    .Insurances
                    .GetInsurancesByStatusAsync(
                        status);

            return _mapper.Map<
                IEnumerable<InsuranceDto>>(
                    insurances);
        }

        public async Task<
            IEnumerable<InsuranceDto>>
            GetExpiredInsurancesAsync()
        {
            var insurances =
                await _unitOfWork
                    .Insurances
                    .GetExpiredInsurancesAsync();

            return _mapper.Map<
                IEnumerable<InsuranceDto>>(
                    insurances);
        }

        public async Task<
            IEnumerable<InsuranceDto>>
            GetActiveInsurancesAsync()
        {
            var insurances =
                await _unitOfWork
                    .Insurances
                    .GetActiveInsurancesAsync();

            return _mapper.Map<
                IEnumerable<InsuranceDto>>(
                    insurances);
        }

        public async Task<int>
            AddInsuranceAsync(
                CreateInsuranceDto insuranceDto)
        {
            var existingInsurance =
                await _unitOfWork
                    .Insurances
                    .GetInsuranceByPolicyNumberAsync(
                        insuranceDto.PolicyNumber);

            if (existingInsurance != null)
            {
                throw new DuplicatePolicyNumberException();
            }

            var insurance =
                _mapper.Map<Insurance>(
                    insuranceDto);

            await _unitOfWork
                .Insurances
                .AddAsync(
                    insurance);

            await _unitOfWork
                .CommitAsync();

            return insurance
                .InsuranceId;
        }

        public async Task
            UpdateInsuranceAsync(
                int insuranceId,
                UpdateInsuranceDto insuranceDto)
        {
            var insurance =
                await _unitOfWork
                    .Insurances
                    .GetByIdAsync(
                        insuranceId);

            if (insurance == null)
            {
                throw new InsuranceNotFoundException();
            }

            if (insurance.ExpiryDate.Date <
                    DateTime.Today)
            {
                throw new
                    InsuranceExpiredException();
            }

            _mapper.Map(
                insuranceDto,
                insurance);

            await _unitOfWork
                .Insurances
                .UpdateAsync(
                    insurance);

            await _unitOfWork
                .CommitAsync();
        }

        public async Task
            DeleteInsuranceAsync(
                int insuranceId)
        {
            var insurance =
                await _unitOfWork
                    .Insurances
                    .GetByIdAsync(
                        insuranceId);

            if (insurance == null)
            {
                throw new InsuranceNotFoundException();
            }

            await _unitOfWork
                .Insurances
                .DeleteAsync(
                    insuranceId);

            await _unitOfWork
                .CommitAsync();
        }
    }
}