using TalentConsulting.TalentSuite.RisksApi.Db.Entities;

namespace TalentConsulting.TalentSuite.RisksApi.Tests;

internal class TestData
{
    public static class Project1
    {
        public static readonly Guid ProjectId = TestContext.CurrentContext.Random.NextGuid();

        public static readonly IEnumerable<Risk> Risks = [
            new Risk()
            {
                Id = TestContext.CurrentContext.Random.NextGuid(),
                ProjectId = ProjectId,
                CreatedByReportId = TestContext.CurrentContext.Random.NextGuid(),
                CreatedByUserId = TestContext.CurrentContext.Random.NextGuid(),
                CreatedWhen = DateTime.UtcNow,
                State = RiskState.New,
                Status = RiskStatus.Red
            },
            new Risk()
            {
                Id = TestContext.CurrentContext.Random.NextGuid(),
                ProjectId = ProjectId,
                CreatedByReportId = TestContext.CurrentContext.Random.NextGuid(),
                CreatedByUserId = TestContext.CurrentContext.Random.NextGuid(),
                CreatedWhen = DateTime.UtcNow,
                State = RiskState.New,
                Status = RiskStatus.Red
            },
            new Risk()
            {
                Id = TestContext.CurrentContext.Random.NextGuid(),
                ProjectId = ProjectId,
                CreatedByReportId = TestContext.CurrentContext.Random.NextGuid(),
                CreatedByUserId = TestContext.CurrentContext.Random.NextGuid(),
                CreatedWhen = DateTime.UtcNow,
                State = RiskState.New,
                Status = RiskStatus.Red
            },
            new Risk()
            {
                Id = TestContext.CurrentContext.Random.NextGuid(),
                ProjectId = ProjectId,
                CreatedByReportId = TestContext.CurrentContext.Random.NextGuid(),
                CreatedByUserId = TestContext.CurrentContext.Random.NextGuid(),
                CreatedWhen = DateTime.UtcNow,
                State = RiskState.New,
                Status = RiskStatus.Red
            },
            new Risk()
            {
                Id = TestContext.CurrentContext.Random.NextGuid(),
                ProjectId = ProjectId,
                CreatedByReportId = TestContext.CurrentContext.Random.NextGuid(),
                CreatedByUserId = TestContext.CurrentContext.Random.NextGuid(),
                CreatedWhen = DateTime.UtcNow,
                State = RiskState.New,
                Status = RiskStatus.Red
            },
        ];

        internal static IEnumerable<Risk> GenerateNewReports(int count)
        {
            return GenerateNewRisks(count, ProjectId);
        }
    }

    private static IEnumerable<Risk> GenerateNewRisks(int count, Guid projectId)
    {
        return Enumerable.Range(1, count).Select(x => new Risk()
        {
            Id = TestContext.CurrentContext.Random.NextGuid(),
            ProjectId = projectId,
            CreatedByReportId = TestContext.CurrentContext.Random.NextGuid(),
            CreatedByUserId = TestContext.CurrentContext.Random.NextGuid(),
            CreatedWhen = DateTime.UtcNow,
            State = RiskState.New,
            Status = RiskStatus.Red
        });
    }
}
