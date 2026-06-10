using AutoMapper;
using HealthAxis.Api.Repositories;
using HealthAxis.Api.Services;
using HealthAxis.Api.Database;
using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;
using HealthAxis.Api.Mapping;

public static class UnityConfig
{
    public static void RegisterComponents()
    {
        var container = new UnityContainer();

        container.RegisterType<AppDBContext>(new HierarchicalLifetimeManager());

        container.RegisterType<IDoctorRepository, DoctorRepositoryImpl>();
        container.RegisterType<IDoctorService, DoctorServiceImpl>();

        container.RegisterType<IPatientRepository, PatientRepositoryImpl>();
        container.RegisterType<IPatientService, PatientServiceImpl>();

        container.RegisterType<IAppointmentRepository, AppointmentRepositoryImpl>();
        container.RegisterType<IAppointmentService, AppointmentServiceImpl>();


        container.RegisterType<IHealthRecordRepository, HealthRecordRepositoryImpl>();
        container.RegisterType<IHealthRecordService, HealthRecordServiceImpl>();

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        IMapper mapper = mapperConfig.CreateMapper();
        container.RegisterInstance(mapper);

        GlobalConfiguration.Configuration.DependencyResolver =
            new UnityDependencyResolver(container);
    }
}