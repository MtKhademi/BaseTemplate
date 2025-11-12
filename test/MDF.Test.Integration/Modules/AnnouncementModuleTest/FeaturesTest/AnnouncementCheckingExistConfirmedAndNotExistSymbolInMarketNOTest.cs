namespace MDF.Test.Integration.Modules.AnnouncementModuleTest.FeaturesTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("ANNOUNCEMENT", "checking-exist-confirmed-and-not-exist-symbol-in-marketNO")]
public partial class AnnouncementCheckingExistConfirmedAndNotExistSymbolInMarketNOTest : BaseTest
{
    private readonly string _api = $"/api/v4/announcement/checking-exist-confirmed-and-not-exist-symbol-in-marketNO";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public AnnouncementCheckingExistConfirmedAndNotExistSymbolInMarketNOTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }

    [Theory]
    [InlineData("2025")]
    public async Task Should_not_be_able_get_data_with_not_correct_filter(string? date)
    {
        //-ARRANGE
        var api = $"{_api}/{date}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
    }


    //سناریو :‌
    // مجمعی هست که تایید شده است ولی به هیچ نمادی که در بازار اول هست متصل نشده است . 
    [Fact]
    public async Task Should_be_able_get_announcementCodalCode_that_dont_have_any_symbol_in_marketNO()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 11, 05, 09, 30, 0);
        var exchangeMarket = await _factory.Repositories
            .MarketExchangeAddAsync(market: new DAL.Modules.Entities.OldEntities.Market
            {
                Code = "COM",
                EnTitle = "Second Market",
                Title = "بازار دوم"
            }, securitiesExchange: new DAL.Modules.Entities.OldEntities.SecuritiesExchange
            {
                Title = "Test - exchange",
            });
        var symbol = await _factory.Repositories.SymbolAddAsync(isin: "IRB858581", firmId: 1,
            typeOfSymbol: TypeOfSymbolTest.Stock, exchangeMarketId: exchangeMarket.ExchangeMarketIdPk);

        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: 8585, firmId: 1, dtPublish: dt);
        var dividend = await _factory.Repositories.DividendPerShareAddAsync(announcementId: announcement.AnnouncementIdPk,
            dtEntry: dt, dtModify: dt, isConfirm: true);

        var api = $"{_api}/{dt.GetISOStringDate()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<IEnumerable<string>>();
        apiResult.Should().NotBeNull();
        apiResult.Should().HaveCount(1);
        apiResult.Should().Contain(announcement.Code.ToString());
    }


    // سناریو :‌
    // مجمع تایید شده است و به نمادی متصل است که در بازار اول هست
    [Fact]
    public async Task When_all_announcement_have_symbol_that_in_marketNO_get_any_data()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 11, 05, 09, 30, 0);
        var exchangeMarket = await _factory.Repositories
            .MarketExchangeAddAsync(market: new DAL.Modules.Entities.OldEntities.Market
            {
                Code = "NO",
                EnTitle = "First Market",
                Title = "بازار اول"
            }, securitiesExchange: new DAL.Modules.Entities.OldEntities.SecuritiesExchange
            {
                Title = "Test - exchange",
            });
        var symbol = await _factory.Repositories.SymbolAddAsync(isin: "IRB858581", firmId: 1,
            typeOfSymbol: TypeOfSymbolTest.Stock, exchangeMarketId: exchangeMarket.ExchangeMarketIdPk);
        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: 8585, firmId: 1, dtPublish: dt);
        var dividend = await _factory.Repositories.DividendPerShareAddAsync(announcementId: announcement.AnnouncementIdPk,
            dtEntry: dt, dtModify: dt, isConfirm: true);

        var api = $"{_api}/{dt.GetISOStringDate()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<IEnumerable<string>>();
        apiResult.Should().NotBeNull();
        apiResult.Should().HaveCount(0);
    }
}
