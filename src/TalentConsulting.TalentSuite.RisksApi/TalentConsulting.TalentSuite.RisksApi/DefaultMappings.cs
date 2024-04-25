using AutoMapper;
using static TalentConsulting.TalentSuite.RisksApi.Endpoints.PostRiskEndpoint;
using TalentConsulting.TalentSuite.RisksApi.Common.Models;
using TalentConsulting.TalentSuite.RisksApi.Db.Entities;
using RiskDto = TalentConsulting.TalentSuite.RisksApi.Common.Dtos.Risk;
using PagingInfoDto = TalentConsulting.TalentSuite.RisksApi.Common.Dtos.PagingInfo;

namespace TalentConsulting.TalentSuite.RisksApi;

public class DefaultMappings : Profile
{
    public DefaultMappings()
    {
        CreateMap<Risk, RiskDto>().ReverseMap();
        CreateMap(typeof(PagedResults<>), typeof(PagingInfoDto));
        CreateMap<CreateRiskRequest, Risk>();
    }
}
