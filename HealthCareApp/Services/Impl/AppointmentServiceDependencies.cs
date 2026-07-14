using AutoMapper;
using HealthCareApp.Repository.Interface;
using HealthCareApp.Services.Interface;
using MassTransit;

namespace HealthCareApp.Services.Impl
{
    public sealed class AppointmentServiceDependencies
    {
        public required IAppointmentRepository AppointmentRepository { get; init; }

        public required IPatientRepository PatientRepository { get; init; }

        public required IDoctorRepository DoctorRepository { get; init; }

        public required IHealthRecordRepository HealthRecordRepository { get; init; }

        public required IDoctorLeaveService DoctorLeaveService { get; init; }

        public required IMapper Mapper { get; init; }

        public required IPublishEndpoint PublishEndpoint { get; init; }

        public required ILogger<AppointmentService> Logger { get; init; }
    }
}