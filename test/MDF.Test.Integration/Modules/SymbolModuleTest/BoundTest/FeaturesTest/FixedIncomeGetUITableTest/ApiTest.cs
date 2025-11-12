namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.FeaturesTest.FixedIncomeGetUITableTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "fixed-income-ui-table")]
public partial class FixIncomeGetTableApiTest : BaseTest
{
    private readonly string _apiGetTable = $"/api/V2/SymbolApi/FixedIncome/Table";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public FixIncomeGetTableApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
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

        //-ACT
        var response = await _client.GetAsync(_apiGetTable);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var tableDto = await response.Content.ReadModelFromJsonAsync<BaseTableDto<FixedIncomeGetUIDtoV2Test>>();
        //- check basic------------------------------------------
        tableDto.Metadata.Basic.PersianTitle.Should().NotBeNullOrWhiteSpace();
        tableDto.Metadata.Basic.HasRowNumber.Should().BeTrue();
        tableDto.Data.Should().NotBeNull();
        tableDto.Data.Pageable.Should().NotBeNull();
        tableDto.Data.Content.Should().NotBeNull();
        tableDto.Metadata.Details.Should().NotBeNull();

        //-check columns---------------------------------------
        tableDto.Metadata.Details.Count.Should().Be(17);

        //-- check columns 
        tableDto.Metadata.Details
            .ColumnAssertion(nameof(FixedIncomeGetUIDtoV2Test.TypeOfSymbolPersianName), ColumnDataType.STRING)
            .ColumnAssertion(nameof(FixedIncomeGetUIDtoV2Test.Duration), ColumnDataType.INT)
            .ColumnAssertion(nameof(FixedIncomeGetUIDtoV2Test.SymbolName), ColumnDataType.STRING)
            .ColumnAssertion(nameof(FixedIncomeGetUIDtoV2Test.SymbolIsin), ColumnDataType.STRING)
            .ColumnAssertion(nameof(FixedIncomeGetUIDtoV2Test.NominalValue), ColumnDataType.INT)
            .ColumnAssertion(nameof(FixedIncomeGetUIDtoV2Test.InterestRate), ColumnDataType.STRING)
            .ColumnAssertion(nameof(FixedIncomeGetUIDtoV2Test.InterestPaymentInterval), ColumnDataType.STRING)
            .ColumnAssertion(nameof(FixedIncomeGetUIDtoV2Test.SubscriptionStartDate), ColumnDataType.DATE_TIME)
            .ColumnAssertion(nameof(FixedIncomeGetUIDtoV2Test.SubscriptionEndDate), ColumnDataType.DATE_TIME)
            .ColumnAssertion(nameof(FixedIncomeGetUIDtoV2Test.TradeStartDate), ColumnDataType.DATE_TIME)
            .ColumnAssertion(nameof(FixedIncomeGetUIDtoV2Test.MaturityDate), ColumnDataType.DATE_TIME)
            .ColumnAssertion(nameof(FixedIncomeGetUIDtoV2Test.PublicationDate), ColumnDataType.DATE_TIME)
            .ColumnAssertion(nameof(FixedIncomeGetUIDtoV2Test.MarketMakerName), ColumnDataType.STRING)
            .ColumnAssertion(nameof(FixedIncomeGetUIDtoV2Test.PublisherName), ColumnDataType.STRING)
            .ColumnAssertion(nameof(FixedIncomeGetUIDtoV2Test.GurantorName), ColumnDataType.STRING)
            .ColumnAssertion(nameof(FixedIncomeGetUIDtoV2Test.Description), ColumnDataType.STRING)
            .ColumnAssertion(nameof(FixedIncomeGetUIDtoV2Test.Actions), ColumnDataType.LIST);

        tableDto.Data.Pageable.PageNumber.Should().Be(0);
        tableDto.Data.Pageable.PageSize.Should().Be(100);



        foreach (var item in tableDto.Data.Content)
        {
            item.TypeOfSymbol.IsBond().Should().BeTrue();
        }
    }

    [Theory]
    [ClassData(typeof(FixedIncomeTableFilterDtoNotValidData))]
    public async Task When_not_valid_filter_Expect_get_bad_response(FixedIncomeTableFilterDtoV2Test dto, List<string> errors)
    {

        //-ARRANGE
        var api = $"{_apiGetTable}?{dto.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
        apiResult.Messages.Should().BeEquivalentTo(errors);
    }


    [Fact]
    public async Task When_call_api_without_filter_Expect_get_ok_response_and_currect_data()
    {
        //-ARRANGE
        var dtoFilter = new FixedIncomeTableFilterDtoV2TestBuilder().Build();
        var api = $"{_apiGetTable}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<FixedIncomeGetUIDtoV2Test>>();
        apiResult.Should().NotBeNull();
        //apiResult.Data.Content.Should().HaveCount(count);
        foreach (var item in apiResult.Data.Content)
        {
            item.TypeOfSymbol.IsBond().Should().BeTrue();

            item.Actions.Should().HaveCount(2);
            item.Actions.Should().Contain(["EDIT", "DETAILS"]);
        }
    }

    [Theory]
    [InlineData("IRB5AE800003")]
    public async Task When_try_to_get_non_bond_symbol_Expect_get_ok_response_and_empty_data(string isin)
    {
        //-ARRANGE
        var dtoFilter = new FixedIncomeTableFilterDtoV2TestBuilder()
            .WithIsin(isin)
            .Build();
        var api = $"{_apiGetTable}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<FixedIncomeGetUIDtoV2Test>>();
        apiResult.Should().NotBeNull();
        //apiResult.Data.Content.Should().HaveCount(count);
        apiResult.Data.Content.Should().BeEmpty();
    }


    [Theory]
    [ClassData(typeof(FixedIncomeTableValidData))]
    public async Task When_try_to_get_bond_symbol_that_has_fixedIncome_data_Expect_get_ok_response(FixedIncomeTableFilterDtoV2Test filterDto, FixedIncomeGetUIDtoV2Test data)
    {
        //-ARRANGE
        await FixIncomeGetTableApiTestRequierAsync();
        var api = $"{_apiGetTable}?{filterDto.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<FixedIncomeGetUIDtoV2Test>>();
        apiResult.Should().NotBeNull();
        //apiResult.Data.Content.Should().HaveCount(count);
        apiResult.Data.Content.Should().NotBeEmpty();
        apiResult.Data.Content.Should().HaveCount(1);

        var fixedIncomeData = apiResult.Data.Content.FirstOrDefault();
        fixedIncomeData.Should().NotBeNull();
        fixedIncomeData.SymbolName.Should().Be(data.SymbolName);
        fixedIncomeData.SymbolIsin.Should().Be(data.SymbolIsin);
        fixedIncomeData.TypeOfSymbol.Should().Be(data.TypeOfSymbol);
        fixedIncomeData.TypeOfSymbolPersianName.Should().Be(data.TypeOfSymbolPersianName);
        fixedIncomeData.Duration.Should().Be(data.Duration);
        fixedIncomeData.InterestPaymentInterval.Should().Be(data.InterestPaymentInterval);
        fixedIncomeData.InterestRate.Should().Be(data.InterestRate);
        fixedIncomeData.RedeemedRate.Should().Be(data.RedeemedRate);

        fixedIncomeData.PublisherName.Should().Be(data.PublisherName);
        fixedIncomeData.MarketMakerName.Should().Be(data.MarketMakerName);

        fixedIncomeData.SubscriptionStartDate.Should().Be(data.SubscriptionStartDate);
        fixedIncomeData.PublicationDate.Should().Be(data.PublicationDate);

        fixedIncomeData.Actions.Should().HaveCount(2);
        fixedIncomeData.Actions.Should().Contain(["EDIT", "DETAILS"]);
    }


    [Theory]
    [ClassData(typeof(FixedIncomeTableFilterValidData))]
    public async Task When_try_to_get_bonds_data_with_filters_Expect_get_ok_response(FixedIncomeTableFilterDtoV2Test filterDto)
    {
        //-ARRANGE
        var api = $"{_apiGetTable}?{filterDto.ToQueryString()}";
        List<string> isins = string.IsNullOrWhiteSpace(filterDto.Isins) ? [] : filterDto.Isins.Split(Constants.SEPERATOR).ToList();


        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);


        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<FixedIncomeGetUIDtoV2Test>>();
        foreach (var bond in apiResult.Data.Content)
        {
            bond.SymbolIsin.Should().NotBeNullOrWhiteSpace();
            bond.SymbolName.Should().NotBeNullOrWhiteSpace();
            bond.TypeOfSymbol.IsBond().Should().BeTrue();

            if (filterDto.TypeOfSymbols != null && filterDto.TypeOfSymbols.Any())
                filterDto.TypeOfSymbols.Select(x => ((int)x)).Should().Contain((int)bond.TypeOfSymbol);



            if (isins.Any()) isins.Should().Contain(bond.SymbolIsin);

            bond.Actions.Should().HaveCount(2);
            bond.Actions.Should().Contain(["EDIT", "DETAILS"]);

        }

    }


    [Fact]
    public async Task When_try_to_get_bond_when_its_not_in_MarketNO_EXPECT_ok_response_and_not_any_data()
    {
        //-ARRANGE
        var filterDto = new FixedIncomeTableFilterDtoV2TestBuilder()
            .WithIsin("IRB5AE800032")
            .Build();

        var api = $"{_apiGetTable}?{filterDto.ToQueryString()}";
        List<string> isins = string.IsNullOrWhiteSpace(filterDto.Isins) ? [] : filterDto.Isins.Split(Constants.SEPERATOR).ToList();


        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);


        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<BaseTableDto<FixedIncomeGetUIDtoV2Test>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Should().NotBeNull();
        apiResult.Data.Content.Should().NotBeNull();

    }


}
