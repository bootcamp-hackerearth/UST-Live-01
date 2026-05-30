using HealthCare_Appointment_Portal.Enums;
using HealthCare_Appointment_Portal.Exceptions;
using HealthCare_Appointment_Portal.Interfaces;
using HealthCare_Appointment_Portal.Models;
using HealthCare_Appointment_Portal.Utilities;

namespace HealthCare_Appointment_Portal.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository
            _appointmentRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository =
                appointmentRepository;
        }

        public Appointment BookAppointment(
            Patient patient,
            Doctor doctor,
            DateOnly date,
            TimeOnly slot)
        {
            DateOnly currentDate =
                DateOnly.FromDateTime(DateTime.Now);

            TimeOnly currentTime =
                TimeOnly.FromDateTime(DateTime.Now);

            if (date < currentDate)
            {
                throw new PastDateException();
            }

            DateOnly maxBookingDate =
                currentDate.AddMonths(6);

            if (date > maxBookingDate)
            {
                throw new AdvanceBookingLimitException();
            }

            if (date == currentDate &&
                slot < currentTime)
            {
                throw new PastTimeSlotException();
            }

            if (!doctor.IsAvailable(date))
            {
                throw new DoctorUnavailableException();
            }

            Appointment? existingAppointment =
                _appointmentRepository
                .GetAllAppointments()
                .FirstOrDefault(a =>
                    a.Doctor.DoctorId ==
                    doctor.DoctorId &&
                    a.ScheduledDate == date &&
                    a.TimeSlot == slot &&
                    a.Status !=
                    AppointmentStatus.Cancelled);

            if (existingAppointment != null)
            {
                throw new AppointmentConflictException();
            }

            Appointment appointment =
                new Appointment
                {
                    Patient = patient,
                    Doctor = doctor,
                    ScheduledDate = date,
                    TimeSlot = slot,
                    Status =
                        AppointmentStatus.Pending
                };

            doctor.Appointments.Add(appointment);

            _appointmentRepository
                .AddAppointment(appointment);

            return appointment;
        }

        public Appointment? GetAppointmentById(
            int appointmentId)
        {
            Appointment? appointment =
                _appointmentRepository
                .GetAppointmentById(appointmentId);

            if (appointment == null)
            {
                throw new AppointmentNotFoundException();
            }

            return appointment;
        }

        public List<Appointment>
            GetAllAppointments()
        {
            return _appointmentRepository
                .GetAllAppointments();
        }

        public List<Appointment>
            GetAppointmentsByPatient(
                int patientId)
        {
            return _appointmentRepository
                .GetAllAppointments()
                .Where(a =>
                    a.Patient.PatientId ==
                    patientId)
                .OrderBy(a =>
                    a.ScheduledDate)
                .ToList();
        }

        public List<Appointment>
            GetAppointmentsByDoctor(
                int doctorId)
        {
            return _appointmentRepository
                .GetAllAppointments()
                .Where(a =>
                    a.Doctor.DoctorId ==
                    doctorId)
                .OrderBy(a =>
                    a.ScheduledDate)
                .ToList();
        }

        public List<Appointment>
            GetUpcomingAppointments()
        {
            DateOnly today =
                DateOnly.FromDateTime(DateTime.Now);

            return _appointmentRepository
                .GetAllAppointments()
                .Where(a =>
                    a.ScheduledDate >= today &&
                    a.Status ==
                    AppointmentStatus.Confirmed)
                .OrderBy(a =>
                    a.ScheduledDate)
                .ToList();
        }

        public List<Appointment>
            GetCompletedAppointments()
        {
            return _appointmentRepository
                .GetAllAppointments()
                .Where(a =>
                    a.Status ==
                    AppointmentStatus.Completed)
                .ToList();
        }

        public void ConfirmAppointment(
            int appointmentId)
        {
            Appointment? appointment =
                _appointmentRepository
                .GetAppointmentById(appointmentId);

            if (appointment == null)
            {
                throw new AppointmentNotFoundException();
            }

            if (appointment.Status !=
                AppointmentStatus.Pending)
            {
                throw new InvalidAppointmentStatusException(
                    Constants.ConfirmOnlyPending);
            }

            appointment.Confirm();

            _appointmentRepository
                .UpdateAppointment(appointment);
        }

        public void CancelAppointment(
            int appointmentId,
            string reason)
        {
            Appointment? appointment =
                _appointmentRepository
                .GetAppointmentById(appointmentId);

            if (appointment == null)
            {
                throw new AppointmentNotFoundException();
            }

            if (appointment.Status !=
                AppointmentStatus.Pending &&
                appointment.Status !=
                AppointmentStatus.Confirmed)
            {
                throw new InvalidAppointmentStatusException(
                    Constants
                        .CancelOnlyPendingOrConfirmed);
            }

            appointment.Cancel(reason);

            _appointmentRepository
                .UpdateAppointment(appointment);
        }

        public void CompleteAppointment(
            int appointmentId)
        {
            Appointment? appointment =
                _appointmentRepository
                .GetAppointmentById(appointmentId);

            if (appointment == null)
            {
                throw new AppointmentNotFoundException();
            }

            if (appointment.Status !=
                AppointmentStatus.Confirmed)
            {
                throw new InvalidAppointmentStatusException(
                    Constants.CompleteOnlyConfirmed);
            }

            appointment.Complete();

            _appointmentRepository
                .UpdateAppointment(appointment);
        }

        public void UpdateAppointment(
            Appointment updatedAppointment)
        {
            Appointment? appointment =
                _appointmentRepository
                .GetAppointmentById(
                    updatedAppointment.AppointmentId);

            if (appointment == null)
            {
                throw new AppointmentNotFoundException();
            }

            _appointmentRepository
                .UpdateAppointment(updatedAppointment);
        }

        public void DeleteAppointmentById(
            int appointmentId)
        {
            Appointment? appointment =
                _appointmentRepository
                .GetAppointmentById(appointmentId);

            if (appointment == null)
            {
                throw new AppointmentNotFoundException();
            }

            if (appointment.Status ==
                AppointmentStatus.Pending ||
                appointment.Status ==
                AppointmentStatus.Confirmed)
            {
                throw new AppointmentDeletionException();
            }

            _appointmentRepository
                .DeleteAppointmentById(
                    appointmentId);
        }
    }
}