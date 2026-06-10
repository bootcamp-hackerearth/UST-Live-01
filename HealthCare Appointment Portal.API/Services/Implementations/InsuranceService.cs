using AutoMapper;
using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.DTOs.InsuranceDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Services
{
    public class InsuranceService : IInsuranceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public InsuranceService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InsuranceDto>> GetAllInsurancesAsync()
        {
            var insurances = await _context.Insurances.ToListAsync();
            return _mapper.Map<IEnumerable<InsuranceDto>>(insurances);
        }

        public async Task<InsuranceDto> GetInsuranceByIdAsync(int insuranceId)
        {
            var insurance = await _context.Insurances.FindAsync(insuranceId);

            if (insurance == null)
                throw new InsuranceNotFoundException();

            return _mapper.Map<InsuranceDto>(insurance);
        }

        public async Task<IEnumerable<InsuranceDto>> GetInsurancesByPatientAsync(int patientId)
        {
            var insurances = await _context.Insurances
                .Where(i => i.PatientId == patientId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<InsuranceDto>>(insurances);
        }

        public async Task<IEnumerable<InsuranceDto>> GetInsurancesByStatusAsync(InsuranceStatus status)
        {
            var insurances = await _context.Insurances
                .Where(i => i.Status == status)
                .ToListAsync();

            return _mapper.Map<IEnumerable<InsuranceDto>>(insurances);
        }

        public async Task<IEnumerable<InsuranceDto>> GetExpiredInsurancesAsync()
        {
            var today = DateTime.Today;

            var insurances = await _context.Insurances
                .Where(i => i.ExpiryDate < today)
                .ToListAsync();

            return _mapper.Map<IEnumerable<InsuranceDto>>(insurances);
        }

        public async Task<IEnumerable<InsuranceDto>> GetActiveInsurancesAsync()
        {
            var today = DateTime.Today;

            var insurances = await _context.Insurances
                .Where(i => i.ExpiryDate >= today)
                .ToListAsync();

            return _mapper.Map<IEnumerable<InsuranceDto>>(insurances);
        }

        public async Task<int> AddInsuranceAsync(CreateInsuranceDto insuranceDto)
        {
            var existingInsurance = await _context.Insurances
                .FirstOrDefaultAsync(i => i.PolicyNumber == insuranceDto.PolicyNumber);

            if (existingInsurance != null)
                throw new DuplicatePolicyNumberException();

            var insurance = _mapper.Map<Insurance>(insuranceDto);

            _context.Insurances.Add(insurance);

            await _context.SaveChangesAsync();

            return insurance.InsuranceId;
        }

        public async Task UpdateInsuranceAsync(int insuranceId, UpdateInsuranceDto insuranceDto)
        {
            var insurance = await _context.Insurances.FindAsync(insuranceId);

            if (insurance == null)
                throw new InsuranceNotFoundException();

            if (insurance.ExpiryDate.Date < DateTime.Today)
                throw new InsuranceExpiredException();

            _mapper.Map(insuranceDto, insurance);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteInsuranceAsync(int insuranceId)
        {
            var insurance = await _context.Insurances.FindAsync(insuranceId);

            if (insurance == null)
                throw new InsuranceNotFoundException();

            _context.Insurances.Remove(insurance);

            await _context.SaveChangesAsync();
        }
    }
}

