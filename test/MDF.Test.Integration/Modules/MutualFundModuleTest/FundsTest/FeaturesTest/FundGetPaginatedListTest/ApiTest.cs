using MassTransit.Internals.Caching;
using MDF.Test.Integration.Modules.MutualFundModuleTest.Requests.Responses;
using System.Net;

namespace MDF.Test.Integration.Modules.MutualFundModuleTest.FundsTest.FeaturesTest.FundGetPaginatedListTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("FUND", "get-paginated")]
public partial class FundGetPaginatedListTest : BaseTest
{
    private readonly string _api = $"/api/v4/fund/paginated";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public FundGetPaginatedListTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task Should_be_able_get_data_empty()
    {
        //-ARRANGE

        //-ACT
        var response = await _client.GetAsync(_api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_be_able_get_data_withou_any_filter()
    {
        //-ARRANGE
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "123", fundType: FundTypeTest.Leverage, fundXMLType: FundXMLTypeTest.StockEtf);
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "124");
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "125");

        //-ACT
        var response = await _client.GetAsync(_api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var paginatedResult = await response.Content.ReadModelFromJsonAsync<PaginatedListTest<FundResponseTest>>();
        paginatedResult.Should().NotBeNull();
        paginatedResult.Data.Should().NotBeNull();
        paginatedResult.Data.Count().Should().Be(3);
        paginatedResult.Data.ToList()[2].SeoregisterNumber.Should().Be("123");
        paginatedResult.Data.ToList()[1].SeoregisterNumber.Should().Be("124");
        paginatedResult.Data.ToList()[0].SeoregisterNumber.Should().Be("125");

        var fund = paginatedResult.Data.Where(x => x.SeoregisterNumber == "123").FirstOrDefault();
        fund.Should().NotBeNull();
        fund.FundType.Should().Be(FundTypeTest.Leverage);
        fund.FundXMLType.Should().Be(FundXMLTypeTest.StockEtf);
    }

    [Theory]
    [InlineData("123", null, null, null)]
    [InlineData(null, FundProviderTest.Mofid, null, null)]
    [InlineData(null, FundProviderTest.Mofid, "IRO101271", null)]
    [InlineData(null, null, "IRO101271", null)]
    [InlineData(null, null, "IRO101231", null)]
    [InlineData(null, FundProviderTest.Tadbir, "IRO101231", null)]
    [InlineData(null, null, null, "IRB8585")]
    [InlineData(null, FundProviderTest.Mofid, null, "IRB8586")]
    [InlineData(null, null, "IRO101271", "IRB8587")]
    [InlineData(null, null, null, null, "check title")]
    [InlineData(null, null, null, null, "یه عنوان")]
    [InlineData(null, null, null, null, null, "2025-09-28", "2025-09-29")]
    public async Task Should_be_able_get_data_with_custom_filter(
        string? seoRegisterNumber = null,
        FundProviderTest? fundProvider = null,
        string? isin = null,
        string? symbolIsin = null,
        string? title = null,
        string? FromLastChangeFundDate = default!,
        string? ToLastChangeFundDate = default!)
    {

        var dtCreate = new DateTime(2025, 09, 20);
        //-ARRANGE
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "123", fundIsin: "IRO101231",
            fundProvider: FundProviderTest.Tadbir,
            dtLastChange: dtCreate);
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "124", dtLastChange: dtCreate);
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "125", dtLastChange: dtCreate);
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "126", dtLastChange: dtCreate, fundProvider: FundProviderTest.Mofid);
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "127", dtLastChange: dtCreate, fundIsin: "IRO101271", fundProvider: FundProviderTest.Mofid);
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "8585", dtLastChange: dtCreate, symbolIsin: "IRB8585");
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "8586", dtLastChange: dtCreate, symbolIsin: "IRB8586", fundProvider: FundProviderTest.Mofid);
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "8587", dtLastChange: dtCreate, symbolIsin: "IRB8587", fundProvider: FundProviderTest.Mofid, fundIsin: "IRO101271");
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "8588", dtLastChange: dtCreate, title: "این یه عنوان است");
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "8588", dtLastChange: dtCreate, title: "this is for check title");

        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "8589", dtLastChange: new DateTime(2025, 09, 28), title: "this is for check title");
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "8590", dtLastChange: new DateTime(2025, 09, 29), title: "this is for check title");



        //-ACT
        var response = await _client.GetAsync($"{_api}?" +
            $"{new FundFilterGetPaginatedListDtoTest
            {
                SymbolIsin = symbolIsin,
                Title = title,
                FundIsin = isin,
                SeoregisterNumber = seoRegisterNumber,
                FundProvider = fundProvider,
                FromLastChangeFundDate = FromLastChangeFundDate,
                ToLastChangeFundData = ToLastChangeFundDate,
            }.ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var paginatedResult = await response.Content.ReadModelFromJsonAsync<PaginatedListTest<FundResponseTest>>();
        paginatedResult.Should().NotBeNull();
        paginatedResult.Data.Should().NotBeNull();
        paginatedResult.Data.Count().Should().BeGreaterThanOrEqualTo(1);

        foreach (var fund in paginatedResult.Data)
        {
            if (!string.IsNullOrWhiteSpace(seoRegisterNumber))
                fund.SeoregisterNumber.Should().Be(seoRegisterNumber);

            if (fundProvider.HasValue)
            {
                fund.FundProvider.HasValue.Should().BeTrue();
                ((int)fund.FundProvider).Should().Be(((int)fundProvider.Value));

            }

            if (!string.IsNullOrWhiteSpace(isin))
                fund.Isin.Should().Be(isin);

            if (!string.IsNullOrEmpty(symbolIsin))
                fund.SymbolIsin.Should().Be(symbolIsin);

            if (!string.IsNullOrWhiteSpace(title))
            {
                fund.Title.Should().NotBeNullOrWhiteSpace();
                fund.Title.Should().Contain(title);
            }

            if (!string.IsNullOrWhiteSpace(FromLastChangeFundDate))
            {
                fund.DateLastChanged.Should().NotBeNull();

                var dt = FromLastChangeFundDate.GetDateFromISOFormat();
                var dtItem = fund.DateLastChanged.GetDateFromISOFormat();

                dtItem.Should().BeOnOrAfter(dt);
            }

            if (!string.IsNullOrWhiteSpace(ToLastChangeFundDate))
            {
                fund.DateLastChanged.Should().NotBeNull();

                var dt = ToLastChangeFundDate.GetDateFromISOFormat();
                var dtItem = fund.DateLastChanged.GetDateFromISOFormat();

                dtItem.Should().BeOnOrBefore(dt);
            }
        }
    }

}
