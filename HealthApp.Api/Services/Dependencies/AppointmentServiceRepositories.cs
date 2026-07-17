using HealthApp.Api.Repositories.Interfaces;

namespace HealthApp.Api.Services.Dependencies
{
    public sealed class AppointmentServiceRepositories
    {
        public AppointmentServiceRepositories(
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IDoctorLeaveRepository doctorLeaveRepository)
        {
            AppointmentRepository = appointmentRepository;
            PatientRepository = patientRepository;
            DoctorRepository = doctorRepository;
            DoctorLeaveRepository = doctorLeaveRepository;
        }

        public IAppointmentRepository AppointmentRepository { get; }

        public IPatientRepository PatientRepository { get; }

        public IDoctorRepository DoctorRepository { get; }

        public IDoctorLeaveRepository DoctorLeaveRepository { get; }
    }
}
