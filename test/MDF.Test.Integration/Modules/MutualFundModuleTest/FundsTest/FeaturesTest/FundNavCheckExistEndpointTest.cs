using MDF.Test.Integration.Modules.MutualFundModuleTest.Requests.Responses;
using System.Net;

namespace MDF.Test.Integration.Modules.MutualFundModuleTest.FundsTest.FeaturesTest.FundNavCheckExistEndpointTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("FUND", "nav-check-exist")]
public partial class FundNavCheckExistEndpointTest : BaseTest
{
    private readonly string _api = $"/api/v4/fund/nav/check-exist";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public FundNavCheckExistEndpointTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }



    [Fact]
    public async Task Should_be_able_get_fund_data_that_dont_have_any_nav()
    {
        //-ARRANGE
        var fund = await _factory.Repositories.FundAddAsync(seoRegisterNumber: "123", fundProvider: FundProviderTest.Mofid);

        //-ACT
        var response = await _client.GetAsync(_api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var fundResults = await response.Content.ReadModelFromJsonAsync<IEnumerable<FundResponseTest>>();

        fundResults.Should().NotBeNull();
        fundResults.Should().HaveCount(1);

        var fundResult = fundResults.Where(x => x.SeoregisterNumber == "123").FirstOrDefault();
        fundResult.Should().NotBeNull();
        fundResult.FundId.Should().Be(fund.MutualFundIdPk);
        fundResult.SeoregisterNumber.Should().Be(fund.SeoregisterNumber);
        fundResult.DateOfLastRecordNav.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_be_able_get_funds_from_multiProvider_that_dont_have_any_nav()
    {
        //-ARRANGE
        var fundMofid = await _factory.Repositories.FundAddAsync(seoRegisterNumber: "123", fundProvider: FundProviderTest.Mofid);
        var fundTadbir = await _factory.Repositories.FundAddAsync(seoRegisterNumber: "124", fundProvider: FundProviderTest.Tadbir);

        //-ACT
        var response = await _client.GetAsync(_api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var fundResults = await response.Content.ReadModelFromJsonAsync<IEnumerable<FundResponseTest>>();

        fundResults.Should().NotBeNull();
        fundResults.Should().HaveCount(2);

        var fundMofidResult = fundResults.Where(x => x.SeoregisterNumber == "123").FirstOrDefault();
        fundMofidResult.Should().NotBeNull();
        fundMofidResult.FundId.Should().Be(fundMofid.MutualFundIdPk);
        fundMofidResult.SeoregisterNumber.Should().Be(fundMofid.SeoregisterNumber);
        fundMofidResult.DateOfLastRecordNav.Should().BeEmpty();
        fundMofidResult.FundProvider.Should().Be(FundProviderTest.Mofid);

        var fundTadbirResult = fundResults.Where(x => x.SeoregisterNumber == "124").FirstOrDefault();
        fundTadbirResult.Should().NotBeNull();
        fundTadbirResult.FundId.Should().Be(fundTadbir.MutualFundIdPk);
        fundTadbirResult.SeoregisterNumber.Should().Be(fundTadbir.SeoregisterNumber);
        fundTadbirResult.DateOfLastRecordNav.Should().BeEmpty();
        fundTadbirResult.FundProvider.Should().Be(FundProviderTest.Tadbir);

    }


    [Fact]
    public async Task Should_be_able_get_fund_data_that_delay_nav()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 11, 08, 16, 08, 0);
        _client = _factory.WithWebHostBuilder(hos =>
        {
            hos.ConfigureServices(cfg =>
            {
                cfg.SetIDateTimeProvider_Now_Moq(dt);
            });
        }).CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);

        var fund = await _factory.Repositories.FundAddAsync(seoRegisterNumber: "123",
            fundProvider: FundProviderTest.Mofid,
            dtStart: dt.AddDays(-10));
        var nav = await _factory.Repositories.FundNavDailyAddAsync(
            seoRegisterNumber: "123",
            dtFinancial: dt.AddDays(-2),
            dtLastChange: dt.AddDays(-2));

        //-ACT
        var response = await _client.GetAsync(_api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var fundResults = await response.Content.ReadModelFromJsonAsync<IEnumerable<FundResponseTest>>();

        fundResults.Should().NotBeNull();
        fundResults.Should().HaveCount(1);

        var fundResult = fundResults.Where(x => x.SeoregisterNumber == "123").FirstOrDefault();
        fundResult.Should().NotBeNull();
        fundResult.FundId.Should().Be(fund.MutualFundIdPk);
        fundResult.SeoregisterNumber.Should().Be(fund.SeoregisterNumber);
        fundResult.FundProvider.Should().Be(FundProviderTest.Mofid);
        fundResult.DateOfLastRecordNav.Should().Be("2025-11-06");
    }

    [Fact]
    public async Task Should_be_able_get_funds_from_multiProvider_that_delay_nav()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 11, 08, 16, 08, 0);
        _client = _factory.WithWebHostBuilder(hos =>
        {
            hos.ConfigureServices(cfg =>
            {
                cfg.SetIDateTimeProvider_Now_Moq(dt);
            });
        }).CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);

        var fundMofid = await _factory.Repositories.FundAddAsync(seoRegisterNumber: "123",
            fundProvider: FundProviderTest.Mofid,
            dtStart: dt.AddDays(-10));
        var nav = await _factory.Repositories.FundNavDailyAddAsync(
            seoRegisterNumber: "123",
            dtFinancial: dt.AddDays(-2),
            dtLastChange: dt.AddDays(-2));


        var fundTadbir = await _factory.Repositories.FundAddAsync(seoRegisterNumber: "124", fundProvider: FundProviderTest.Tadbir);
        await _factory.Repositories.FundNavDailyAddAsync(
            seoRegisterNumber: "124",
            dtFinancial: dt.AddDays(-2),
            dtLastChange: dt.AddDays(-2));

        //-ACT
        var response = await _client.GetAsync(_api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var fundResults = await response.Content.ReadModelFromJsonAsync<IEnumerable<FundResponseTest>>();

        fundResults.Should().NotBeNull();
        fundResults.Should().HaveCount(2);

        var fundMofidResult = fundResults.Where(x => x.SeoregisterNumber == "123").FirstOrDefault();
        fundMofidResult.Should().NotBeNull();
        fundMofidResult.FundId.Should().Be(fundMofid.MutualFundIdPk);
        fundMofidResult.SeoregisterNumber.Should().Be(fundMofid.SeoregisterNumber);
        fundMofidResult.FundProvider.Should().Be(FundProviderTest.Mofid);
        fundMofidResult.DateOfLastRecordNav.Should().Be("2025-11-06");

        var fundTadbirResult = fundResults.Where(x => x.SeoregisterNumber == "124").FirstOrDefault();
        fundTadbirResult.Should().NotBeNull();
        fundTadbirResult.FundId.Should().Be(fundTadbir.MutualFundIdPk);
        fundTadbirResult.SeoregisterNumber.Should().Be(fundTadbir.SeoregisterNumber);
        fundTadbirResult.FundProvider.Should().Be(FundProviderTest.Tadbir);
        fundTadbirResult.DateOfLastRecordNav.Should().Be("2025-11-06");
    }


    [Fact]
    public async Task Should_not_be_able_get_fund_when_exist_nav()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 11, 08, 16, 08, 0);
        _client = _factory.WithWebHostBuilder(hos =>
        {
            hos.ConfigureServices(cfg =>
            {
                cfg.SetIDateTimeProvider_Now_Moq(dt);
            });
        }).CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);

        var fund = await _factory.Repositories.FundAddAsync(seoRegisterNumber: "123",
            fundProvider: FundProviderTest.Mofid,
            dtStart: dt.AddDays(-10));
        var nav = await _factory.Repositories.FundNavDailyAddAsync(
            seoRegisterNumber: "123",
            dtFinancial: dt,
            dtLastChange: dt);

        //-ACT
        var response = await _client.GetAsync(_api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var fundResults = await response.Content.ReadModelFromJsonAsync<IEnumerable<FundResponseTest>>();

        fundResults.Should().NotBeNull();
        fundResults.Should().HaveCount(0);

    }
}
