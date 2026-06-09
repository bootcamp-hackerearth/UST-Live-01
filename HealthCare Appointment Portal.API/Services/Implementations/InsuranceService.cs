using AutoMapper;
using HealthCare_Appointment_Portal.Data;
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
        private readonly IInsuranceRepository
            _insuranceRepository;

        private readonly IPatientRepository
            _patientRepository;

        private readonly ApplicationDbContext
            _context;

        private readonly IMapper
            _mapper;

        public InsuranceService(
            IInsuranceRepository insuranceRepository,
            IPatientRepository patientRepository,
            ApplicationDbContext context,
            IMapper mapper)
        {
            _insuranceRepository =
                insuranceRepository;

            _patientRepository =
                patientRepository;

            _context =
                context;

            _mapper =
                mapper;
        }

        public async Task<
            IEnumerable<InsuranceDto>>
            GetAllInsurancesAsync()
        {
            var insurances =
                await _insuranceRepository
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
                await _insuranceRepository
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
            var patient =
                await _patientRepository
                    .GetByIdAsync(
                        patientId);

            if (patient == null)
            {
                throw new PatientNotFoundException();
            }

            var insurances =
                await _insuranceRepository
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
                await _insuranceRepository
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
                await _insuranceRepository
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
                await _insuranceRepository
                    .GetActiveInsurancesAsync();

            return _mapper.Map<
                IEnumerable<InsuranceDto>>(
                    insurances);
        }

        public async Task<int>
            AddInsuranceAsync(
                CreateInsuranceDto insuranceDto)
        {
            var patient =
                await _patientRepository
                    .GetByIdAsync(
                        insuranceDto.PatientId);

            if (patient == null)
            {
                throw new PatientNotFoundException();
            }

            var existingInsurance =
                await _insuranceRepository
                    .GetInsuranceByPolicyNumberAsync(
                        insuranceDto.PolicyNumber);

            if (existingInsurance != null)
            {
                throw new DuplicatePolicyNumberException();
            }

            var insurance =
                _mapper.Map<Insurance>(
                    insuranceDto);

            insurance.PatientId =
                insuranceDto.PatientId;

            await _insuranceRepository
                .AddAsync(
                    insurance);

            await _context
                .SaveChangesAsync();

            return insurance
                .InsuranceId;
        }

        public async Task
            UpdateInsuranceAsync(
                int insuranceId,
                UpdateInsuranceDto insuranceDto)
        {
            var insurance =
                await _insuranceRepository
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

            await _insuranceRepository
                .UpdateAsync(
                    insurance);

            await _context
                .SaveChangesAsync();
        }

        public async Task
            DeleteInsuranceAsync(
                int insuranceId)
        {
            var insurance =
                await _insuranceRepository
                    .GetByIdAsync(
                        insuranceId);

            if (insurance == null)
            {
                throw new InsuranceNotFoundException();
            }

            await _insuranceRepository
                .DeleteAsync(
                    insuranceId);

            await _context
                .SaveChangesAsync();
        }
    }
}