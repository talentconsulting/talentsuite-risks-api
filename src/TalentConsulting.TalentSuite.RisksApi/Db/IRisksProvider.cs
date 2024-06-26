﻿using TalentConsulting.TalentSuite.RisksApi.Common.Models;
using TalentConsulting.TalentSuite.RisksApi.Db.Entities;

namespace TalentConsulting.TalentSuite.RisksApi.Db;

internal interface IRisksProvider
{
    Task<PagedResults<Risk>> FetchAllBy(Guid? projectId, SafePageParameters pagingInfo, CancellationToken cancellationToken);
    Task<bool> Delete(Guid riskId, CancellationToken cancellationToken);
    Task<Risk?> Fetch(Guid riskId, CancellationToken cancellationToken);
    Task<Risk?> Update(Risk risk, CancellationToken cancellationToken);
    Task<Risk> Create(Risk risk, CancellationToken cancellationToken);
}
