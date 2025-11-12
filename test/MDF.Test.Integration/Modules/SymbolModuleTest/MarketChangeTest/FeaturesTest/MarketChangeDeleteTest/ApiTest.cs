using MDF.Test.Integration.Modules.SymbolModuleTest.MarketChangeTest.Requests;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.MarketChangeTest.FeaturesTest.MarketChangeDeleteTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait($"SYMBOL", "market-change-delete")]
public partial class MarketChangeDeleteTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/market-change/";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public MarketChangeDeleteTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }



    [Theory]
    [InlineData("", "")]
    [InlineData("", null)]
    [InlineData(null, null)]
    [InlineData(null, "")]
    public async Task When_call_api_with_not_valid_data_Expect_get_bad_response(string fromSymbolIsin, string toSymbolIsin)
    {
        //-ARRANGE
        var dto = new MarketChangeDeleteRequestTest(fromSymbolIsin, toSymbolIsin);
        //-ACT
        var response = await _client.DeleteAsync($"{_api}?{dto.ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.IsSuccess.Should().Be(false);
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
        apiResult.ErrorKey.Should().Be("MarketChangeDeleteRequestException");
    }


    [Fact]
    public async Task When_not_exist_fromIsin_Expect_get_not_found()
    {
        //-ARRANGE
        var dto = new MarketChangeDeleteRequestTest("IRB5AE800008", "IRB5AE800009");

        //-ACT
        var response = await _client.DeleteAsync($"{_api}?{dto.ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.IsSuccess.Should().Be(false);
        apiResult.ErrorKey.Should().Be("SymbolNotExistIsinException");
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.NotFound);
    }

    [Fact]
    public async Task When_not_exist_toIsin_Expect_get_not_found()
    {
        //-ARRANGE
        var fromIsin = "IRB5AE800008";
        await _factory.Repositories.SymbolAddAsync(isin: fromIsin);
        var toIsin = "IRB5AE800009";
        var dto = new MarketChangeDeleteRequestTest(fromIsin, toIsin);

        //-ACT
        var response = await _client.DeleteAsync($"{_api}?{dto.ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.IsSuccess.Should().Be(false);
        apiResult.ErrorKey.Should().Be("SymbolNotExistIsinException");
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.NotFound);
    }


    [Fact]
    public async Task When_send_valid_data_but_not_marketChange_get_notFound()
    {
        //-ARRANGE
        var fromIsin = "IRB5AE800008";
        await _factory.Repositories.SymbolAddAsync(isin: fromIsin);
        var toIsin = "IRB5AE800009";
        await _factory.Repositories.SymbolAddAsync(isin: toIsin);
        var dto = new MarketChangeDeleteRequestTest(fromIsin, toIsin);

        //-ACT
        var response = await _client.DeleteAsync($"{_api}?{dto.ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.IsSuccess.Should().Be(false);
        apiResult.ErrorKey.Should().Be("MarketChangeNotExistException");
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.NotFound);
    }


    [Fact]
    public async Task When_send_valid_data_and_exist_then_delete()
    {
        //-ARRANGE
        var fromIsin = "IRB5AE800008";
        await _factory.Repositories.SymbolAddAsync(isin: fromIsin);
        var toIsin = "IRB5AE800009";
        await _factory.Repositories.SymbolAddAsync(isin: toIsin);

        await _factory.Repositories.ChangeMarketAddAsync(fromIsin, toIsin, DateTime.Now);
        var dto = new MarketChangeDeleteRequestTest(fromIsin, toIsin);

        //-ACT
        var response = await _client.DeleteAsync($"{_api}?{dto.ToQueryString()}");

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        // check not exist any more
        var responseGetPaginatedList = await _client.GetAsync($"/api/v4/symbol/market-change/paginated-list");
        var apiResultGetPaginatedList = await responseGetPaginatedList.Content.ReadModelFromJsonAsync<PaginatedListTest<MarketChangeGetDtoV4Test>>();
        apiResultGetPaginatedList.Data.Should().HaveCount(0);
    }


    [Fact]
    public async Task Should_be_able_delete_one_without_change_old_data()
    {
        //-ARRANGE
        var fromIsin = "IRB5AE800008";
        var toIsin = "IRB5AE800009";
        await _factory.Repositories.SymbolAddAsync(isin: fromIsin);
        await _factory.Repositories.SymbolAddAsync(isin: toIsin);
        await _factory.Repositories.ChangeMarketAddAsync(fromIsin, toIsin, DateTime.Now.AddDays(-10));


        fromIsin = "IRB5AE800010";
        toIsin = "IRB5AE800011";
        await _factory.Repositories.SymbolAddAsync(isin: fromIsin);
        await _factory.Repositories.SymbolAddAsync(isin: toIsin);
        await _factory.Repositories.ChangeMarketAddAsync(fromIsin, toIsin, DateTime.Now.AddYears(-2));

        fromIsin = "IRB5AE800012";
        toIsin = "IRB5AE800013";
        await _factory.Repositories.SymbolAddAsync(isin: fromIsin);
        await _factory.Repositories.SymbolAddAsync(isin: toIsin);
        await _factory.Repositories.ChangeMarketAddAsync(fromIsin, toIsin, DateTime.Now);

        var dto = new MarketChangeDeleteRequestTest(fromIsin, toIsin);

        //-ACT
        var response = await _client.DeleteAsync($"{_api}?{dto.ToQueryString()}");

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        // check not exist any more
        var responseGetPaginatedList = await _client.GetAsync($"/api/v4/symbol/market-change/paginated-list");
        var apiResultGetPaginatedList = await responseGetPaginatedList.Content.ReadModelFromJsonAsync<PaginatedListTest<MarketChangeGetDtoV4Test>>();
        apiResultGetPaginatedList.Data.Should().HaveCount(2);
    }
}
