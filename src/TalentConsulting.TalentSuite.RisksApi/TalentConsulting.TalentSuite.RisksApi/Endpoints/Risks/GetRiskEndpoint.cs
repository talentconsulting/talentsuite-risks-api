using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TalentConsulting.TalentSuite.RisksApi.Db;
using TalentConsulting.TalentSuite.RisksApi.Common.Dtos;

namespace TalentConsulting.TalentSuite.RisksApi.Endpoints;

internal sealed class GetRiskEndpoint
{
    public static void Register(WebApplication app)
    {
        app.MapGet("/risks/{id:guid}", GetRisk)
            .Produces<Risk>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
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
        return risk is null
            ? Results.NotFound()
            : TypedResults.Ok(mapper.Map<Risk>(risk));
    }
}