namespace TalentConsulting.TalentSuite.RisksApi.Common.Dtos;

public record struct GetRisksResponse(PagingInfo PagingInfo, IEnumerable<Risk> Risks);