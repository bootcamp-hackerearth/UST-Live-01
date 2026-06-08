using System.Web.Http;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;
using HealthAxis_MVC.Repositories;
using HealthAxis_MVC.Repositories.Impl;
using HealthAxis_MVC.Services;
using HealthAxis_MVC.Services.Impl;
using AutoMapper;
using HealthAxis_Web.App_Start;
using HealthAxis_Web.Database;

public static class UnityConfig
{
    public static void RegisterComponents()
    {
        var container = new UnityContainer();

        // DB Context
        container.RegisterType<AppDBContext>(new HierarchicalLifetimeManager());

        // Repository
        container.RegisterType<IDoctorRepository, DoctorRepository>();

        // Service
        container.RegisterType<IDoctorService, DoctorServiceImpl>();

        // AutoMapper
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        IMapper mapper = mapperConfig.CreateMapper();
        container.RegisterInstance(mapper);

        // Set Web API resolver
        GlobalConfiguration.Configuration.DependencyResolver =
            new UnityDependencyResolver(container);
    }
}