namespace TalentConsulting.TalentSuite.RisksApi.Common.Dtos;

public record struct PagingInfo(int TotalCount, int Page, int PageSize, int First, int Last);