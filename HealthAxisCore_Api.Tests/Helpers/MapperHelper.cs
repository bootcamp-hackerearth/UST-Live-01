using AutoMapper;
using HealthAxisCore_Api.Mappings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;



namespace HealthAxisCore_Api.Tests.Helpers
{
    public static class MapperHelper
    {
        public static IMapper GetMapper()
        {
            var config = new MapperConfiguration(
                cfg =>
                {
                    cfg.AddProfile<MappingProfile>();
                },
                NullLoggerFactory.Instance
            );

            return config.CreateMapper();
        }
    }
}
