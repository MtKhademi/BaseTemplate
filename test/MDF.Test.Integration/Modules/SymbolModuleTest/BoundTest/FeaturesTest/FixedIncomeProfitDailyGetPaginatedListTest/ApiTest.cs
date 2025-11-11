using MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Requests;
using MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Responses;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.FeaturesTest.FixedIncomeProfitDailyGetPaginatedListTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "fixed-income-profit-daily-paginated")]
public partial class FixedIncomeProfitDailyGetPaginatedListTest : BaseTest
{
    private readonly string _apiGetTable = $"/api/v4/symbol/bound/fixed-income/profit-daily/[isin]/paginated";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public FixedIncomeProfitDailyGetPaginatedListTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
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
        var api = _apiGetTable.Replace("[isin]", isin);

        //-ACT
        var response = await _client.GetAsync(api);
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
        var api = _apiGetTable.Replace("[isin]", isin);

        //-ACT
        var response = await _client.GetAsync(api);
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
    public async Task When_not_exist_any_profit_get_empty_table()
    {

        //-ARRANGE
        var isin = "IRB5AE800000";
        var instrument = await _factory.Repositories.InstrumentAddAsync(title: "Test - instrument");
        await _factory.Repositories.SymbolAddAsync(isin: isin, instrumentId: instrument.InstrumentIdPk);
        await _factory.Repositories.FixedIncomeAddAsync(instrumentId: instrument.InstrumentIdPk);
        var api = _apiGetTable.Replace("[isin]", isin);

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var tableDto = await response.Content.ReadModelFromJsonAsync<PaginatedList<FixedIncomeProfitDailyResponseTest>>();
        //- check basic------------------------------------------

    }


    [Theory]
    [InlineData("IRB5AE800000", null, null)]
    [InlineData("IRB5AE800000", "2025-08-18", "2025-08-18")]
    public async Task Should_be_able_get_profit_daily(
        string? isinForSearch = default!,
        string? startDate = default!,
        string? endDate = default!)
    {

        //-ARRANGE
        var isin = "IRB5AE800000";
        var instrument = await _factory.Repositories.InstrumentAddAsync(title: "Test - instrument");
        await _factory.Repositories.SymbolAddAsync(isin: isin, instrumentId: instrument.InstrumentIdPk);
        var fixedIncome = await _factory.Repositories.FixedIncomeAddAsync(instrumentId: instrument.InstrumentIdPk);
        var dt = new DateTime(2025, 08, 17, 12, 36, 0);
        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncomeId: fixedIncome.FixedIncomeIdPk, dtProfitDaily: dt.AddDays(-5), isHasAdjusted: false);
        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncomeId: fixedIncome.FixedIncomeIdPk, dtProfitDaily: dt.AddDays(-4), isHasAdjusted: false);
        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncomeId: fixedIncome.FixedIncomeIdPk, dtProfitDaily: dt.AddDays(-3), isHasAdjusted: false);
        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncomeId: fixedIncome.FixedIncomeIdPk, dtProfitDaily: dt.AddDays(-2), isHasAdjusted: false);
        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncomeId: fixedIncome.FixedIncomeIdPk, dtProfitDaily: dt.AddDays(-1), isHasAdjusted: false);
        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncomeId: fixedIncome.FixedIncomeIdPk, dtProfitDaily: dt.AddDays(0), isHasAdjusted: false);
        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncomeId: fixedIncome.FixedIncomeIdPk, dtProfitDaily: dt.AddDays(1), isHasAdjusted: false);
        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncomeId: fixedIncome.FixedIncomeIdPk, dtProfitDaily: dt.AddDays(2), isHasAdjusted: true);
        var api = _apiGetTable.Replace("[isin]", isinForSearch);

        //-ACT
        var response = await _client.GetAsync($"{api}?{(new FixedIncomeProfitDailyGetPaginatedListRequestTest
        {
            StartDate = startDate,
            EndDate = endDate
        }).ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var paginatedListData = await response.Content.ReadModelFromJsonAsync<PaginatedList<FixedIncomeProfitDailyResponseTest>>();

        foreach (var item in paginatedListData.Data)
        {
            item.Isin.Should().Be(isin);

            if (!string.IsNullOrWhiteSpace(startDate))
            {
                item.ProfitDailyDate.Should().NotBeNull();
                item.ProfitDailyDate.GetDateFromISOFormat().Should().BeOnOrAfter(startDate.GetDateFromISOFormat());
            }

            if(!string.IsNullOrWhiteSpace(endDate))
            {
                item.ProfitDailyDate.Should().NotBeNull();
                item.ProfitDailyDate.GetDateFromISOFormat().Should().BeOnOrBefore(endDate.GetDateFromISOFormat());
            }
        }

    }
}
