using MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Requests;
using MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Responses;
using Microsoft.JSInterop.Implementation;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.FeaturesTest.FixedIncomeProfitDailyGetPerDayTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "fixed-income-profit-daily-paginated")]
public partial class FixedIncomeProfitDailyGetPerDayTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/bound/fixed-income/profit-daily/";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public FixedIncomeProfitDailyGetPerDayTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task When_not_exist_symbol_then_get_not_found()
    {

        //-ARRANGE
        var isin = "IRB5AE800000";

        //-ACT
        var response = await _client.GetAsync(_api + isin + $"?{new FixedIncomeProfitDailyGetPerDayRequestTest
        {
            Date = "2025-09-23"
        }.ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.NotFound);
        apiResult.ErrorKey.Should().Be("SymbolNotExistIsinException");
    }

    [Fact]
    public async Task When_not_exist_FixedIncome_then_get_not_found()
    {

        //-ARRANGE
        var isin = "IRB5AE800000";
        var instrument = await _factory.Repositories.InstrumentAddAsync(title: "Test - instrument");
        await _factory.Repositories.SymbolAddAsync(isin: isin, instrumentId: instrument.InstrumentIdPk);

        //-ACT
        var response = await _client.GetAsync(_api + isin + $"?{new FixedIncomeProfitDailyGetPerDayRequestTest
        {
            Date = "2025-09-23"
        }.ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.NotFound);
        apiResult.ErrorKey.Should().Be("FixedIncomeNotExistWithIsinExceptions");
    }

    [Fact]
    public async Task When_not_exist_any_profit_error()
    {

        //-ARRANGE
        var isin = "IRB5AE800000";
        var instrument = await _factory.Repositories.InstrumentAddAsync(title: "Test - instrument");
        await _factory.Repositories.SymbolAddAsync(isin: isin, instrumentId: instrument.InstrumentIdPk);
        await _factory.Repositories.FixedIncomeAddAsync(instrumentId: instrument.InstrumentIdPk);

        //-ACT
        var response = await _client.GetAsync(_api + isin + $"?{new FixedIncomeProfitDailyGetPerDayRequestTest
        {
            Date = "2025-09-23"
        }.ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var tableDto = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        //- check basic------------------------------------------

    }


    [Fact]
    public async Task Should_be_able_get_profit_daily()
    {

        //-ARRANGE
        var isin = "IRB5AE800000";
        var instrument = await _factory.Repositories.InstrumentAddAsync(title: "Test - instrument");
        await _factory.Repositories.SymbolAddAsync(isin: isin, instrumentId: instrument.InstrumentIdPk);
        var fixedIncome = await _factory.Repositories.FixedIncomeAddAsync(instrumentId: instrument.InstrumentIdPk);
        var dt = new DateTime(2025, 09, 23, 12, 36, 0);
        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncomeId: fixedIncome.FixedIncomeIdPk, dtProfitDaily: dt.AddDays(-5), isHasAdjusted: false);
        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncomeId: fixedIncome.FixedIncomeIdPk, dtProfitDaily: dt.AddDays(-4), isHasAdjusted: false);
        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncomeId: fixedIncome.FixedIncomeIdPk, dtProfitDaily: dt.AddDays(-3), isHasAdjusted: false);
        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncomeId: fixedIncome.FixedIncomeIdPk, dtProfitDaily: dt.AddDays(-2), isHasAdjusted: false);
        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncomeId: fixedIncome.FixedIncomeIdPk, dtProfitDaily: dt.AddDays(-1), isHasAdjusted: false);
        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncomeId: fixedIncome.FixedIncomeIdPk, dtProfitDaily: dt.AddDays(0), isHasAdjusted: false);
        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncomeId: fixedIncome.FixedIncomeIdPk, dtProfitDaily: dt.AddDays(1), isHasAdjusted: false);
        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncomeId: fixedIncome.FixedIncomeIdPk, dtProfitDaily: dt.AddDays(2), isHasAdjusted: true);

        //-ACT
        var response = await _client.GetAsync($"{_api + isin}?{(new FixedIncomeProfitDailyGetPerDayRequestTest
        {
            Date = "2025-09-23",
            Volume = 10
        }).ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var profitResponse = await response.Content.ReadModelFromJsonAsync<FixedIncomeProfitDailyResponseTest>();
        // ASSERT (body snapshot expectation)
        profitResponse.Should().NotBeNull();
        profitResponse.ProfitDailyPrice.Should().Be(10);
        profitResponse.ProfitDailyDate.Should().Be("2025-09-23");
        profitResponse.SubscriptionProfit.Should().Be(10);
        profitResponse.NominalPrice.Should().Be(5);
        profitResponse.HasAdjusted.Should().BeFalse();
        profitResponse.Isin.Should().BeNull(); // per sample json
    }
}
