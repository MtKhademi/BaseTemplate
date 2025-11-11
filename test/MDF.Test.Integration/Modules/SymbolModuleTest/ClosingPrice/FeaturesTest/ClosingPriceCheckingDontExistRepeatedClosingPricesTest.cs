namespace MDF.Test.Integration.Modules.SymbolModuleTest.ClosingPrice.FeaturesTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "closing-price-checking-dont-exist-repeated")]
public partial class ClosingPriceCheckingDontExistRepeatedClosingPricesTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/closing-price/checking-dont-exist-repeated-closing-price";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public ClosingPriceCheckingDontExistRepeatedClosingPricesTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }

    [Theory]
    [InlineData("2025")]
    public async Task Should_not_be_able_get_data_with_not_correct_filter(string? date)
    {
        //-ARRANGE
        var api = $"{_api}/{date}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
    }


    [Fact]
    public async Task Should_be_able_get_symbolId_that_have_multiPrice()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 11, 04, 13, 30, 0);
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt);
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt.AddDays(1));
        await _factory.Repositories.ClosingPriceAddAsync(isin: "IROTTX812583", dtOfEvent: dt.AddDays(1));
        await _factory.Repositories.ClosingPriceAddAsync(isin: "IROTTX812584", dtOfEvent: dt.AddDays(1));
        await _factory.Repositories.ClosingPriceAddAsync(isin: "IROTTX812584", dtOfEvent: dt.AddDays(1));

        var api = $"{_api}/{dt.GetISOStringDate()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<IEnumerable<(int symbolId, int count)>>();
        apiResult.Should().NotBeNull(); 
        apiResult.Should().HaveCount(1);
    }
}
