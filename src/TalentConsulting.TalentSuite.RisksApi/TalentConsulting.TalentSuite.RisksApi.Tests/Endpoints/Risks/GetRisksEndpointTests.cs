using static TalentConsulting.TalentSuite.RisksApi.Endpoints.GetRisksEndpoint;

namespace TalentConsulting.TalentSuite.RisksApi.Tests.Endpoints.Risks;

[TestFixture]
public class GetRisksEndpointTests : ServerFixtureBase
{
    [Test]
    public async Task Get_Returns_Risks()
    {
        // arrange
        var risks = TestData.Project1.GenerateNewRisks(10);
        var ids = risks.Select(r => r.Id).ToList();
        await Server.ResetDbAsync(async ctx => {
            await ctx.Risks.AddRangeAsync(risks);
            await ctx.SaveChangesAsync(CancellationToken.None);
        });

        // act
        using var response = await Client.GetAsync($"/risks?pageSize=10&page=1&projectId={TestData.Project1.ProjectId}");
        var getRisksResponse = await response.Content.ReadFromJsonAsync<GetRisksResponse>();

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        getRisksResponse.Risks.Count().ShouldBe(10);
        getRisksResponse.PagingResults.Page.ShouldBe(1);
        getRisksResponse.PagingResults.PageSize.ShouldBe(10);
        getRisksResponse.Risks.All(risk => ids.Contains(risk.Id));
    }

    [TestCase("9999999999", "9999999999", "C00211ED-256F-4067-99E0-E1C4316A28D7")]
    [TestCase("foo", "1", "C00211ED-256F-4067-99E0-E1C4316A28D7")]
    [TestCase("1", "foo", "C00211ED-256F-4067-99E0-E1C4316A28D7")]
    [TestCase("1", "1", "foo")]
    [TestCase(null, "1", "C00211ED-256F-4067-99E0-E1C4316A28D7")]
    [TestCase("1", null, "C00211ED-256F-4067-99E0-E1C4316A28D7")]
    [TestCase("1", "1", null)]
    public async Task Get_Rejects_Invalid_Inputs(string? page, string? pageSize, string? projectId)
    {
        // arrange
        await Server.ResetDbAsync(async ctx => {
            var risks = TestData.Project1.GenerateNewRisks(1000);
            await ctx.Risks.AddRangeAsync(risks);
            await ctx.SaveChangesAsync(CancellationToken.None);
        });

        // act
        using var response = await Client.GetAsync($"/risks?pageSize={pageSize}&page={page}&projectId={projectId}");

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [TestCase("-1", "-1", 1, 1)]
    [TestCase("0", "0", 1, 1)]
    [TestCase("1", "9999999", 1, 100)]
    [TestCase("9999999", "1", 999, 1)]
    public async Task Get_Handles_Out_Of_Range_Paging(string page, string pageSize, int expectedPage, int expectedPageSize)
    {
        // arrange
        await Server.ResetDbAsync(async ctx =>
        {
            var risks = TestData.Project1.GenerateNewRisks(1000);
            await ctx.Risks.AddRangeAsync(risks);
            await ctx.SaveChangesAsync(CancellationToken.None);
        });

        // act
        using var response = await Client.GetAsync($"/risks?pageSize={pageSize}&page={page}");
        var reportsResponse = await response.Content.ReadFromJsonAsync<GetRisksResponse?>();

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        reportsResponse.ShouldNotBeNull();
        reportsResponse.Value.PagingResults.Page.ShouldBe(expectedPage);
        reportsResponse.Value.PagingResults.PageSize.ShouldBe(expectedPageSize);
    }

    [TestCase("2", "100", 1, 100, 50, 50)] // edge case
    [TestCase("5", "5", 5, 5, 5, 50)]
    [TestCase("7", "3", 7, 3, 3, 50)]
    [TestCase("21", "4", 13, 4, 2, 50)]
    [TestCase("51", "1", 50, 1, 1, 50)] // edge case
    public async Task Get_Handles_Paging(string page, string pageSize, int expectedPage, int expectedPageSize, int expectedCount, int expectedTotal)
    {
        // arrange
        await Server.ResetDbAsync(async ctx =>
        {
            var reports = TestData.Project1.GenerateNewRisks(50);
            await ctx.Risks.AddRangeAsync(reports);
            await ctx.SaveChangesAsync(CancellationToken.None);
        });

        // act
        using var response = await Client.GetAsync($"/risks?pageSize={pageSize}&page={page}&projectId={TestData.Project1.ProjectId}");
        var reportsResponse = await response.Content.ReadFromJsonAsync<GetRisksResponse?>();

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        reportsResponse.ShouldNotBeNull();
        reportsResponse.Value.PagingResults.Page.ShouldBe(expectedPage);
        reportsResponse.Value.PagingResults.PageSize.ShouldBe(expectedPageSize);
        reportsResponse.Value.Risks.Count().ShouldBe(expectedCount);
        reportsResponse.Value.PagingResults.TotalCount.ShouldBe(expectedTotal);
    }

    [Test]
    public async Task Get_Filters_By_ProjectId()
    {
        // arrange
        await Server.ResetDbAsync(async ctx =>
        {
            var reports = TestData.Project1.GenerateNewRisks(50);
            await ctx.Risks.AddRangeAsync(reports);

            reports = TestData.Project2.GenerateNewRisks(50);
            await ctx.Risks.AddRangeAsync(reports);

            await ctx.SaveChangesAsync(CancellationToken.None);
        });

        // act
        using var response = await Client.GetAsync($"/risks?pageSize=10&page=1&projectId={TestData.Project2.ProjectId}");
        var reportsResponse = await response.Content.ReadFromJsonAsync<GetRisksResponse?>();

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        reportsResponse.ShouldNotBeNull();
        reportsResponse.Value.Risks.All(x => x.ProjectId == TestData.Project2.ProjectId).ShouldBeTrue();
        reportsResponse.Value.PagingResults.Page.ShouldBe(1);
        reportsResponse.Value.PagingResults.PageSize.ShouldBe(10);
        reportsResponse.Value.PagingResults.TotalCount.ShouldBe(50);
    }

    [Test]
    public async Task Get_Returns_Correct_Paging_Info_When_No_Data()
    {
        // act
        using var response = await Client.GetAsync($"/risks?pageSize=10&page=1&projectId={TestData.Project2.ProjectId}");
        var reportsResponse = await response.Content.ReadFromJsonAsync<GetRisksResponse?>();

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        reportsResponse.ShouldNotBeNull();
        reportsResponse.Value.PagingResults.Page.ShouldBe(1);
        reportsResponse.Value.PagingResults.PageSize.ShouldBe(10);
        reportsResponse.Value.PagingResults.TotalCount.ShouldBe(0);
        reportsResponse.Value.PagingResults.First.ShouldBe(0);
        reportsResponse.Value.PagingResults.Last.ShouldBe(0);
    }
}