namespace MDF.Test.Integration.Modules.SymbolModuleTest.MarketChangeTest.FeaturesTest.MarketChangeUpdateTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait($"SYMBOL", "market-change-update")]
public partial class MarketChangeUpdateTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/market-change/";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public MarketChangeUpdateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }



    [Theory]
    [ClassData(typeof(MarketChangeAddOrUpdateDtoNotValidData))]
    public async Task When_call_api_with_not_valid_data_Expect_get_bad_response(MarketChangeUpdateDtoV4Test dto, IEnumerable<string> messagesException)
    {
        //-ARRANGE

        //-ACT
        var response = await _client.PutAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.IsSuccess.Should().Be(false);
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
        apiResult.ErrorKey.Should().Be("MarketChangeUpdateRequestException");
    }


    [Fact]
    public async Task When_not_exist_isin_Expect_get_not_found_symbol_response()
    {
        //-ARRANGE
        var dto = new MarketChangeUpdateDtoV4Test()
        {
            FromSymbolCloseDate = "2025-01-10",
            FromSymbolIsin = "IRB5AE800008",
            ToSymbolOpenDate = "2025-01-11",
            ToSymbolIsin = "IRB5AE800009"
        };
        //-ACT
        var response = await _client.PutAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Assertion(["there is not exist any SymbolEntity => Isin : IRB5AE800008"]);
    }


    [Fact]
    public async Task When_send_valid_data_and_not_exist_already_get_bad_request()
    {
        //-ARRANGE
        var dt = "2025-01-26".ConvertToDateFromMiladiDate(dateSeperator: "-");
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800008");
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800009");
        var dto = new MarketChangeUpdateDtoV4Test()
        {
            ToSymbolIsin = "IRB5AE800009",
            ToSymbolOpenDate = dt.GetISOStringDateTime(),
            FromSymbolIsin = "IRB5AE800008",
            FromSymbolCloseDate = dt.AddDays(-1).GetISOStringDateTime()
        };


        //-ACT
        var response = await _client.PutAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.ErrorKey.Should().Be("MarketChangeNotExistException");
    }

    [Fact]
    public async Task When_send_valid_data_and_exist_already_Expect_update_data()
    {
        //-ARRANGE
        var dt = "2025-01-26".ConvertToDateFromMiladiDate(dateSeperator: "-");
        await _factory.Repositories.ChangeMarketAddAsync(fromIsin: "IRB5AE800008", toIsin: "IRB5AE800009", dt);
        var dto = new MarketChangeUpdateDtoV4Test()
        {
            ToSymbolIsin = "IRB5AE800009",
            ToSymbolOpenDate = dt.AddDays(-1).GetISOStringDateTime(),
            FromSymbolIsin = "IRB5AE800008",
            FromSymbolCloseDate = dt.AddDays(2).GetISOStringDateTime()
        };


        //-ACT
        var response = await _client.PutAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var marketChangeDb = await _factory.Repositories.ChangeMarketGetsync(fromIsin: "IRB5AE800008");
        marketChangeDb.Should().NotBeNull();
        marketChangeDb.ToSymbolIdOpenDate.Should().Be(dt.AddDays(-1));
        marketChangeDb.FromSymbolIdCloseDate.Should().Be(dt.AddDays(2));

    }

}
