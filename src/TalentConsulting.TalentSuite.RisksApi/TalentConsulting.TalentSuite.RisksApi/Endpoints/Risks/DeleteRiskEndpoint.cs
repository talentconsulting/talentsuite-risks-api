﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TalentConsulting.TalentSuite.ReportsApi.Db;
using TalentConsulting.TalentSuite.RisksApi.Common.Dtos;

namespace TalentConsulting.TalentSuite.RisksApi.Endpoints;

internal sealed class DeleteRiskEndpoint
{
    public static void Register(WebApplication app)
    {
        app.MapDelete("/risks/{id:guid}", DeleteRisk)
            .Produces<Risk>(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags("Risks")
            .WithDescription("Delete a specific Risk")
            .WithOpenApi();
    }

    private static async Task<IResult> DeleteRisk(
        [FromServices] IRisksProvider risksProvider,
        Guid id,
        CancellationToken cancellationToken)
    {
        return await risksProvider.Delete(id, cancellationToken)
            ? Results.NoContent()
            : Results.NotFound();
    }
}