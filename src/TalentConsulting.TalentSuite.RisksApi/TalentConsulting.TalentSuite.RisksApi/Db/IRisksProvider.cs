using TalentConsulting.TalentSuite.RisksApi.Common.Models;
using TalentConsulting.TalentSuite.RisksApi.Db.Entities;

namespace TalentConsulting.TalentSuite.ReportsApi.Db;

internal interface IRisksProvider
{
    Task<PagedResults<Risk>> FetchAllBy(Guid? projectId, SafePageParameters pagingInfo, CancellationToken cancellationToken);
    //Task<bool> Delete(Guid reportId, CancellationToken cancellationToken);
    //Task<Risk?> Fetch(Guid reportId, CancellationToken cancellationToken);
    //Task<Risk?> Update(Risk report, CancellationToken cancellationToken);
    //Task<Risk> Create(Risk report, CancellationToken cancellationToken);
}
