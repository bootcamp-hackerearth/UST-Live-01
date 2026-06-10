using AutoMapper;
//using HealthCareApi.Mapping;

namespace HealthCareApi.App_Start
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
        {

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            IMapper mapper = config.CreateMapper();
            config.AssertConfigurationIsValid();
            return mapper;

        }
    }
}