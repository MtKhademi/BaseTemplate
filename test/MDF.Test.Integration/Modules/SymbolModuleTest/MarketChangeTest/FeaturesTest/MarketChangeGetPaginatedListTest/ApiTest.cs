using MDF.Test.Integration.Modules.SymbolModuleTest.MarketChangeTest.Requests;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.MarketChangeTest.FeaturesTest.MarketChangeGetPaginatedListTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "market-change-paginated-list")]
public partial class MarketChangeGetPaginatedListTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/market-change/paginated-list";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public MarketChangeGetPaginatedListTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task Should_be_able_get_paginatedList_data()
    {
        //-ARRANGE

        //-ACT
        var response = await _client.GetAsync(_api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var paginatedList = await response.Content.ReadModelFromJsonAsync<PaginatedListTest<MarketChangeGetDtoV4Test>>();
        paginatedList.AssertionPaginatedListEmpty();

    }

    [Theory]
    [ClassData(typeof(MarketChangeFilterGetDtoV4NotValidData))]
    public async Task Should_not_be_able_get_data_with_not_correct_filter(MarketChangeGetPaginatedListRequestTest dto, List<string> errors)
    {
        //-ARRANGE
        var api = $"{_api}?{dto.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.ErrorKey.Should().Be("MarketChangeGetPaginatedListRequestException");
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
    }

    [Fact]
    public async Task Should_be_able_data_without_any_filter()
    {
        //-ARRANGE
        await AddRequierAsync();
        var dtoFilter = new MarketChangeGetPaginatedListRequestTest();
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<PaginatedListTest<MarketChangeGetDtoV4Test>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Should().NotBeNull();
    }


    [Fact]
    public async Task Should_be_able_get_firmId_in_oldSymbol()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 11, 09, 09, 49, 00);
        await _factory.Repositories.ChangeMarketAddAsync(
            fromIsin: "IRB1238585", toIsin: "IRB1238586",
            fromFirmId: 1001, toFirmId: 1001,
            dtCreate: dt, dtOpen: dt.AddDays(2), dtClose: dt.AddDays(1));
        var dtoFilter = new MarketChangeGetPaginatedListRequestTest();
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<PaginatedListTest<MarketChangeGetDtoV4Test>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Should().NotBeNull();
        var changeMarket = apiResult.Data.FirstOrDefault(c => c.SymbolOldIsin == "IRB1238585");
        changeMarket.Should().NotBeNull();
        changeMarket.SymbolOldFirmId.Should().Be(1001);
        changeMarket.SymbolNewFirmId.Should().Be(1001);
    }

}
