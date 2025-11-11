namespace MDF.Test.Integration.Modules.SymbolModuleTest.MarketChangeTest.FeaturesTest.MarketChangeAddTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait($"SYMBOL", "market-change-add")]
public partial class MarketChangeAddTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/market-change/";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public MarketChangeAddTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }



    [Theory]
    [ClassData(typeof(MarketChangeAddOrUpdateDtoNotValidData))]
    public async Task When_call_api_with_not_valid_data_Expect_get_bad_response(MarketChangeAddOrUpdateDtoV4Test dto, IEnumerable<string> messagesException)
    {
        //-ARRANGE

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.ErrorKey.Should().Be("MarketChangeCreateRequestException");
    }


    [Fact]
    public async Task When_not_exist_isin_Expect_get_not_found_symbol_response()
    {
        //-ARRANGE
        var dto = new MarketChangeAddOrUpdateDtoV4Test()
        {
            FromSymbolCloseDate = "2025-01-10",
            FromSymbolIsin = "IRB5AE800008",
            ToSymbolOpenDate = "2025-01-11",
            ToSymbolIsin = "IRB5AE800009"
        };
        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Assertion(["there is not exist any SymbolEntity => Isin : IRB5AE800008"]);
    }


    [Fact]
    public async Task When_send_valid_data_and_not_exist_already_Expect_add_new_marketChange_and_get_ok_response()
    {
        //-ARRANGE
        var dt = "2025-01-26".ConvertToDateFromMiladiDate(dateSeperator: "-");
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800008");
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800009");
        var dto = new MarketChangeAddOrUpdateDtoV4Test()
        {
            ToSymbolIsin = "IRB5AE800009",
            ToSymbolOpenDate = dt.GetISOStringDateTime(),
            FromSymbolIsin = "IRB5AE800008",
            FromSymbolCloseDate = dt.AddDays(-1).GetISOStringDateTime()
        };


        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var marketChangeInDb = await _factory.Repositories.ChangeMarketGetsync(dto.FromSymbolIsin);
        var symbolFromDb = await _factory.Repositories.SymbolGetByIsinAsync(dto.FromSymbolIsin);
        var symbolToDb = await _factory.Repositories.SymbolGetByIsinAsync(dto.ToSymbolIsin);
        marketChangeInDb.Should().NotBeNull();
        symbolFromDb.Should().NotBeNull();
        symbolToDb.Should().NotBeNull();
        marketChangeInDb!.FromSymbolId.Should().Be(symbolFromDb.SymbolIdPk);
        marketChangeInDb.ToSymbolId.Should().Be(symbolToDb.SymbolIdPk);
        marketChangeInDb.FromSymbolIdCloseDate.HasValue.Should().BeTrue();
        marketChangeInDb.FromSymbolIdCloseDate!.Value.Date.Should().Be(dt.AddDays(-1).Date);
        marketChangeInDb.ToSymbolIdOpenDate.HasValue.Should().BeTrue();
        marketChangeInDb.ToSymbolIdOpenDate!.Value.Date.Should().Be(dt.Date);
        marketChangeInDb.CreateDate.HasValue.Should().BeTrue();
        marketChangeInDb.ChangeDate.HasValue.Should().BeTrue();
        marketChangeInDb.IsFinal.Should().BeFalse();
    }

    [Fact]
    public async Task When_send_valid_data_and_exist_already_get_bad_request()
    {
        //-ARRANGE
        var dt = "2025-01-26".ConvertToDateFromMiladiDate(dateSeperator: "-");
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800008");
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800009");
        var dto = new MarketChangeAddOrUpdateDtoV4Test()
        {
            ToSymbolIsin = "IRB5AE800009",
            ToSymbolOpenDate = dt.GetISOStringDateTime(),
            FromSymbolIsin = "IRB5AE800008",
            FromSymbolCloseDate = dt.AddDays(-1).GetISOStringDateTime()
        };


        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        // send again and add again
        response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Assertion(["Market change from symbol IRB5AE800008 to symbol IRB5AE800009 already exist"]);
    }

}
