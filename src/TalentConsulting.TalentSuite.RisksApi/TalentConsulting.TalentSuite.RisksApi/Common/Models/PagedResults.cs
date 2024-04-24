namespace TalentConsulting.TalentSuite.ReportsApi.Db;

public record struct PagedResults<T>(int Page, int PageSize, int First, int Last, int TotalCount, List<T> Results);