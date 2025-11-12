using MDF.Test.Integration.Modules.SymbolModuleTest.SymbolStateTest.Responses;
using MDF.Test.Integration.Modules.SymbolModuleTest.SymbolTest.Responses;
using SymbolModule.Contract.SymbolState.Requests;
using System.Net;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.SymbolStateTest.FeaturesTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "symbol-state-paginated-list")]
public partial class SymbolStateGetPaginatedListTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/symbol-state/paginated-list";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public SymbolStateGetPaginatedListTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }



    [Theory]
    [InlineData("05")]
    public async Task Should_not_be_able_get_data_with_not_correct_filter(string? isin)
    {
        //-ARRANGE
        var dto = new SymbolStateGetPaginatedListRequestTest
        {
            SymbolIsin = isin,
            CurrentPage = 1,
            SizeOfPage = 10
        };

        //-ACT
        var response = await _client.GetAsync($"{_api}?{dto.ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.ErrorKey.Should().Be("SymbolStateGetPaginatedListRequestException");
    }




    [Fact]
    public async Task Should_be_able_get_symbol_state()
    {
        //-ARRANGE
        var isin = "IRB00x8585";
        var dt = new DateTime(2025, 11, 09, 15, 28, 0);
        var symbol = await _factory.Repositories.SymbolAddAsync(isin);

        var symbolState = await _factory.Repositories.SymbolStateAddAsync(
                symbolId: symbol.SymbolIdPk,
                stateTypeCode: "A",
                tradingStateCode: "AD",
                stateActionCode: "AC",
                dateOfEvent: dt);

        var dto = new SymbolStateGetPaginatedListRequestTest
        {
            SymbolIsin = isin,
            CurrentPage = 0,
            SizeOfPage = 50
        };

        //-ACT
        var response = await _client.GetAsync($"{_api}?{dto.ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<PaginatedList<SymbolStateResponseTest>>();
        apiResult.Data.Should().NotBeNull();
        apiResult.Data.Should().HaveCount(1);
        var state = apiResult.Data.First();
        state.SymbolStateId.Should().Be(symbolState.SymbolStateIdPk);
        state.SymbolId.Should().Be(symbol.SymbolIdPk);
        state.SymbolIsin.Should().Be(isin);
        state.DateOfEvent.Should().Be(dt.GetISOStringDate());
        state.StateTypeCode.Should().Be("A");
        state.StateTypeTitle.Should().Be("مجاز");
    }


    [Fact]
    public async Task Should_be_able_get_history_symbol_state()
    {
        //-ARRANGE
        var isin = "IRB00x8585";
        var dt = new DateTime(2025, 11, 01, 09, 28, 0);
        var symbol = await _factory.Repositories.SymbolAddAsync(isin, dtCreate: dt, dtEntry: dt);

        await _factory.Repositories.SymbolStateAddAsync(
                symbolId: symbol.SymbolIdPk, dateOfEvent: dt);

        await _factory.Repositories.SymbolStateAddAsync(
            symbolId: symbol.SymbolIdPk, dateOfEvent: dt.AddDays(1));

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
            CurrentPage = 0,
            SizeOfPage = 50
        };

        //-ACT
        var response = await _client.GetAsync($"{_api}?{dto.ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<PaginatedList<SymbolStateResponseTest>>();
        apiResult.Data.Should().NotBeNull();
        apiResult.Data.Should().HaveCount(5);

        var state1 = apiResult.Data.ToList()[0];
        state1.DateOfEvent.Should().Be(dt.AddDays(10).GetISOStringDate());
        state1.StateTypeCode.Should().Be("A");
        state1.StateTypeTitle.Should().Be("مجاز");

        var state2 = apiResult.Data.ToList()[1];
        state2.DateOfEvent.Should().Be(dt.AddDays(5).GetISOStringDate());
        state2.StateTypeCode.Should().Be("AB");
        state2.StateTypeTitle.Should().Be("ممنوع-متوقف");

        var state3 = apiResult.Data.ToList()[2];
        state3.DateOfEvent.Should().Be(dt.AddDays(2).GetISOStringDate());
        state3.StateTypeCode.Should().Be("B");
        state3.StateTypeTitle.Should().Be("ممنوع-متوقف");

        var state4 = apiResult.Data.ToList()[3];
        state4.DateOfEvent.Should().Be(dt.AddDays(1).GetISOStringDate());
        state4.StateTypeCode.Should().Be("A");
        state4.StateTypeTitle.Should().Be("مجاز");

        var state5 = apiResult.Data.ToList()[4];
        state5.DateOfEvent.Should().Be(dt.GetISOStringDate());
        state5.StateTypeCode.Should().Be("A");
        state5.StateTypeTitle.Should().Be("مجاز");

    }



    [Theory]
    [InlineData("IRB00x8585")]
    [InlineData("IRB00x8586")]
    public async Task Should_be_able_get_data_with_special_filter(string isinInFilter)
    {
        //-ARRANGE
        var dt = new DateTime(2025, 11, 11, 09, 28, 0);
        var symbol1 = await _factory.Repositories.SymbolAddAsync("IRB00x8585", dtCreate: dt, dtEntry: dt);
        await _factory.Repositories.SymbolStateAddAsync(symbolId: symbol1.SymbolIdPk, dateOfEvent: dt);


        var symbol2 = await _factory.Repositories.SymbolAddAsync("IRB00x8586", dtCreate: dt, dtEntry: dt);
        await _factory.Repositories.SymbolStateAddAsync(symbolId: symbol2.SymbolIdPk, dateOfEvent: dt);

        var dto = new SymbolStateGetPaginatedListRequestTest
        {
            SymbolIsin = isinInFilter,
        };


        //-ACT
        var response = await _client.GetAsync($"{_api}?{dto.ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<PaginatedList<SymbolStateResponseTest>>();
        apiResult.Data.Should().NotBeNull();

        foreach (var item in apiResult.Data)
        {
            if (!string.IsNullOrWhiteSpace(isinInFilter))
                item.SymbolIsin.Should().Be(isinInFilter);
        }

    }

}
