using MDF.Test.Integration.Modules.SymbolModuleTest.MarketChangeTest.Requests;
using MDF.Test.Integration.Modules.SymbolModuleTest.SymbolStateTest.Responses;
using SymbolModule.Contract.SymbolState.Requests;
using System.Net;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.MarketChangeTest.FeaturesTest.SymbolStateGetUITableTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "symbol-state-ui-table")]
public partial class SymbolStateGetUITableTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/symbol-state/ui-table";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public SymbolStateGetUITableTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task Should_be_able_get_data_for_table_ui_empty()
    {
        //-ARRANGE
        var isin = "IRB00x8585";
        var dt = new DateTime(2025, 11, 01, 09, 28, 0);
        var symbol = await _factory.Repositories.SymbolAddAsync(isin, dtCreate: dt, dtEntry: dt);
        var dto = new SymbolStateGetPaginatedListRequestTest
        {
            SymbolIsin = isin,
        };


        //-ACT
        var response = await _client.GetAsync($"{_api}?{dto.ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var tableDto = await response.Content.ReadModelFromJsonAsync<BaseTableDto<SymbolStateUITableRowResponseTest>>();

        //-- check columns 
        tableDto.Metadata.Details
            .ColumnAssertion(nameof(SymbolStateUITableRowResponseTest.SymbolIsin), ColumnDataType.STRING)
            .ColumnAssertion(nameof(SymbolStateUITableRowResponseTest.SymbolName), ColumnDataType.STRING)
            .ColumnAssertion(nameof(SymbolStateUITableRowResponseTest.StateTypeCode), ColumnDataType.STRING)
            .ColumnAssertion(nameof(SymbolStateUITableRowResponseTest.StateTypeTitle), ColumnDataType.STRING)
            .ColumnAssertion(nameof(SymbolStateUITableRowResponseTest.DateOfEvent), ColumnDataType.DATE_TIME);
            //.ColumnAssertion(nameof(SymbolStateUITableRowResponseTest.Actions), ColumnDataType.LIST);
    }


    [Fact]
    public async Task Should_be_able_get_history_symbol_state()
    {
        //-ARRANGE
        var isin = "IRB00x8585";
        var dt = new DateTime(2025, 11, 01, 09, 28, 0);
        var symbol = await _factory.Repositories.SymbolAddAsync(isin, dtCreate: dt, dtEntry: dt);

        await _factory.Repositories.SymbolStateAddAsync(symbolId: symbol.SymbolIdPk, dateOfEvent: dt);

        await _factory.Repositories.SymbolStateAddAsync(symbolId: symbol.SymbolIdPk, dateOfEvent: dt.AddDays(1));

        await _factory.Repositories.SymbolStateAddAsync(
            symbolId: symbol.SymbolIdPk,
            stateTypeCode: "B",
            stateTypeTitle: "ممنوع-متوقف",
            dateOfEvent: dt.AddDays(2));

        await _factory.Repositories.SymbolStateAddAsync(
            symbolId: symbol.SymbolIdPk,
            stateTypeCode: "AB",
            stateTypeTitle: "ممنوع-متوقف",
            dateOfEvent: dt.AddDays(5));

        await _factory.Repositories.SymbolStateAddAsync(
            symbolId: symbol.SymbolIdPk,
            stateTypeCode: "A",
            stateTypeTitle: "مجاز",
            dateOfEvent: dt.AddDays(10));

        var dto = new SymbolStateGetPaginatedListRequestTest
        {
            SymbolIsin = isin,
        };

        //-ACT
        var response = await _client.GetAsync($"{_api}?{dto.ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<SymbolStateUITableRowResponseTest>>();
        apiResult.Data.Should().NotBeNull();
        apiResult.Data.Content.Should().HaveCount(5);

        var state1 = apiResult.Data.Content.ToList()[0];
        state1.DateOfEvent.Should().Be(dt.AddDays(10).GetISOStringDate());
        state1.StateTypeCode.Should().Be("A");
        state1.StateTypeTitle.Should().Be("مجاز");

        var state2 = apiResult.Data.Content.ToList()[1];
        state2.DateOfEvent.Should().Be(dt.AddDays(5).GetISOStringDate());
        state2.StateTypeCode.Should().Be("AB");
        state2.StateTypeTitle.Should().Be("ممنوع-متوقف");

        var state3 = apiResult.Data.Content.ToList()[2];
        state3.DateOfEvent.Should().Be(dt.AddDays(2).GetISOStringDate());
        state3.StateTypeCode.Should().Be("B");
        state3.StateTypeTitle.Should().Be("ممنوع-متوقف");

        var state4 = apiResult.Data.Content.ToList()[3];
        state4.DateOfEvent.Should().Be(dt.AddDays(1).GetISOStringDate());
        state4.StateTypeCode.Should().Be("A");
        state4.StateTypeTitle.Should().Be("مجاز");

        var state5 = apiResult.Data.Content.ToList()[4];
        state5.DateOfEvent.Should().Be(dt.GetISOStringDate());
        state5.StateTypeCode.Should().Be("A");
        state5.StateTypeTitle.Should().Be("مجاز");

    }

}
