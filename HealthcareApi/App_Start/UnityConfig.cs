using AutoMapper;
using HealthcareApi.App_Start;
using HealthcareApi.Data;
using HealthcareApi.Repositories;
using HealthcareApi.Repositories.Implementations;
using HealthcareApi.Services;
using HealthcareApi.Services.Implementations;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace HealthcareApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<HealthcareDbContext>();

            container.RegisterType<IPatientRepository, PatientRepository>();
            container.RegisterType<IDoctorRepository, DoctorRepository>();
            container.RegisterType<IAppointmentRepository, AppointmentRepository>();
            container.RegisterType<IHealthRecordRepository, HealthRecordRepository>();

            container.RegisterType<IPatientService, PatientService>();
            container.RegisterType<IDoctorService, DoctorService>();
            container.RegisterType<IAppointmentService, AppointmentService>();
            container.RegisterType<IHealthRecordService, HealthRecordService>();

            var mappingConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<MappingProfile>();
            });

            IMapper mapper = mappingConfig.CreateMapper();
            container.RegisterInstance<IMapper>(mapper);

            GlobalConfiguration.Configuration.DependencyResolver =
                new UnityDependencyResolver(container);
        }
    }
}
