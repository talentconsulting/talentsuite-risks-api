using TalentConsulting.TalentSuite.RisksApi.Db.Entities;

namespace TalentConsulting.TalentSuite.RisksApi.Common.Dtos;

public record struct Risk(
    Guid Id,
    Guid ProjectId,
    string Description,
    string Impact,
    Guid CreatedByReportId,
    Guid CreatedByUserId,
    DateTime CreatedWhen,
    Guid? ResolvedByReportId,
    Guid? ResolvedBy,
    DateTime? ResolvedWhen,
    RiskState State,
    RiskStatus Status
);