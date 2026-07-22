using HealthApp.Api.Data;
using HealthApp.Api.Repositories.Interfaces;

namespace HealthApp.Api.Services.Dependencies
{
    public sealed class AppointmentServiceRepositories
    {
        public AppointmentServiceRepositories(
            HealthAppDbContext context,
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository,
            IDoctorRepository doctorRepository,
            IDoctorLeaveRepository doctorLeaveRepository)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(appointmentRepository);
            ArgumentNullException.ThrowIfNull(patientRepository);
            ArgumentNullException.ThrowIfNull(doctorRepository);
            ArgumentNullException.ThrowIfNull(doctorLeaveRepository);

            Context = context;
            AppointmentRepository = appointmentRepository;
            PatientRepository = patientRepository;
            DoctorRepository = doctorRepository;
            DoctorLeaveRepository = doctorLeaveRepository;
        }

        public HealthAppDbContext Context { get; }

        public IAppointmentRepository AppointmentRepository { get; }

        public IPatientRepository PatientRepository { get; }

        public IDoctorRepository DoctorRepository { get; }

        public IDoctorLeaveRepository DoctorLeaveRepository { get; }
    }
}
