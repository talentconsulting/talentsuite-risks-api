using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TalentConsulting.TalentSuite.ReportsApi.Db;
using TalentConsulting.TalentSuite.RisksApi.Db.Entities;

namespace TalentConsulting.TalentSuite.RisksApi.Endpoints;

internal sealed class PostRiskEndpoint
{
    internal record struct CreateRiskRequest(
        Guid ProjectId,
        string Description,
        string Impact,
        Guid CreatedByReportId,
        Guid CreatedByUserId,
        DateTime CreatedWhen,
        Guid? ResolvedByReportId,
        Guid? ResolvedBy,
        DateTime? ResolvedWhen,
        RiskState State,
        RiskStatus Status
    );

    public static void Register(WebApplication app)
    {
        app.MapPost("/risks", CreateRisk)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags("Risks")
            .WithDescription("Query for a specific Risk")
            .WithOpenApi();
    }

    private static async Task<IResult> CreateRisk(
        HttpContext http,
        [FromServices] IRisksProvider risksProvider,
        [FromServices] IValidator<CreateRiskRequest> validator,
        [FromServices] IMapper mapper,
        [FromBody] CreateRiskRequest createRiskRequest,
        CancellationToken cancellationToken)
    {
        var results = validator.Validate(createRiskRequest);
        if (!results.IsValid)
        {
            return Results.ValidationProblem(results.ToDictionary());
        }

        var risk = mapper.Map<Risk>(createRiskRequest);
        risk = await risksProvider.Create(risk, cancellationToken);

        http.Response.Headers.Location = $"/risks/{risk.Id}";
        return Results.Created();
    }
}