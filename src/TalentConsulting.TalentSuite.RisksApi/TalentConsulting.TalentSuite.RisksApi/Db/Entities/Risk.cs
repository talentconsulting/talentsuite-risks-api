using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TalentConsulting.TalentSuite.RisksApi.Db.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RiskStatus
{
    Red,
    Amber,
    Green
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RiskState
{
    New,
    Increasing,
    Decreasing,
    Static,
    Resolved
}

[Table("risks")]
public class Risk
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }

    public string Description { get; set; } = string.Empty;
    public string Impact { get; set; } = string.Empty;

    public Guid CreatedByReportId { get; set; }
    public Guid CreatedByUserId { get; set; }
    public DateTime CreatedWhen { get; set; }
    
    public Guid? ResolvedByReportId { get; set; }
    public Guid? ResolvedByUserId { get; set; }
    public DateTime? ResolvedWhen { get; set; }

    public RiskState State { get; set; } = RiskState.New;
    public RiskStatus Status { get; set; } = RiskStatus.Red;
}