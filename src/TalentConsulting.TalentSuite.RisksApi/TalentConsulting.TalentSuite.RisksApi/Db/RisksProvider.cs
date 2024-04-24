using Microsoft.EntityFrameworkCore;
using TalentConsulting.TalentSuite.ReportsApi.Db;
using TalentConsulting.TalentSuite.RisksApi.Common.Models;
using TalentConsulting.TalentSuite.RisksApi.Db.Entities;

namespace TalentConsulting.TalentSuite.RisksApi.Db;

internal class RisksProvider(IApplicationDbContext context) : IRisksProvider
{
    //public async Task<Risk> Create(Risk report, CancellationToken cancellationToken)
    //{
    //    context.Risks.Add(report);
    //    await context.SaveChangesAsync(cancellationToken);

    //    return report;
    //}

    //public async Task<Risk?> Fetch(Guid reportId, CancellationToken cancellationToken)
    //{
    //    return await context.Risks.FindAsync([reportId], cancellationToken: cancellationToken);
    //}

    //public async Task<bool> Delete(Guid reportId, CancellationToken cancellationToken)
    //{
    //    var report = await Fetch(reportId, cancellationToken);
    //    if (report is null)
    //    {
    //        return false;
    //    }

    //    foreach (var risk in report.Risks)
    //    {
    //        context.Remove(risk);
    //    }
    //    context.Remove(report);
    //    await context.SaveChangesAsync(cancellationToken);
        
    //    return true;
    //}

    //public async Task<Risk?> Update(Risk report, CancellationToken cancellationToken)
    //{
    //    var existingReport = await Fetch(report.Id, cancellationToken);
    //    if (existingReport is null)
    //    {
    //        return null;
    //    }

    //    context.Entry(existingReport).CurrentValues.SetValues(report);
    //    foreach (var risk in report.Risks)
    //    {
    //        var existingRisk = existingReport.Risks.FirstOrDefault(r => r.Id == risk.Id);
    //        if (existingRisk is null)
    //        {
    //            existingReport.Risks.Add(risk);
    //        }
    //        else
    //        {
    //            context.Entry(existingRisk).CurrentValues.SetValues(risk);
    //        }
    //    }

    //    foreach (var risk in existingReport.Risks)
    //    {
    //        if (!report.Risks.Any(r => r.Id == risk.Id))
    //        {
    //            context.Remove(risk);
    //        }
    //    }

    //    var transaction = context.Database.BeginTransaction();
    //    try
    //    {
    //        await context.SaveChangesAsync(cancellationToken);
    //        await transaction.CommitAsync(cancellationToken);
    //    }
    //    catch
    //    {
    //        await transaction.RollbackAsync(cancellationToken);
    //        throw;
    //    }

    //    return existingReport;
    //}

    public async Task<PagedResults<Risk>> FetchAllBy(Guid? projectId, SafePageParameters pagingInfo, CancellationToken cancellationToken)
    {
        await context.SetupTestData();
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