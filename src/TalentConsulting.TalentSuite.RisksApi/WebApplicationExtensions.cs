using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using TalentConsulting.TalentSuite.RisksApi.Db;
using TalentConsulting.TalentSuite.RisksApi.Endpoints;

namespace TalentConsulting.TalentSuite.RisksApi;

[ExcludeFromCodeCoverage]
public static partial class WebApplicationExtensions
{
    static void RegisterEndpoints(this WebApplication app)
    {
        GetRisksEndpoint.Register(app);
        GetRiskEndpoint.Register(app);
        PostRiskEndpoint.Register(app);
        PutRiskEndpoint.Register(app);
        DeleteRiskEndpoint.Register(app);
        GetReadinessEndpoint.Register(app);
        GetInfoEndpoint.Register(app);
    }

    public static async Task Configure(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options => {
                options.ShowCommonExtensions();
            });

            app.UseCors("AllowReactAppForLocalDev");
        }

        app.UseHttpsRedirection();
        app.MapHealthChecks("/health");
        await app.InitialiseDb();
        app.RegisterEndpoints();
    }

    private static async Task InitialiseDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        if (!dbContext.Database.ProviderName?.Contains("InMemory", StringComparison.OrdinalIgnoreCase) ?? false)
        {
            await dbContext.Database.MigrateAsync(CancellationToken.None);
        }
    }
}