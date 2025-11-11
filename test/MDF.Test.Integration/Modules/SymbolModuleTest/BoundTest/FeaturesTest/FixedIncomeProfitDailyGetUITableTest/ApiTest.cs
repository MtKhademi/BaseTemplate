using MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Responses;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.FeaturesTest.FixedIncomeProfitDailyGetUITableTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "fixed-income-profit-daily-ui-table")]
public partial class FixedIncomeProfitDailyGetUITableTest : BaseTest
{
    private readonly string _apiGetTable = $"/api/v4/symbol/bound/fixed-income/profit-daily/[isin]/ui-table/";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public FixedIncomeProfitDailyGetUITableTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
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
        var tableDto = await response.Content.ReadModelFromJsonAsync<BaseTableDto<FixedIncomeProfitDailyUITableRowResponseTest>>();
        //- check basic------------------------------------------
        tableDto.Metadata.Basic.PersianTitle.Should().NotBeNullOrWhiteSpace();
        tableDto.Metadata.Basic.HasRowNumber.Should().BeTrue();
        tableDto.Data.Should().NotBeNull();
        tableDto.Data.Pageable.Should().NotBeNull();
        tableDto.Data.Content.Should().NotBeNull();
        tableDto.Metadata.Details.Should().NotBeNull();

        //-- check columns 
        tableDto.Metadata.Details
            .ColumnAssertion(nameof(FixedIncomeProfitDailyUITableRowResponseTest.ProfitDailyPrice), ColumnDataType.INT)
            .ColumnAssertion(nameof(FixedIncomeProfitDailyUITableRowResponseTest.ProfitDailyDate), ColumnDataType.DATE_TIME)
            .ColumnAssertion(nameof(FixedIncomeProfitDailyUITableRowResponseTest.ModifiedDate), ColumnDataType.DATE_TIME)
            .ColumnAssertion(nameof(FixedIncomeProfitDailyUITableRowResponseTest.SubscriptionProfit), ColumnDataType.INT)
            .ColumnAssertion(nameof(FixedIncomeProfitDailyUITableRowResponseTest.NominalPrice), ColumnDataType.INT)
            .ColumnAssertion(nameof(FixedIncomeProfitDailyUITableRowResponseTest.HasAdjusted), ColumnDataType.BOOLEAN);

        tableDto.Data.Pageable.PageNumber.Should().Be(0);
        tableDto.Data.Pageable.PageSize.Should().Be(100);

    }


    [Fact]
    public async Task Should_be_able_get_profit_daily()
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
        var api = _apiGetTable.Replace("[isin]", isin);

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var tableDto = await response.Content.ReadModelFromJsonAsync<BaseTableDto<FixedIncomeProfitDailyUITableRowResponseTest>>();
        tableDto.Data.Content.Should().NotBeNull();
        tableDto.Data.Content.Should().HaveCount(8);
        foreach (var row in tableDto.Data.Content)
        {
            row.ProfitDailyPrice.Should().Be(10);
            row.SubscriptionProfit.Should().Be(10);
            row.NominalPrice.Should().Be(5);
        }

    }

}
