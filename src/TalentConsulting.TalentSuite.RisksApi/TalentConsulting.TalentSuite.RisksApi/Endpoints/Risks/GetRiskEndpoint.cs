using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TalentConsulting.TalentSuite.ReportsApi.Db;
using TalentConsulting.TalentSuite.RisksApi.Common.Dtos;

namespace TalentConsulting.TalentSuite.RisksApi.Endpoints;

internal sealed class GetRiskEndpoint
{
    internal record InfoResponse(string Version);
    
    public static void Register(WebApplication app)
    {
        app.MapGet("/risks/{id:guid}", GetRisk)
            .Produces<Risk>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags("Risks")
            .WithDescription("Query for a specific Risk")
            .WithOpenApi();
    }

    private static async Task<IResult> GetRisk(
        [FromServices] IRisksProvider risksProvider,
        [FromServices] IMapper mapper,
        Guid id,
        CancellationToken cancellationToken)
    {
        var risk = await risksProvider.Fetch(id, cancellationToken);
        var result = mapper.Map<Risk>(risk);

        return risk is null
            ? Results.NotFound()
            : TypedResults.Ok(result);
    }
}