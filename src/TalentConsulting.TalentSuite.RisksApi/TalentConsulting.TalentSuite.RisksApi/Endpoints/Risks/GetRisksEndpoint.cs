using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TalentConsulting.TalentSuite.ReportsApi.Db;
using TalentConsulting.TalentSuite.RisksApi.Common.Dtos;
using TalentConsulting.TalentSuite.RisksApi.Common.Models;

namespace TalentConsulting.TalentSuite.RisksApi.Endpoints;

internal sealed class GetRisksEndpoint
{
    internal record struct GetRisksResponse(PagingInfo PagingInfo, IEnumerable<Risk> Risks);

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

        var pagingInfo = mapper.Map<PagingInfo>(results);
        var risks = mapper.Map<IEnumerable<Risk>>(results.Results);
        var response = new GetRisksResponse(pagingInfo, risks);

        return TypedResults.Ok(response);
    }
}