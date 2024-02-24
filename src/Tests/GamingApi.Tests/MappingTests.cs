using AutoMapper;
using Yld.GamingApi.WebApi.Mapping;

namespace GamingApi.Tests
{
    public class MappingTests
    {
        private readonly IMapper _mapper;

        public MappingTests()
        {
            var assemblyWithMapperProfiles = typeof(GameListProfile).Assembly;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(assemblyWithMapperProfiles);
                cfg.ShouldMapProperty = p => p.GetMethod?.IsPublic == true || p.GetMethod?.IsPrivate == true;
            });

            this._mapper = config.CreateMapper();
        }

        [Fact]
        public void AutomapperConfigurationTest()
        {
            this._mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
