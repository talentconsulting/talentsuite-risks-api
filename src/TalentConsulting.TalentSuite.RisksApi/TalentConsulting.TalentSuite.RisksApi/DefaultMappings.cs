using AutoMapper;
using TalentConsulting.TalentSuite.RisksApi.Common.Models;
using TalentConsulting.TalentSuite.RisksApi.Common.Dtos;
using TalentConsulting.TalentSuite.RisksApi.Db.Entities;
using static TalentConsulting.TalentSuite.RisksApi.Endpoints.PostRiskEndpoint;
using static TalentConsulting.TalentSuite.RisksApi.Endpoints.GetRisksEndpoint;
using static TalentConsulting.TalentSuite.RisksApi.Endpoints.PutRiskEndpoint;

namespace TalentConsulting.TalentSuite.RisksApi;

public class DefaultMappings : Profile
{
    public DefaultMappings()
    {
        CreateMap<Risk, RiskDto>().ReverseMap();
        CreateMap(typeof(PagedResults<>), typeof(PagingResults));
        CreateMap<CreateRiskRequest, Risk>();
        CreateMap<UpdateRiskRequest, Risk>();
    }
}
