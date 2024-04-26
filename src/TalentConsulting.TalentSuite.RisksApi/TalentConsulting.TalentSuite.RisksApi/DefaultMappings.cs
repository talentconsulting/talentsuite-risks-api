using AutoMapper;
using static TalentConsulting.TalentSuite.RisksApi.Endpoints.PostRiskEndpoint;
using static TalentConsulting.TalentSuite.RisksApi.Endpoints.GetRisksEndpoint;
using TalentConsulting.TalentSuite.RisksApi.Common.Models;
using TalentConsulting.TalentSuite.RisksApi.Db.Entities;
using TalentConsulting.TalentSuite.RisksApi.Common.Dtos;

namespace TalentConsulting.TalentSuite.RisksApi;

public class DefaultMappings : Profile
{
    public DefaultMappings()
    {
        CreateMap<Risk, RiskDto>().ReverseMap();
        CreateMap(typeof(PagedResults<>), typeof(PagingResults));
        CreateMap<CreateRiskRequest, Risk>();
    }
}
