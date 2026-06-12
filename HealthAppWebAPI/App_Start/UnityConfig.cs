using AutoMapper;
using HealthAppWebAPI.App_Start;
using HealthAppWebAPI.Repositories.Impl;
using HealthAppWebAPI.Repositories.Interfaces;
using HealthAppWebAPI.Services.Interfaces;
using HealthAppWebAPI.Services.Impl;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace HealthAppWebAPI
{
	public static class UnityConfig
	{
		public static void RegisterComponents()
		{
			var container = new UnityContainer();


			container.RegisterType<HealthAppDbContext>();
			container.RegisterType<IDoctorRepository, DoctorRepository>();
			container.RegisterType
				<IPatientRepository, PatientRepository>();

			container.RegisterType
				<IPatientService, PatientService>();

			container.RegisterType
				<IDoctorRepository, DoctorRepository>();

			container.RegisterType<IDoctorService, DoctorService>();

			container.RegisterType<IAppointmentRepository, AppointmentRepository>();

			container.RegisterType<IAppointmentService, AppointmentService>();

			container.RegisterType<IHealthRecordRepository, HealthRecordRepository>();
			container.RegisterType<IHealthRecordService, HealthRecordService>();

			var mappingConfig = new MapperConfiguration(config =>
			{

				config.AddProfile<MappingProfile>();

			}); IMapper mapper = mappingConfig.CreateMapper(); container.RegisterInstance<IMapper>(mapper);


			GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
		}
	}
}
