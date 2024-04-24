﻿using Microsoft.Extensions.Diagnostics.HealthChecks;
using TalentConsulting.TalentSuite.RisksApi.Db;

namespace TalentConsulting.TalentSuite.RisksApi;

internal class DefaultHealthCheck(IApplicationDbContext dbContext) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        if (dbContext.Database.ProviderName?.EndsWith("InMemory", StringComparison.OrdinalIgnoreCase) ?? false)
        {
            return HealthCheckResult.Healthy();
        }

        try
        {
            await dbContext.Ping(cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch
        {
            return HealthCheckResult.Unhealthy();
        }
    }
}