using MDF.Test.Integration.Modules.SymbolModuleTest.MarketChangeTest.Requests;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.MarketChangeTest.FeaturesTest.MarketChangeGetUITableTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "market-change-ui-table")]
public partial class MarketChangeGetUITableTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/market-change/ui-table";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public MarketChangeGetUITableTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
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

        //-ACT
        var response = await _client.GetAsync(_api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var tableDto = await response.Content.ReadModelFromJsonAsync<BaseTableDto<MarketChangeUITableRowResponseTest>>();

        //-- check columns 
        tableDto.Metadata.Details
            .ColumnAssertion(nameof(MarketChangeUITableRowResponseTest.SymbolOldName), ColumnDataType.STRING)
            .ColumnAssertion(nameof(MarketChangeUITableRowResponseTest.SymbolOldIsin), ColumnDataType.STRING)
            .ColumnAssertion(nameof(MarketChangeUITableRowResponseTest.SymbolOldMarketName), ColumnDataType.STRING)
            .ColumnAssertion(nameof(MarketChangeUITableRowResponseTest.SymbolOldCloseDate), ColumnDataType.DATE_TIME)
            .ColumnAssertion(nameof(MarketChangeUITableRowResponseTest.SymbolNewName), ColumnDataType.STRING)
            .ColumnAssertion(nameof(MarketChangeUITableRowResponseTest.SymbolNewIsin), ColumnDataType.STRING)
            .ColumnAssertion(nameof(MarketChangeUITableRowResponseTest.SymbolNewMarketName), ColumnDataType.STRING)
            .ColumnAssertion(nameof(MarketChangeUITableRowResponseTest.SymbolNewOpenDate), ColumnDataType.DATE_TIME)
            .ColumnAssertion(nameof(MarketChangeUITableRowResponseTest.State), ColumnDataType.STRING)
            .ColumnAssertion(nameof(MarketChangeUITableRowResponseTest.Actions), ColumnDataType.LIST);
    }

    [Theory]
    [ClassData(typeof(MarketChangeFilterGetDtoV4NotValidData))]
    public async Task Should_not_be_able_get_data_with_not_correct_filter(MarketChangeGetPaginatedListRequestTest dto, List<string> errors)
    {
        //-ARRANGE
        var api = $"{_api}?{dto.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.ErrorKey.Should().Be("MarketChangeGetPaginatedListRequestException");
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
    }

    [Fact]
    public async Task Should_be_able_data_without_any_filter()
    {
        //-ARRANGE
        await AddRequier2Async();
        var dtoFilter = new MarketChangeGetPaginatedListRequestTest();
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<MarketChangeUITableRowResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Content.Should().HaveCount(5);
        //apiResult.Data.Content.Should().HaveCount(count);
        foreach (var item in apiResult.Data.Content)
        {
        }
    }

    [Fact]
    public async Task Should_be_able_get_data_for_special_row_and_check_data_have_to_correct()
    {
        //-ARRANGE
        string oldIsin = "IRB1238585";
        string newIsin = "IRB1238586";
        var dt = new DateTime(2025, 01, 10, 11, 49, 00);
        await _factory.Repositories.ChangeMarketAddAsync(oldIsin, newIsin, dt);
        var dtoFilter = new MarketChangeGetPaginatedListRequestTest { OldIsin = oldIsin };
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<MarketChangeUITableRowResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Content.Should().NotBeEmpty();
        var marketChangeDto = apiResult.Data.Content.FirstOrDefault();
        marketChangeDto.Should().NotBeNull();
        marketChangeDto.SymbolNewIsin.Should().Be(newIsin);
        marketChangeDto.SymbolOldIsin.Should().Be(oldIsin);
        marketChangeDto.State.Should().Be("ثبت کاربر");

    }

    [Fact]
    public async Task Should_be_able_get_correct_ActionButtons()
    {
        //-ARRANGE
        string oldIsin = "IRB1238585";
        string newIsin = "IRB1238586";
        var dt = new DateTime(2025, 10, 26, 10, 17, 00);
        await _factory.Repositories.ChangeMarketAddAsync(oldIsin, newIsin, dt);
        await _factory.Repositories.ChangeMarketAddAsync("IRB1238590", "IRB1238591", dt.AddDays(-10), isFinal: true);
        await _factory.Repositories.ChangeMarketAddAsync("IRB1238592", "IRB1238593", dt.AddDays(-45), isFinal: true);
        await _factory.Repositories.ChangeMarketAddAsync("IRB1238594", "IRB1238595", dt.AddDays(45));

        //-ACT
        var response = await _client.GetAsync(_api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<MarketChangeUITableRowResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Content.Should().NotBeEmpty();
        apiResult.Data.Content.Should().HaveCount(4);

        foreach (var item in apiResult.Data.Content)
        {
            item.Actions.Should().Contain("EDIT");
            item.Actions.Should().Contain("DELETE");
        }
    }


    /// <summary>
    /// سناریو :‌
    /// باید ستون های فرم قدیمی و جدید رو داشته باشیم
    /// و اگر مقدار داشته باشند هم بیاد
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Should_be_able_show_firmId_in_table()
    {
        //-ARRANGE
        string oldIsin = "IRB1238585";
        string newIsin = "IRB1238586";
        var dt = new DateTime(2025, 11, 09, 09, 17, 00);
        await _factory.Repositories.ChangeMarketAddAsync(oldIsin, newIsin,
            dtCreate: dt, dtClose: dt.AddDays(1), dtOpen: dt.AddDays(2),
            fromFirmId: 10, toFirmId: 15);

        //-ACT
        var response = await _client.GetAsync(_api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<MarketChangeUITableRowResponseTest>>();
        apiResult.Should().NotBeNull();

        // چک کردن وجود داشتن ستون فرم ها
        apiResult.Metadata.Details
            .ColumnAssertion(nameof(MarketChangeUITableRowResponseTest.SymbolOldFirmId), ColumnDataType.INT)
            .ColumnAssertion(nameof(MarketChangeUITableRowResponseTest.SymbolNewFirmId), ColumnDataType.INT);
        apiResult.Data.Content.Should().NotBeEmpty();
        apiResult.Data.Content.Should().HaveCount(1);

        var marketChange = apiResult.Data.Content.FirstOrDefault();
        marketChange.Should().NotBeNull();
        marketChange.SymbolOldName.Should().Be("SYMBOL-NAME");
        marketChange.SymbolOldIsin.Should().Be("IRB1238585");
        marketChange.SymbolOldCloseDate.Should().Be("2025-11-10");
        marketChange.SymbolOldMarketName.Should().BeNull();
        marketChange.SymbolOldFirmId.Should().Be(10);
        marketChange.SymbolNewName.Should().Be("SYMBOL-NAME");
        marketChange.SymbolNewIsin.Should().Be("IRB1238586");
        marketChange.SymbolNewOpenDate.Should().Be("2025-11-11");
        marketChange.SymbolNewMarketName.Should().BeNull();
        marketChange.SymbolNewFirmId.Should().Be(15);
        marketChange.State.Should().Be("ثبت کاربر");
        marketChange.IsFinal.Should().BeFalse();
        marketChange.Actions.Should().Contain(new[] { "EDIT", "DELETE" });
    }
}
