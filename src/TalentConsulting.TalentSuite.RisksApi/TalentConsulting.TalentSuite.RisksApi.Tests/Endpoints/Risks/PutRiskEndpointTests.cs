using TalentConsulting.TalentSuite.RisksApi.Db.Entities;

namespace TalentConsulting.TalentSuite.RisksApi.Tests.Endpoints.Risks;

[TestFixture]
public class PutRiskEndpointTests : ServerFixtureBase
{
    [SetUp]
    public async Task Setup()
    {
        await Server.ResetDbAsync(async ctx => {
            ctx.Risks.AddRange(TestData.Project1.Risks);
            await ctx.SaveChangesAsync(CancellationToken.None);
        });
    }

    public record struct TestUpdateRiskRequest(
        string? Id,
        string? Description,
        string? Impact,
        string? ResolvedByReportId,
        string? ResolvedByUserId,
        string? ResolvedWhen,
        string? State,
        string? Status
    )
    {
        public static TestUpdateRiskRequest From(Risk risk)
        {
            return new TestUpdateRiskRequest(
                risk.Id.ToString(),
                risk.Description,
                risk.Impact,
                risk.ResolvedByReportId?.ToString(),
                risk.ResolvedByUserId?.ToString(),
                risk.ResolvedWhen?.ToString(),
                risk.State.ToString(),
                risk.Status.ToString());
        }
    }

    public static IEnumerable<object[]> BadRequestTestCases
    {
        get
        {
            var empty = Guid.Empty.ToString();
            var risk = TestUpdateRiskRequest.From(TestData.Project1.Risks.First());
            var tooLongText = TestContext.CurrentContext.Random.GetString(Math.Max(Risk.MaxDescriptionLength, Risk.MaxImpactLength) + 1);

            yield return new object[] { risk with { Id = empty }, "'Id' must not be empty" };
            yield return new object[] { risk with { Id = null }, "BadHttpRequestException" };
            yield return new object[] { risk with { Id = "Invalid" }, "BadHttpRequestException" };

            yield return new object[] { risk with { Description = null }, "'Description' must not be empty" };
            yield return new object[] { risk with { Description = string.Empty }, "'Description' must not be empty" };
            yield return new object[] { risk with { Description = tooLongText }, $"The length of 'Description' must be {Risk.MaxDescriptionLength} characters or fewer. You entered {tooLongText.Length} characters." };

            yield return new object[] { risk with { Impact = null }, "'Impact' must not be empty" };
            yield return new object[] { risk with { Impact = string.Empty }, "'Impact' must not be empty" };
            yield return new object[] { risk with { Impact = tooLongText }, $"The length of 'Impact' must be {Risk.MaxImpactLength} characters or fewer. You entered {tooLongText.Length} characters." };

            yield return new object[] { risk with { ResolvedByReportId = "Invalid" }, "BadHttpRequestException" };

            yield return new object[] { risk with { ResolvedByUserId = "Invalid" }, "BadHttpRequestException" };

            yield return new object[] { risk with { State = string.Empty }, "BadHttpRequestException" };
            yield return new object[] { risk with { State = "Invalid" }, "BadHttpRequestException" };
            yield return new object[] { risk with { State = null }, "BadHttpRequestException" };

            yield return new object[] { risk with { Status = string.Empty }, "BadHttpRequestException" };
            yield return new object[] { risk with { Status = "Invalid" }, "BadHttpRequestException" };
            yield return new object[] { risk with { Status = null }, "BadHttpRequestException" };
        }
    }

    [Test]
    [TestCaseSource(nameof(BadRequestTestCases))]
    public async Task Put_Returns_BadRequest(TestUpdateRiskRequest risk, string expectedError)
    {
        // arrange
        var content = JsonContent.Create(risk);

        // act
        using var response = await Client.PutAsync($"/risks", content);
        var body = await response.Content.ReadAsStringAsync();

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        body.ShouldContain(expectedError);
    }

    [Test]
    public async Task Put_Returns_NotFound()
    {
        // arrange
        var risk = new TestUpdateRiskRequest(Guid.NewGuid().ToString(), "A desc", "Impact", null, null, null, RiskState.Static.ToString(), RiskStatus.Amber.ToString());

        // act
        using var response = await Client.PutAsync($"/risks", JsonContent.Create(risk));

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Put_Can_Update_Risk()
    {
        // arrange
        var risk = TestUpdateRiskRequest.From(TestData.Project1.Risks.First()) with
        {
            Description = "Something else",
            Impact = "Something else",
            ResolvedByReportId = Guid.NewGuid().ToString(),
            ResolvedByUserId = Guid.NewGuid().ToString(),
            ResolvedWhen = DateTime.UtcNow.ToString("O"),
            State = RiskState.Resolved.ToString(),
            Status = RiskStatus.Green.ToString()
        };
        Risk? actual = new();
        
        // act
        using var response = await Client.PutAsync($"/risks", JsonContent.Create(risk));
        await Server.QueryDbAsync(async ctx => actual = await ctx.Risks.FindAsync(TestData.Project1.Risks.First().Id));
        var body = await response.Content.ReadAsStringAsync();
        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        actual.ShouldNotBeNull().ShouldSatisfyAllConditions(
            () => actual.Description.ShouldBe(risk.Description),
            () => actual.Impact.ShouldBe(risk.Impact),
            () => actual.ResolvedByReportId?.ToString().ShouldBe(risk.ResolvedByReportId),
            () => actual.ResolvedByUserId?.ToString().ShouldBe(risk.ResolvedByUserId),
            () => actual.ResolvedWhen?.ToString("O").ShouldBe(risk.ResolvedWhen),
            () => actual.State.ToString().ShouldBe(risk.State),
            () => actual.Status.ToString().ShouldBe(risk.Status)
        );
    }
}