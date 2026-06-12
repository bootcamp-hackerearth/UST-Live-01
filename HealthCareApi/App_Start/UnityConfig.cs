using AutoMapper;
using HealthCareApi.Repositories.Implementations;
using HealthCareApi.Repositories.Interfaces;
using HealthCareApi.Services.Implementations;
using HealthCareApi.Services.Interfaces;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace HealthCareApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();


            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            IMapper mapper = mapperConfig.CreateMapper();


            
            container.RegisterInstance<IMapper>(mapper);
            container.RegisterType<HealthAppDbContext>();

            container.RegisterType(typeof(IRepository<>), typeof(Repository<>));

           
            container.RegisterType<IDoctorRepository, DoctorRepository>();
            container.RegisterType<IPatientRepository, PatientRepository>();
            container.RegisterType<IAppointmentRepository, AppointmentRepository>();
            container.RegisterType<IHealthRecordRepository, HealthRecordRepository>();

            
            container.RegisterType<IDoctorService, DoctorService>();
            container.RegisterType<IPatientService, PatientService>();
            container.RegisterType<IAppointmentService, AppointmentService>();
            container.RegisterType<IHealthRecordService, HealthRecordService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}