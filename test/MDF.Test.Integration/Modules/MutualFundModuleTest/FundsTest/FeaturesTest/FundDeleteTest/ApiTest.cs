namespace MDF.Test.Integration.Modules.MutualFundModuleTest.FundsTest.FeaturesTest.FundDeleteTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("FUND", "delete")]
public partial class FundDeleteTest : BaseTest
{
    private readonly string _api = $"/api/v4/fund";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public FundDeleteTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task Should_not_be_able_delete_a_fund_when_not_exist()
    {
        //-ARRANGE

        //-ACT
        var response = await _client.DeleteAsync(_api + "/1258585");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var fundResponse = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
    }


    [Fact]
    public async Task Should_be_able_delete_a_fund()
    {
        //-ARRANGE
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "123123");


        //-ACT
        var response = await _client.DeleteAsync(_api + "/123123");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        response = await _client.GetAsync(_api + "/123123");
        await response.WriteOnConsoleAsync(_outPutHelper);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

    }

    [Fact]
    public async Task Should_not_be_able_delete_a_fund_when_have_some_FundNavs()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 07, 30, 10, 01, 0);
        var seoRegisterNumber = "123123";
        var fund = await _factory.Repositories.FundAddAsync(seoRegisterNumber);
        await _factory.Repositories.FundNavDailyAddAsync(seoRegisterNumber, dtFinancial: dt.AddDays(-1));
        await _factory.Repositories.FundNavDailyAddAsync(seoRegisterNumber, dtFinancial: dt);
        await _factory.Repositories.FundNavDailyAddAsync(seoRegisterNumber, dtFinancial: dt.AddDays(1));


        //-ACT
        var response = await _client.DeleteAsync(_api + "/123123");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Messages.Should().Contain("This fund have some navs, you can't delete it");
    }
}
