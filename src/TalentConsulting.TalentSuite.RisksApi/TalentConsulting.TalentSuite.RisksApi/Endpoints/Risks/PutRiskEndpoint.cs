﻿using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TalentConsulting.TalentSuite.RisksApi.Db;
using TalentConsulting.TalentSuite.RisksApi.Db.Entities;
using TalentConsulting.TalentSuite.RisksApi.Common.Dtos;

namespace TalentConsulting.TalentSuite.RisksApi.Endpoints;

internal sealed class PutRiskEndpoint
{
    public static void Register(WebApplication app)
    {
        app.MapPut("/risks", PutRisk)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags("Risks")
            .WithDescription("Query for a specific Risk")
            .WithOpenApi();
    }

    private static async Task<IResult> PutRisk(
        [FromServices] IRisksProvider risksProvider,
        [FromServices] IValidator<RiskDto> validator,
        [FromServices] IMapper mapper,
        [FromBody] RiskDto riskDto,
        CancellationToken cancellationToken)
    {
        var results = validator.Validate(riskDto);
        if (!results.IsValid)
        {
            return Results.ValidationProblem(results.ToDictionary());
        }

        var risk = await risksProvider.Update(mapper.Map<Risk>(riskDto), cancellationToken);
        return risk is null
            ? Results.NotFound()
            : Results.NoContent();
    }
}