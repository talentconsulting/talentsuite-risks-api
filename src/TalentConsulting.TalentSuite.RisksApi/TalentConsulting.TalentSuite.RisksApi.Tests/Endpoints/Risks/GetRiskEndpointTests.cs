using AutoMapper;
using TalentConsulting.TalentSuite.RisksApi.Common.Dtos;

namespace TalentConsulting.TalentSuite.RisksApi.Tests.Endpoints.Risks;

[TestFixture]
public class GetRiskEndpointTests : ServerFixtureBase
{
    private readonly IMapper _mapper = AutoMapperConfig.Initialize();

    [SetUp]
    public async Task Setup()
    {
        await Server.ResetDbAsync(async ctx => {
            ctx.Risks.AddRange(TestData.Project1.Risks);
            await ctx.SaveChangesAsync(CancellationToken.None);
        });
    }

    [Test]
    public async Task Get_Returns_NotFound()
    {
        // act
        using var response = await Client.GetAsync($"/risks/{Guid.NewGuid()}");

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Get_Returns_Risk()
    {
        // arrange
        var expected = TestData.Project1.Risks.First();

        // act
        using var response = await Client.GetAsync($"/risks/{expected.Id}");
        var actual = await response.Content.ReadFromJsonAsync<RiskDto?>();

        // assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        actual.ShouldBeEquivalentTo(_mapper.Map<RiskDto>(expected));
    }
}