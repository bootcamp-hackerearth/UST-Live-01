using HealthApp.Api.Data;
using HealthApp.Api.Repositories.Interfaces;

namespace HealthApp.Api.Services.Dependencies
{
    public sealed class DoctorLeaveServiceDependencies
    {
        public DoctorLeaveServiceDependencies(
            HealthAppDbContext context,
            IDoctorLeaveRepository doctorLeaveRepository,
            IDoctorRepository doctorRepository,
            IAppointmentRepository appointmentRepository,
            IPatientRepository patientRepository)
        {
            Context = context;
            DoctorLeaveRepository = doctorLeaveRepository;
            DoctorRepository = doctorRepository;
            AppointmentRepository = appointmentRepository;
            PatientRepository = patientRepository;
        }

        public HealthAppDbContext Context { get; }

        public IDoctorLeaveRepository DoctorLeaveRepository { get; }

        public IDoctorRepository DoctorRepository { get; }

        public IAppointmentRepository AppointmentRepository { get; }

        public IPatientRepository PatientRepository { get; }
    }
}