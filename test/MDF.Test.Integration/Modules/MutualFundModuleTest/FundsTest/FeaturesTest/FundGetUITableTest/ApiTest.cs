using MDF.Test.Integration.Modules.MutualFundModuleTest.Requests.Responses;

namespace MDF.Test.Integration.Modules.MutualFundModuleTest.FundsTest.FeaturesTest.FundGetUITableTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("FUND", "get-ui-table")]
public partial class FundGetUITableTest : BaseTest
{
    private readonly string _api = $"/api/v4/fund/ui-table";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public FundGetUITableTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
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
        var tableDto = await response.Content.ReadModelFromJsonAsync<BaseTableDto<FundGetUITableRowResponseTest>>();

        //-- check columns 
        tableDto.Metadata.Details
            .ColumnAssertion(nameof(FundGetUITableRowResponseTest.FundId), ColumnDataType.STRING)
            .ColumnAssertion(nameof(FundGetUITableRowResponseTest.SeoregisterNumber), ColumnDataType.STRING)
            .ColumnAssertion(nameof(FundGetUITableRowResponseTest.Title), ColumnDataType.STRING)
            .ColumnAssertion(nameof(FundGetUITableRowResponseTest.FundProvider), ColumnDataType.STRING)
            .ColumnAssertion(nameof(FundGetUITableRowResponseTest.FundType), ColumnDataType.STRING)
            .ColumnAssertion(nameof(FundGetUITableRowResponseTest.DateStart), ColumnDataType.DATE_TIME)
            .ColumnAssertion(nameof(FundGetUITableRowResponseTest.DateLastChanged), ColumnDataType.DATE_TIME)
            .ColumnAssertion(nameof(FundGetUITableRowResponseTest.DateOfLastRecordNav), ColumnDataType.DATE_TIME)
            .ColumnAssertion(nameof(FundGetUITableRowResponseTest.Website), ColumnDataType.STRING)
            .ColumnAssertion(nameof(FundGetUITableRowResponseTest.Isin), ColumnDataType.STRING)
            .ColumnAssertion(nameof(FundGetUITableRowResponseTest.SymbolIsin), ColumnDataType.STRING)
            .ColumnAssertion(nameof(FundGetUITableRowResponseTest.Actions), ColumnDataType.LIST);
    }

    [Fact]
    public async Task Should_be_able_get_data_without_any_filter()
    {
        //-ARRANGE
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "123", 
            fundType: FundTypeTest.Leverage, 
            fundXMLType: FundXMLTypeTest.StockEtf);

        //-ACT
        var response = await _client.GetAsync(_api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<FundGetUITableRowResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Content.Should().HaveCount(1);
        foreach (var item in apiResult.Data.Content)
        {
            item.Actions.Should().Contain("EDIT");
            item.Actions.Should().Contain("DELETE");
            item.Actions.Should().Contain("SHOW-FUND-NAV");
            item.Actions.Should().Contain("GET-FUND-NAV-FROM-API");

            if (item.SeoregisterNumber == "123")
            {
                item.FundType.Should().Be(FundTypeTest.Leverage);
                item.FundXMLType.Should().Be(FundXMLTypeTest.StockEtf);
            }
        }
    }

    [Fact]
    public async Task Should_be_able_get_data_for_special_row_and_check_data_have_to_correct()
    {
        //-ARRANGE
        await _factory.Repositories.FundAddAsync(seoRegisterNumber: "1238585");

        var dtoFilter = new FundFilterGetPaginatedListDtoTest { SeoregisterNumber = "1238585" };
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<FundGetUITableRowResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Content.Should().NotBeEmpty();
        var fundDto = apiResult.Data.Content.FirstOrDefault();
        fundDto.Should().NotBeNull();
        fundDto.SeoregisterNumber.Should().Be("1238585");
    }

}
