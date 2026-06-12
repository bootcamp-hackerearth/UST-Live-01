using AutoMapper;
using HealthCare_Appointment_Portal.Data;
using HealthCare_Appointment_Portal.DTOs.PatientDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PatientService(
            IPatientRepository patientRepository,
            IAppointmentRepository appointmentRepository,
            IUserRepository userRepository,
            ApplicationDbContext context,
            IMapper mapper)
        {
            _patientRepository = patientRepository;
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PatientDto>>
            GetAllPatientsAsync()
        {
            var patients =
                await _patientRepository
                    .GetAllAsync();

            var patientDtos =
                _mapper.Map<List<PatientDto>>(
                    patients);

            foreach (var patientDto in patientDtos)
            {
                var appointments =
                    await _appointmentRepository
                        .GetAppointmentsByPatientAsync(
                            patientDto.PatientId);

                patientDto.AppointmentCount =
                    appointments.Count();
            }

            return patientDtos;
        }

        public async Task<PatientDto>
            GetPatientByIdAsync(
                int patientId)
        {
            var patient =
                await _patientRepository
                    .GetByIdAsync(patientId);

            if (patient == null)
            {
                throw new PatientNotFoundException();
            }

            var patientDto =
                _mapper.Map<PatientDto>(
                    patient);

            var appointments =
                await _appointmentRepository
                    .GetAppointmentsByPatientAsync(
                        patientId);

            patientDto.AppointmentCount =
                appointments.Count();

            return patientDto;
        }

        public async Task<PatientDto>
            GetPatientByEmailAsync(
                string email)
        {
            var patient =
                await _patientRepository
                    .GetPatientByEmailAsync(
                        email);

            if (patient == null)
            {
                throw new PatientNotFoundException();
            }

            return _mapper.Map<PatientDto>(
                patient);
        }

        public async Task<int>
            AddPatientAsync(
                CreatePatientDto patientDto)
        {
            var existingPatient =
                await _patientRepository
                    .GetPatientByEmailAsync(
                        patientDto.Email);

            if (existingPatient != null)
            {
                throw new DuplicatePatientException();
            }

            var patient =
                _mapper.Map<Patient>(
                    patientDto);

            await _patientRepository
                .AddAsync(patient);

            await _context
                .SaveChangesAsync();

            var user = new User
            {
                UserCode =
                    $"P{patient.PatientId:D3}",
                Email =
                    patient.Email,
                PasswordHash =
                    string.Empty,
                Role =
                    Role.Patient,
                ReferenceId =
                    patient.PatientId
            };

            await _userRepository
                .AddAsync(user);

            await _context
                .SaveChangesAsync();

            return patient.PatientId;
        }

        public async Task
            UpdatePatientAsync(
                int patientId,
                UpdatePatientDto patientDto)
        {
            var patient =
                await _patientRepository
                    .GetByIdAsync(
                        patientId);

            if (patient == null)
            {
                throw new PatientNotFoundException();
            }

            var existingPatient =
                await _patientRepository
                    .GetPatientByEmailAsync(
                        patientDto.Email);

            if (existingPatient != null
                &&
                existingPatient.PatientId
                != patientId)
            {
                throw new DuplicatePatientException();
            }

            _mapper.Map(
                patientDto,
                patient);

            await _patientRepository
                .UpdateAsync(
                    patient);

            await _context
                .SaveChangesAsync();
        }

        public async Task DeletePatientAsync(int patientId)
        {
            var patient = await _patientRepository.GetByIdAsync(patientId);

            if (patient == null)
            {
                throw new PatientNotFoundException();
            }

            var appointments =
                await _appointmentRepository.GetAppointmentsByPatientAsync(patientId);

            bool hasConfirmedAppointments =
                appointments.Any(a => a.Status == AppointmentStatus.Confirmed);

            if (hasConfirmedAppointments)
            {
                throw new PatientDeletionException();
            }

            await _patientRepository.DeleteAsync(patientId);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PatientDto>>
            GetPatientsByInsuranceStatusAsync(
                InsuranceStatus status)
        {
            var patients =
                await _patientRepository
                    .GetPatientsByInsuranceStatusAsync(
                        status);

            return _mapper.Map<
                IEnumerable<PatientDto>>(
                    patients);
        }
    }
}