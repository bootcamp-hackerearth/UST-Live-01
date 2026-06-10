using AutoMapper;
using HealthCare_Appointment_Portal.DTOs.DoctorDtos;
using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare_Appointment_Portal.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public DoctorService(
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorDto>>
            GetAllDoctorsAsync()
        {
            var doctors =
                await _doctorRepository
                    .GetAllAsync();

            return _mapper.Map<
                IEnumerable<DoctorDto>>(
                    doctors);
        }

        public async Task<DoctorDto>
            GetDoctorByIdAsync(
                int doctorId)
        {
            var doctor =
                await _doctorRepository
                    .GetByIdAsync(
                        doctorId);

            if (doctor == null)
            {
                throw new DoctorNotFoundException();
            }

            return _mapper.Map<
                DoctorDto>(
                    doctor);
        }

        public async Task<int>
            AddDoctorAsync(
                CreateDoctorDto doctorDto)
        {
            var doctor =
                _mapper.Map<Doctor>(
                    doctorDto);

            await _doctorRepository
                .AddAsync(
                    doctor);

            var user =
                new User
                {
                    UserCode =
                        $"D{doctor.DoctorId:D3}",

                    Email =
                        $"doctor{doctor.DoctorId}@hospital.com",

                    PasswordHash =
                        string.Empty,

                    Role =
                        Role.Doctor,

                    ReferenceId =
                        doctor.DoctorId
                };

            await _userRepository
                .AddAsync(
                    user);

            return doctor.DoctorId;
        }

        public async Task
            UpdateDoctorAsync(
                int doctorId,
                UpdateDoctorDto doctorDto)
        {
            var doctor =
                await _doctorRepository
                    .GetByIdAsync(
                        doctorId);

            if (doctor == null)
            {
                throw new DoctorNotFoundException();
            }

            _mapper.Map(
                doctorDto,
                doctor);

            await _doctorRepository
                .UpdateAsync(
                    doctor);
        }

        public async Task
            DeleteDoctorAsync(
                int doctorId)
        {
            var doctor =
                await _doctorRepository
                    .GetByIdAsync(
                        doctorId);

            if (doctor == null)
            {
                throw new DoctorNotFoundException();
            }

            var appointments =
                await _appointmentRepository
                    .GetAppointmentsByDoctorAsync(
                        doctorId);

            bool hasConfirmedAppointments =
                appointments.Any(a =>
                    a.Status ==
                    AppointmentStatus.Confirmed);

            if (hasConfirmedAppointments)
            {
                throw new DoctorDeletionException();
            }

            var pendingAppointments =
                appointments
                    .Where(a =>
                        a.Status ==
                        AppointmentStatus.Pending)
                    .ToList();

            foreach (var appointment
                in pendingAppointments)
            {
                appointment.Cancel(
                    Constants
                        .DoctorRemovedFromSystem);

                await _appointmentRepository
                    .UpdateAsync(
                        appointment);
            }

            await _doctorRepository
                .DeleteAsync(
                    doctorId);
        }

        public async Task<IEnumerable<DoctorDto>>
            GetDoctorsBySpecialisationAsync(
                Specialisation specialisation)
        {
            var doctors =
                await _doctorRepository
                    .GetDoctorsBySpecialisationAsync(
                        specialisation);

            return _mapper.Map<
                IEnumerable<DoctorDto>>(
                    doctors);
        }

        public async Task<IEnumerable<DoctorDto>>
            GetDoctorsByNameAsync(
                string searchTerm)
        {
            var doctors =
                await _doctorRepository
                    .GetDoctorsByNameAsync(
                        searchTerm);

            return _mapper.Map<
                IEnumerable<DoctorDto>>(
                    doctors);
        }
    }
}