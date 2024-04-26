using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TalentConsulting.TalentSuite.RisksApi.Db;
using TalentConsulting.TalentSuite.RisksApi.Common.Dtos;
using TalentConsulting.TalentSuite.RisksApi.Common.Models;

namespace TalentConsulting.TalentSuite.RisksApi.Endpoints;

internal sealed class GetRisksEndpoint
{
    internal record struct PagingParametersDto(int Page, int PageSize);
    internal record struct PagingResults(int TotalCount, int Page, int PageSize, int First, int Last);
    internal record struct GetRisksResponse(PagingResults PagingResults, IEnumerable<RiskDto> Risks);

    public static void Register(WebApplication app)
    {
        app.MapGet("/risks", GetRisks)
            .Produces<GetRisksResponse>(StatusCodes.Status200OK)
            .WithTags("Risks")
            .WithDescription("Returns a list of risks per the query parameters")
            .WithOpenApi();
    }

    private static async Task<IResult> GetRisks(
        [FromServices] IRisksProvider risksProvider,
        [FromServices] IMapper mapper,
        int? page,
        int? pageSize,
        Guid? projectId,
        CancellationToken cancellationToken)
    {
        var safePageParams = new SafePageParameters(page, pageSize);
        var results = await risksProvider.FetchAllBy(projectId, safePageParams, cancellationToken);

        var pagingInfo = mapper.Map<PagingResults>(results);
        var risks = mapper.Map<IEnumerable<RiskDto>>(results.Results);
        var response = new GetRisksResponse(pagingInfo, risks);

        return TypedResults.Ok(response);
    }
}