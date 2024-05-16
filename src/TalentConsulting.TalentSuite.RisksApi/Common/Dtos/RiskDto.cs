using TalentConsulting.TalentSuite.RisksApi.Db.Entities;

namespace TalentConsulting.TalentSuite.RisksApi.Common.Dtos;

public record struct RiskDto(
    Guid Id,
    Guid ProjectId,
    string Description,
    string Impact,
    Guid CreatedByReportId,
    Guid CreatedByUserId,
    DateTime CreatedWhen,
    Guid? ResolvedByReportId,
    Guid? ResolvedByUserId,
    DateTime? ResolvedWhen,
    RiskState State,
    RiskStatus Status
);