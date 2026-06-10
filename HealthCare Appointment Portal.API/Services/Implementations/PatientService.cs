using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PatientService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync(string searchTerm = null)
        {
            var patients = await _unitOfWork.Patients.GetAllAsync();

            // Perform filtering if a search term is provided
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.Trim().ToLower();

                patients = patients.Where(p =>
                    p.FullName.ToLower().Contains(searchTerm) ||
                    p.Email.ToLower().Contains(searchTerm) ||
                    (p.PhoneNumber != null && p.PhoneNumber.Contains(searchTerm)));
            }

            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }

        public async Task<PatientDto> GetPatientByIdAsync(int patientId)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(patientId);

            if (patient == null)
            {
                throw new PatientNotFoundException();
            }

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<PatientDto> GetPatientByEmailAsync(string email)
        {
            var patient = await _unitOfWork.Patients.GetPatientByEmailAsync(email);

            if (patient == null)
            {
                throw new PatientNotFoundException();
            }

            return _mapper.Map<PatientDto>(patient);
        }

        public async Task<int> AddPatientAsync(CreatePatientDto patientDto)
        {
            var existingPatient = await _unitOfWork.Patients.GetPatientByEmailAsync(patientDto.Email);

            if (existingPatient != null)
            {
                throw new DuplicatePatientException();
            }

            var patient = _mapper.Map<Patient>(patientDto);

            await _unitOfWork.Patients.AddAsync(patient);
            await _unitOfWork.CommitAsync();

            var user = new User
            {
                UserCode = $"P{patient.PatientId:D3}",
                Email = patient.Email,
                PasswordHash = string.Empty,
                Role = Role.Patient,
                ReferenceId = patient.PatientId,
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();

            return patient.PatientId;
        }

        public async Task UpdatePatientAsync(int patientId, UpdatePatientDto patientDto)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(patientId);

            if (patient == null)
            {
                throw new PatientNotFoundException();
            }

            var existingPatient = await _unitOfWork.Patients.GetPatientByEmailAsync(patientDto.Email);

            if (existingPatient != null && existingPatient.PatientId != patientId)
            {
                throw new DuplicatePatientException();
            }

            _mapper.Map(patientDto, patient);

            await _unitOfWork.Patients.UpdateAsync(patient);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeletePatientAsync(int patientId)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(patientId);

            if (patient == null)
            {
                throw new PatientNotFoundException();
            }

            var appointments = await _unitOfWork.Appointments.GetAppointmentsByPatientAsync(patientId);

            bool hasConfirmedAppointments = appointments.Any(a => a.Status == AppointmentStatus.Confirmed);

            if (hasConfirmedAppointments)
            {
                throw new PatientDeletionException();
            }

            await _unitOfWork.Patients.DeleteAsync(patientId);
            await _unitOfWork.CommitAsync();
        }
    }
}