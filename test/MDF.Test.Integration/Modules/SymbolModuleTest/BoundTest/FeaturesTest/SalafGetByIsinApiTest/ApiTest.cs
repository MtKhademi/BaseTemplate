namespace Test.Integration.ModulesTest.SymbolModuleTest.BoundTest.FeaturesTest.SalafGetByIsinApiTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "bound-salaf-get-by-isin")]
public partial class SalafGetByIsinTest : BaseTest
{
    private readonly string _apiAddress = $"/api/V4/symbol/bound/salaf/[ISIN]";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public SalafGetByIsinTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }



    [Theory]
    [ClassData(typeof(SalafGetByIsinNotValidData))]
    public async Task When_call_api_with_not_valid_data_Expect_get_bad_response(string isin, IEnumerable<string> messagesException)
    {
        //-ARRANGE

        //-ACT
        var response = await _client.GetAsync(_apiAddress.Replace("[ISIN]", isin));
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Messages.Should().BeEquivalentTo(messagesException);
    }


    [Fact]
    public async Task Should_get_correct_salaf_data()
    {
        //-ARRANGE
        var isin = "IRB5AE800061";
        await RequierDataTestAsync();

        //-ACT
        var response = await _client.GetAsync(_apiAddress.Replace("[ISIN]", isin));
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var fixedIncomeData = await response.Content.ReadModelFromJsonAsync<SalafGetDtoV4Test>();
        fixedIncomeData.Should().NotBeNull();
        fixedIncomeData.Should().NotBeNull();
        fixedIncomeData.FixedIncomeId.Should().BeGreaterThan(0);
        fixedIncomeData.SymbolName.Should().Be("SYMBOL-NAME");
        fixedIncomeData.SymbolIsin.Should().Be("IRB5AE800061");
        fixedIncomeData.TypeOfSymbol.Should().Be(TypeOfSymbolTest.Bond_Salaf);
        fixedIncomeData.TypeOfSymbolPersianName.Should().Be("اوراق سلف");
        fixedIncomeData.IsSalaf.Should().BeTrue();
        fixedIncomeData.Duration.Should().Be(2);
        fixedIncomeData.InterestRate.Should().Be(10);
        fixedIncomeData.RedeemedRate.Should().Be(10);

        //fixedIncomeData.PublisherName.Should().Be(data.PublisherName);
        //fixedIncomeData.MarketMakerName.Should().Be(data.MarketMakerName);

        fixedIncomeData.SubscriptionStartDate.Should().Be("2024-09-23T15:15:00");
        fixedIncomeData.SecondaryTradeStartDate.Should().Be("2024-09-27T15:15:00");
        fixedIncomeData.SecondaryTradeEndDate.Should().Be("2024-10-03T15:15:00");   
        fixedIncomeData.PublicationDate.Should().Be(null);
    }
}
