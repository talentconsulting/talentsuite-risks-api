﻿namespace TalentConsulting.TalentSuite.RisksApi.Endpoints;

internal sealed class GetReadinessEndpoint
{
    public static void Register(WebApplication app)
    {
        app.MapGet("/readiness", GetReadiness)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status503ServiceUnavailable)
            .WithTags("Service Status")
            .WithDescription("Returns whether the service is ready")
            .WithOpenApi();
    }

    // TODO: implement whatever constitutes a readiness check
    private static IResult GetReadiness() => Results.NoContent();
}