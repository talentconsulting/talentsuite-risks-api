using AutoMapper;

namespace TalentConsulting.TalentSuite.RisksApi.Tests;

internal  static class AutoMapperConfig
{
    public static IMapper Initialize()
    {
        var coreAssembly = typeof(WebApplicationBuilderExtensions).Assembly;
        return new MapperConfiguration(mc => mc.AddMaps(coreAssembly)).CreateMapper();
    }
}