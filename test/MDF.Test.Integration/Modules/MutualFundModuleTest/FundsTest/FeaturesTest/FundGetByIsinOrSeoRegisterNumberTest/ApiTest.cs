using MDF.Test.Integration.Modules.MutualFundModuleTest.Requests.Responses;

namespace MDF.Test.Integration.Modules.MutualFundModuleTest.FundsTest.FeaturesTest.FundGetByIsinOrSeoRegisterNumber;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("FUND", "get-by-isin-or-seoRegisterNumber")]
public class FundGetByIsinOrSeoRegisterNumber : BaseTest
{
    private readonly string _apiAddress = "api/v4/fund";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public FundGetByIsinOrSeoRegisterNumber(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);
        _outPutHelper = outPutHelper;
        var scope = factory.Services.CreateScope();
    }



    [Fact]
    public async Task Should_not_be_able_get_fund_when_not_exist_isin()
    {
        //-ARRANGE

        //-ACT
        var response = await _client.GetAsync($"{_apiAddress}/123");
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Assertion(["there is not exist any Fund =\u003E isin-or-seoRegisterNumber : 123"]);
    }


    [Fact]
    public async Task Should_be_able_get_fund_when_exist_isin()
    {
        //-ARRANGE
        var dtStart = new DateTime(2025, 04, 28, 09, 51, 0);
        await _factory.Repositories.FundAddAsync(
            seoRegisterNumber: "seo-123",
            fundIsin: "123",
            title: "fund-title sample",
            fundProvider: FundProviderTest.Mofid,
            fundType: FundTypeTest.StockEtf,
            dtStart: dtStart);

        //-ACT
        var response = await _client.GetAsync($"{_apiAddress}/123");
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<FundResponseTest>();
        apiResult.Should().NotBeNull();
        apiResult.SeoregisterNumber.Should().Be("seo-123");
        apiResult.Title.Should().Be("fund-title sample");
        apiResult.Isin.Should().Be("123");
        apiResult.FundType.Should().Be(FundTypeTest.StockEtf);
        apiResult.FundProvider.Should().Be(FundProviderTest.Mofid);
        apiResult.DateStart.Should().Be(dtStart.GetISOStringDate());

    }

    [Fact]
    public async Task Should_be_able_get_fund_with_orgnaization_data()
    {
        //-ARRANGE
        var dtStart = new DateTime(2025, 04, 28, 09, 51, 0);
        await _factory.Repositories.OrganizationAddAsync(2);
        await _factory.Repositories.FundAddAsync(
            seoRegisterNumber: "seo-123",
            fundIsin: "123",
            title: "fund-title sample",
            fundProvider: FundProviderTest.Mofid,
            fundType: FundTypeTest.StockEtf,
            dtStart: dtStart,
            organizationIdFk: 1, managerOrganizationIdFk: 2);

        //-ACT
        var response = await _client.GetAsync($"{_apiAddress}/123");
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<FundResponseTest>();
        apiResult.Should().NotBeNull();
        apiResult.SeoregisterNumber.Should().Be("seo-123");
        apiResult.Title.Should().Be("fund-title sample");
        apiResult.Isin.Should().Be("123");
        apiResult.FundType.Should().Be(FundTypeTest.StockEtf);
        apiResult.FundProvider.Should().Be(FundProviderTest.Mofid);
        apiResult.DateStart.Should().Be(dtStart.GetISOStringDate());
        apiResult.OrganizationId.Should().Be(1);
        apiResult.OrganizationName.Should().Be("وزارت امور اقتصادی و دارایی");
        apiResult.ManagerId.Should().Be(2);
        apiResult.ManagerName.Should().Be("Organization Title");

    }


    [Fact]
    public async Task Should_be_able_get_fund_when_exist_seoRegisterNumber()
    {
        //-ARRANGE
        await _factory.Repositories.OrganizationAddAsync(2);
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "seo-123", fundIsin: "123",
            managerOrganizationIdFk: 2, organizationIdFk: 1);

        //-ACT
        var response = await _client.GetAsync($"{_apiAddress}/seo-123");
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<FundResponseTest>();
        apiResult.Should().NotBeNull();
        apiResult.SeoregisterNumber.Should().Be("seo-123");
        apiResult.Title.Should().Be("Fund Title Default");
        apiResult.Isin.Should().Be("123");
        apiResult.OrganizationId.Should().Be(1);
        apiResult.OrganizationName.Should().Be("وزارت امور اقتصادی و دارایی");
        apiResult.ManagerId.Should().Be(2);
        apiResult.ManagerName.Should().Be("Organization Title");

    }

    [Fact]
    public async Task Should_be_able_get_fund_with_fundFee_data()
    {
        //-ARRANGE
        await _factory.Repositories.OrganizationAddAsync(2);
        var fundEntity = await _factory.Repositories.FundAddAsync(seoRegisterNumber: "seo-123", fundIsin: "123",
              managerOrganizationIdFk: 2, organizationIdFk: 1);

        var dt = new DateTime(2025, 09, 29);
        await _factory.Repositories.FundFeeAddAsync(mutualFundIdFk: fundEntity.MutualFundIdPk, eventDate: dt);
        await _factory.Repositories.FundFeeAddAsync(mutualFundIdFk: fundEntity.MutualFundIdPk, eventDate: dt.AddDays(-1));
        await _factory.Repositories.FundFeeAddAsync(mutualFundIdFk: fundEntity.MutualFundIdPk, eventDate: dt.AddDays(-2));


        //-ACT
        var response = await _client.GetAsync($"{_apiAddress}/seo-123");
        _outPutHelper.WriteLine(await response.Content.ReadAsStringAsync());

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<FundResponseTest>();
        apiResult.Should().NotBeNull();
        apiResult.SeoregisterNumber.Should().Be("seo-123");
        apiResult.Title.Should().Be("Fund Title Default");
        apiResult.Isin.Should().Be("123");
        apiResult.OrganizationId.Should().Be(1);
        apiResult.OrganizationName.Should().Be("وزارت امور اقتصادی و دارایی");
        apiResult.ManagerId.Should().Be(2);
        apiResult.ManagerName.Should().Be("Organization Title");

        apiResult.FundFees.Should().NotBeNull();
        apiResult.FundFees.Should().HaveCount(1);
        apiResult.FundFees.First().EventDate.Should().Be(dt.GetISOStringDate());

    }
}
