using TalentConsulting.TalentSuite.RisksApi.Db.Entities;

namespace TalentConsulting.TalentSuite.RisksApi.Tests.Endpoints.Risks;

[TestFixture]
public class DeleteRiskEndpointTests : ServerFixtureBase
{
    [SetUp]
    public async Task Setup()
    {
        await Server.ResetDbAsync(async ctx => {
            ctx.Risks.AddRange(TestData.Project1.Risks);
            await ctx.SaveChangesAsync(CancellationToken.None);
        });
    }

    [Test]
    public async Task Delete_Returns_NotFound()
    {
        // act
        using var response = await Client.DeleteAsync($"/reports/{Guid.NewGuid()}");

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Delete_Removes_Risk_From_DataStore()
    {
        // arrange
        var risk = TestData.Project1.Risks.First();
        Risk? target = new ();

        // act
        using var deleteResponse = await Client.DeleteAsync($"/risks/{risk.Id}");
        await Server.QueryDbAsync(async ctx => target = await ctx.Risks.FindAsync(risk.Id));

        // assert
        deleteResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);
        target.ShouldBeNull();
    }
}