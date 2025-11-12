namespace MDF.Test.Integration.Modules.SymbolModuleTest.MarketChangeTest.FeaturesTest.MarketChangesConfirmTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait($"SYMBOL", "market-change-confirm")]
public partial class MarketChangesConfirmTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/market-change/";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public MarketChangesConfirmTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
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
        var allChanges = await _factory.Repositories.ChangeMarketGetssync();
        allChanges.Should().BeEmpty();

        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var marketChangeInDb = await _factory.Repositories.ChangeMarketGetsync(dto.FromSymbolIsin);
        marketChangeInDb.Should().NotBeNull();
        marketChangeInDb.IsFinal.Should().BeFalse();

        //-ACT
        response = await _client.PutAsync($"{_api}confirm", null);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        marketChangeInDb = await _factory.Repositories.ChangeMarketGetsync(dto.FromSymbolIsin);
        marketChangeInDb.Should().NotBeNull();
        marketChangeInDb.IsFinal.Should().BeTrue();
    }

}
