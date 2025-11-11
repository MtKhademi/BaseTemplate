namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.FeaturesTest.SalafProfitDailyGetUITableTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "salaf-profit-daily-ui-table")]
public partial class SalafProfitDailyGetUITable : BaseTest
{
    private readonly string _apiGetTable = $"/api/v4/symbol/bound/salaf/profit-daily/[ISIN]/ui-table";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public SalafProfitDailyGetUITable(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Fact]
    public async Task When_get_table_Expect_get_information_data_and_OK_response()
    {
        //-ARRANGE
        await RequierTest1Async();

        //-ACT
        var response = await _client.GetAsync(_apiGetTable.Replace("[ISIN]", "IRB5AE800045"));
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var tableDto = await response.Content.ReadModelFromJsonAsync<BaseTableDto<SalafProfitDailyUITableRowDtoV4Test>>();
        tableDto.AssertionUITable(6);

        tableDto.Metadata.Details
            .ColumnAssertion(nameof(SalafProfitDailyUITableRowDtoV4Test.FixedIncomeId), ColumnDataType.INT)
            .ColumnAssertion(nameof(SalafProfitDailyUITableRowDtoV4Test.SalafProfitId), ColumnDataType.INT)
            .ColumnAssertion(nameof(SalafProfitDailyUITableRowDtoV4Test.Price), ColumnDataType.INT)
            .ColumnAssertion(nameof(SalafProfitDailyUITableRowDtoV4Test.NumberOfDay), ColumnDataType.INT)
            .ColumnAssertion(nameof(SalafProfitDailyUITableRowDtoV4Test.CreatedAt), ColumnDataType.DATE_TIME)
            .ColumnAssertion(nameof(SalafProfitDailyUITableRowDtoV4Test.DateOfEvent), ColumnDataType.DATE_TIME);
    
        tableDto.Data.Content.Should().HaveCount(3);

    }
}
