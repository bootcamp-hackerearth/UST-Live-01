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

            // DbContext
            container.RegisterType<HealthcareDbContext>();

            // Repositories
            container.RegisterType<IPatientRepository, PatientRepository>();
            container.RegisterType<IDoctorRepository, DoctorRepository>();
            container.RegisterType<IAppointmentRepository, AppointmentRepository>();
            container.RegisterType<IHealthRecordRepository, HealthRecordRepository>();

            // Services
            container.RegisterType<IPatientService, PatientService>();
            container.RegisterType<IDoctorService, DoctorService>();
            container.RegisterType<IAppointmentService, AppointmentService>();
            container.RegisterType<IHealthRecordService, HealthRecordService>();

            // AutoMapper
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