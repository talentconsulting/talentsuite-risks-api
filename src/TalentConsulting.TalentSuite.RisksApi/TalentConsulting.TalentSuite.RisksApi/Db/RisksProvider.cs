using Microsoft.EntityFrameworkCore;
using TalentConsulting.TalentSuite.RisksApi.Common.Models;
using TalentConsulting.TalentSuite.RisksApi.Db.Entities;

namespace TalentConsulting.TalentSuite.RisksApi.Db;

internal class RisksProvider(IApplicationDbContext context) : IRisksProvider
{
    public async Task<Risk> Create(Risk risk, CancellationToken cancellationToken)
    {
        context.Risks.Add(risk);
        await context.SaveChangesAsync(cancellationToken);

        return risk;
    }

    public async Task<Risk?> Fetch(Guid riskId, CancellationToken cancellationToken)
    {
        return await context.Risks.FindAsync([riskId], cancellationToken: cancellationToken);
    }

    public async Task<bool> Delete(Guid riskId, CancellationToken cancellationToken)
    {
        var risk = await Fetch(riskId, cancellationToken);
        if (risk is null)
        {
            return false;
        }

        context.Remove(risk);
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<Risk?> Update(Risk risk, CancellationToken cancellationToken)
    {
        var existingRisk = await Fetch(risk.Id, cancellationToken);
        if (existingRisk is null)
        {
            return null;
        }

        context.Entry(existingRisk).CurrentValues.SetValues(risk);
        await context.SaveChangesAsync(cancellationToken);

        return existingRisk;
    }

    public async Task<PagedResults<Risk>> FetchAllBy(Guid? projectId, SafePageParameters pagingInfo, CancellationToken cancellationToken)
    {
        var entities = projectId.HasValue ? context.Risks.Where(x => x.ProjectId == projectId) : context.Risks;
        var totalCount = await entities.CountAsync(cancellationToken);
        
        var maxPage = (int)Math.Ceiling((double)totalCount / pagingInfo.PageSize);
        var actualPage = Math.Max(1, Math.Min(pagingInfo.Page, maxPage));
        var skip = (actualPage - 1) * pagingInfo.PageSize;
        
        entities = entities
            .Skip(skip)
            .Take(pagingInfo.PageSize);

        var results = await entities.ToListAsync(cancellationToken);
        var first = totalCount > 0 ? skip + 1 : 0;
        var last = Math.Max(first + results.Count - 1, 0);
        return new PagedResults<Risk>(actualPage, pagingInfo.PageSize, first, last, totalCount, results);
    }
}