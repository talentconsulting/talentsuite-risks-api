using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using TalentConsulting.TalentSuite.RisksApi.Db.Entities;

namespace TalentConsulting.TalentSuite.RisksApi.Db;

[ExcludeFromCodeCoverage]
internal class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
    }

    public async Task Ping(CancellationToken cancellationToken)
    {
        await Database
                .ExecuteSqlRawAsync("SELECT 1;", cancellationToken)
                .ConfigureAwait(false);
    }

    public async Task SetupTestData()
    {
        var projectId = Guid.NewGuid();

        Risks.AddRange(
            new Risk() {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                CreatedByReportId = Guid.NewGuid(),
                CreatedByUserId = Guid.NewGuid(),
                CreatedWhen = DateTime.UtcNow,
            },
            new Risk()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                CreatedByReportId = Guid.NewGuid(),
                CreatedByUserId = Guid.NewGuid(),
                CreatedWhen = DateTime.UtcNow,
            },
            new Risk()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                CreatedByReportId = Guid.NewGuid(),
                CreatedByUserId = Guid.NewGuid(),
                CreatedWhen = DateTime.UtcNow,
            },
            new Risk()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                CreatedByReportId = Guid.NewGuid(),
                CreatedByUserId = Guid.NewGuid(),
                CreatedWhen = DateTime.UtcNow,
            },
            new Risk()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                CreatedByReportId = Guid.NewGuid(),
                CreatedByUserId = Guid.NewGuid(),
                CreatedWhen = DateTime.UtcNow,
            },
            new Risk()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                CreatedByReportId = Guid.NewGuid(),
                CreatedByUserId = Guid.NewGuid(),
                CreatedWhen = DateTime.UtcNow,
            },
            new Risk()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                CreatedByReportId = Guid.NewGuid(),
                CreatedByUserId = Guid.NewGuid(),
                CreatedWhen = DateTime.UtcNow,
            },
            new Risk()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                CreatedByReportId = Guid.NewGuid(),
                CreatedByUserId = Guid.NewGuid(),
                CreatedWhen = DateTime.UtcNow,
            },
            new Risk()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                CreatedByReportId = Guid.NewGuid(),
                CreatedByUserId = Guid.NewGuid(),
                CreatedWhen = DateTime.UtcNow,
            },
            new Risk()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                CreatedByReportId = Guid.NewGuid(),
                CreatedByUserId = Guid.NewGuid(),
                CreatedWhen = DateTime.UtcNow,
            },
            new Risk()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                CreatedByReportId = Guid.NewGuid(),
                CreatedByUserId = Guid.NewGuid(),
                CreatedWhen = DateTime.UtcNow,
            },
            new Risk()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                CreatedByReportId = Guid.NewGuid(),
                CreatedByUserId = Guid.NewGuid(),
                CreatedWhen = DateTime.UtcNow,
            },
            new Risk()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                CreatedByReportId = Guid.NewGuid(),
                CreatedByUserId = Guid.NewGuid(),
                CreatedWhen = DateTime.UtcNow,
            },
            new Risk()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                CreatedByReportId = Guid.NewGuid(),
                CreatedByUserId = Guid.NewGuid(),
                CreatedWhen = DateTime.UtcNow,
            },
            new Risk()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                CreatedByReportId = Guid.NewGuid(),
                CreatedByUserId = Guid.NewGuid(),
                CreatedWhen = DateTime.UtcNow,
            },
            new Risk()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                CreatedByReportId = Guid.NewGuid(),
                CreatedByUserId = Guid.NewGuid(),
                CreatedWhen = DateTime.UtcNow,
            }
        );
        await SaveChangesAsync(CancellationToken.None);
    }

    public DbSet<Risk> Risks => Set<Risk>();
}
