using MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Requests;
using MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Responses;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.FeaturesTest.AdjustedPriceGetUITableTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "adjusted-price-ui-table")]
public partial class AdjustedPriceGetUITableTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/adjusted-price/ui-table";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public AdjustedPriceGetUITableTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
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
        var tableDto = await response.Content.ReadModelFromJsonAsync<BaseTableDto<AdjustedPriceUITableRowResponseTest>>();
        tableDto.AssertionUITableEmpty(13);

        //-- check columns 
        tableDto.Metadata.Details
            .ColumnAssertion(nameof(AdjustedPriceUITableRowResponseTest.Id), ColumnDataType.INT)
            .ColumnAssertion(nameof(AdjustedPriceUITableRowResponseTest.IsActive), ColumnDataType.BOOLEAN)
            .ColumnAssertion(nameof(AdjustedPriceUITableRowResponseTest.Date), ColumnDataType.DATE_TIME)
            .ColumnAssertion(nameof(AdjustedPriceUITableRowResponseTest.CapitalChangeCodalCode), ColumnDataType.INT)
            .ColumnAssertion(nameof(AdjustedPriceUITableRowResponseTest.DividendPerShareCodalCode), ColumnDataType.INT)
            .ColumnAssertion(nameof(AdjustedPriceUITableRowResponseTest.SymbolName), ColumnDataType.STRING)
            .ColumnAssertion(nameof(AdjustedPriceUITableRowResponseTest.SymbolIsin), ColumnDataType.STRING)
            .ColumnAssertion(nameof(AdjustedPriceUITableRowResponseTest.ClosingPrice), ColumnDataType.INT)
            .ColumnAssertion(nameof(AdjustedPriceUITableRowResponseTest.LastTradedPrice), ColumnDataType.INT)
            .ColumnAssertion(nameof(AdjustedPriceUITableRowResponseTest.AdjustedPrice), ColumnDataType.INT)
            .ColumnAssertion(nameof(AdjustedPriceUITableRowResponseTest.AdjustedLastPrice), ColumnDataType.INT)
            .ColumnAssertion(nameof(AdjustedPriceUITableRowResponseTest.IsAdjusted), ColumnDataType.BOOLEAN)
            .ColumnAssertion(nameof(AdjustedPriceUITableRowResponseTest.Actions), ColumnDataType.LIST);
    }

    [Fact]
    public async Task Should_be_able_data_without_any_filter()
    {
        //-ARRANGE
        await AddRequierAsync();
        var dtoFilter = new AdjustedPriceGetPaginatedListRequestTest();
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<AdjustedPriceUITableRowResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Content.Should().NotBeNullOrEmpty();
        apiResult.Data.Content.Count().Should().Be(5);
    }

    [Theory]
    [InlineData("2025-07-13", "", "")]
    [InlineData("", "2025-07-13", "")]
    [InlineData("", "", "2025-07-11")]
    public async Task Should_be_able_data_with_filter(string date, string startDate, string endDate)
    {
        //-ARRANGE
        await AddRequierAsync();
        var dtoFilter = new AdjustedPriceGetPaginatedListRequestTest()
        {
            StartDate = startDate,
            EndDate = endDate,
            Date = date
        };
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<AdjustedPriceUITableRowResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Content.Should().NotBeNullOrEmpty();

        foreach (var item in apiResult.Data.Content)
        {
            if (!string.IsNullOrWhiteSpace(date))
                item.Date.Should().Be(date);

            if (!string.IsNullOrWhiteSpace(startDate))
                item.Date.ConvertToDateFromMiladiDate(dateSeperator: "-").Should().BeOnOrAfter(startDate.ConvertToDateFromMiladiDate(dateSeperator: "-"));

            if (!string.IsNullOrWhiteSpace(endDate))
                item.Date.ConvertToDateFromMiladiDate(dateSeperator: "-").Should().BeOnOrBefore(endDate.ConvertToDateFromMiladiDate(dateSeperator: "-"));
        }
    }
}
