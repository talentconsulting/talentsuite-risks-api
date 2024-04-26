using TalentConsulting.TalentSuite.RisksApi.Db.Entities;

namespace TalentConsulting.TalentSuite.RisksApi.Tests.Endpoints.Risks;

[TestFixture]
public class PostRiskEndpointTests : ServerFixtureBase
{
    [SetUp]
    public async Task Setup()
    {
        await Server.ResetDbAsync(async ctx => {
            ctx.Risks.AddRange(TestData.Project1.Risks);
            await ctx.SaveChangesAsync(CancellationToken.None);
        });
    }

    public record struct TestCreateRiskDto(
        string? ProjectId,
        string? Description,
        string? Impact,
        string? CreatedByReportId,
        string? CreatedByUserId,
        string? Status
    );

    internal record struct TestInvalidCreateRiskDto(
        string? ProjectId,
        string? Description,
        string? Impact,
        string? CreatedByReportId,
        string? CreatedByUserId,
        string? Status,
        string? ResolvedByReportId
    );

    public static IEnumerable<object[]> BadRequestTestCases
    {
        get
        {
            var empty = Guid.Empty.ToString();
            var projectId = TestContext.CurrentContext.Random.NextGuid().ToString();
            var userId = TestContext.CurrentContext.Random.NextGuid().ToString();
            var reportId = TestContext.CurrentContext.Random.NextGuid().ToString();
            var risk = new TestCreateRiskDto(projectId, "description", "impact", reportId, userId, RiskStatus.Green.ToString());

            yield return new object[] { risk with { ProjectId = empty }, "'Project Id' must not be empty" };
            yield return new object[] { risk with { ProjectId = null }, "BadHttpRequestException" };
            yield return new object[] { risk with { ProjectId = "Invalid" }, "BadHttpRequestException" };
            
            yield return new object[] { risk with { Description = null }, "'Description' must not be empty" };
            yield return new object[] { risk with { Description = string.Empty }, "'Description' must not be empty" };
            
            yield return new object[] { risk with { CreatedByReportId = null }, "BadHttpRequestException" };
            yield return new object[] { risk with { CreatedByReportId = "Invalid" }, "BadHttpRequestException" };
            
            yield return new object[] { risk with { CreatedByUserId = null }, "BadHttpRequestException" };
            yield return new object[] { risk with { CreatedByUserId = "Invalid" }, "BadHttpRequestException" };

            yield return new object[] { risk with { Impact = null }, "'Impact' must not be empty" };
            yield return new object[] { risk with { Impact = string.Empty }, "'Impact' must not be empty" };
            
            yield return new object[] { risk with { Status = string.Empty }, "BadHttpRequestException" };
            yield return new object[] { risk with { Status = "Invalid" }, "BadHttpRequestException" };
            yield return new object[] { risk with { Status = null }, "BadHttpRequestException" };
        }
    }

    [Test]
    [TestCaseSource(nameof(BadRequestTestCases))]
    public async Task Post_Returns_BadRequest(TestCreateRiskDto risk, string expectedError)
    {
        // arrange
        var content = JsonContent.Create(risk);

        // act
        using var response = await Client.PostAsync($"/risks", content);
        var body = await response.Content.ReadAsStringAsync();

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        body.ShouldContain(expectedError);
    }

    [Test]
    public async Task Post_Returns_Created()
    {
        // arrange
        var testStartTime = DateTime.UtcNow;
        var risk = new TestCreateRiskDto(
            TestContext.CurrentContext.Random.NextGuid().ToString(),
            "A description",
            "The impact",
            TestContext.CurrentContext.Random.NextGuid().ToString(),
            TestContext.CurrentContext.Random.NextGuid().ToString(),
            RiskStatus.Green.ToString()
        );
        Risk? actual = null;

        // act
        using var response = await Client.PostAsync($"/risks", JsonContent.Create(risk));
        var id = response.Headers.Location?.OriginalString.Split('/')[^1];
        if (Guid.TryParse(id, out Guid riskId))
        {
            await Server.QueryDbAsync(async ctx => actual = await ctx.Risks.FindAsync(riskId));
        }

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        actual.ShouldNotBeNull().ShouldSatisfyAllConditions(
            () => actual.Id.ToString().ShouldBe(id),
            () => actual.ProjectId.ToString().ShouldBe(risk.ProjectId),
            () => actual.Description.ShouldBe(risk.Description),
            () => actual.Impact.ShouldBe(risk.Impact),
            () => actual.CreatedByReportId.ToString().ShouldBe(risk.CreatedByReportId),
            () => actual.CreatedByUserId.ToString().ShouldBe(risk.CreatedByUserId),
            () => actual.CreatedWhen.ShouldBeGreaterThan(testStartTime),
            () => actual.ResolvedByReportId.ShouldBeNull(),
            () => actual.ResolvedByUserId.ShouldBeNull(),
            () => actual.ResolvedWhen.ShouldBeNull(),
            () => actual.State.ShouldBe(RiskState.New),
            () => actual.Status.ToString().ShouldBe(risk.Status)
        );
    }

    [Test]
    public async Task Post_Does_Not_Accept_Additional_Properties()
    {
        // arrange
        var risk = new TestInvalidCreateRiskDto(
            TestContext.CurrentContext.Random.NextGuid().ToString(),
            "A description",
            "The impact",
            TestContext.CurrentContext.Random.NextGuid().ToString(),
            TestContext.CurrentContext.Random.NextGuid().ToString(),
            RiskStatus.Green.ToString(),
            TestContext.CurrentContext.Random.NextGuid().ToString() // attempt to set a property the method does not accept
        );
        Risk? actual = null;

        // act
        using var response = await Client.PostAsync($"/risks", JsonContent.Create(risk));
        var id = response.Headers.Location?.OriginalString.Split('/')[^1];
        if (Guid.TryParse(id, out Guid riskId))
        {
            await Server.QueryDbAsync(async ctx => actual = await ctx.Risks.FindAsync(riskId));
        }

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        actual.ShouldNotBeNull().ShouldSatisfyAllConditions(
            () => actual.Id.ToString().ShouldBe(id),
            () => actual.ResolvedByReportId.ShouldBeNull()
        );
    }
}