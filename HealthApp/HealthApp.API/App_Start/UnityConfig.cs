using AutoMapper;
using HealthApp.API.Data;
using HealthApp.API.Mapping;
using HealthApp.API.Repository.Impl;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Impl;
using HealthApp.API.Service.Interface;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace HealthApp.API
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<HealthAppEntities>();
            container.RegisterType<IAppointmentRepository,AppointmentRepository>();
            container.RegisterType<IDoctorRepository,DoctorRepository>();
            container.RegisterType<IPatientRepository,PatientRepository>();
            container.RegisterType<IHealthRecordRepository,HealthRecordRepository>();

            container.RegisterType<IAppointmentService,AppointmentService>();
            container.RegisterType<IDoctorService,DoctorService>();
            container.RegisterType<IPatientService, PatientService>();
            container.RegisterType<IHealthRecordService,HealthRecordService>();

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<MappingProfile>();
            });
            IMapper mapper=mapperConfig.CreateMapper();
            container.RegisterInstance<IMapper>(mapper);


            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}