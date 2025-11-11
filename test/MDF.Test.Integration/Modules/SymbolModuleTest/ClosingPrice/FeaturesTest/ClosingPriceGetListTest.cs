using MDF.Test.Integration.Modules.SymbolModuleTest.ClosingPrice.Requests;
using MDF.Test.Integration.Modules.SymbolModuleTest.ClosingPrice.Responses;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.ClosingPrice.FeaturesTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "closing-price-list")]
public partial class ClosingPriceGetListTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/closing-price/list";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public ClosingPriceGetListTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }

    [Theory]
    [InlineData(null, null, null, null)]
    public async Task Should_not_be_able_get_data_with_not_correct_filter(
        string? startDate, string? endDate,
        string? isin, params string[]? isins)
    {
        //-ARRANGE
        var dto = new ClosingPriceGetListRequestTest
        {
            EndDate = endDate,
            StartDate = startDate,
            Isin = isin,
            Isins = isins
        };
        var api = $"{_api}?{dto.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
        apiResult.ErrorKey.Should().Be("ClosingPriceGetListRequestException");
    }

    [Fact]
    public async Task Should_not_be_able_get_data_without_any_filter()
    {
        //-ARRANGE
        var dtoFilter = new ClosingPriceGetListRequestTest();
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
        apiResult.ErrorKey.Should().Be("ClosingPriceGetListRequestException");
    }


    [Theory]
    [InlineData("2025-11-02", "2025-11-03", "IRB8TOT85851", null)]
    [InlineData("2025-11-02", "2025-11-03", "IRB8TOT85853", null)]
    [InlineData("2025-11-02", "2025-11-03", null, "IRB8TOT85853", "IRB8TOT85851")]
    [InlineData(null, null, null, "IRB8TOT85853", "IRB8TOT85851")]
    public async Task should_be_able_get_data(
        string? startDate, string? endDate,
        string? isin, params string[]? isins)
    {
        //-ARRANGE
        var dt = new DateTime(2025, 11, 03, 10, 09, 0);
        await _factory.Repositories.ClosingPriceAddAsync(isin: "IRB8TOT85851", dtOfEvent: dt.AddDays(-4));
        await _factory.Repositories.ClosingPriceAddAsync(isin: "IRB8TOT85851", dtOfEvent: dt.AddDays(-3));
        await _factory.Repositories.ClosingPriceAddAsync(isin: "IRB8TOT85851", dtOfEvent: dt.AddDays(-2));
        await _factory.Repositories.ClosingPriceAddAsync(isin: "IRB8TOT85851", dtOfEvent: dt.AddDays(-1));
        await _factory.Repositories.ClosingPriceAddAsync(isin: "IRB8TOT85851", dtOfEvent: dt);
        await _factory.Repositories.ClosingPriceAddAsync(isin: "IRB8TOT85852", dtOfEvent: dt);
        await _factory.Repositories.ClosingPriceAddAsync(isin: "IRB8TOT85853", dtOfEvent: dt, closingPrice: 10, lastPrice: 12);

        var dtoFilter = new ClosingPriceGetListRequestTest()
        {
            Isin = isin,
            Isins = isins,
            StartDate = startDate,
            EndDate = endDate
        };
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<IEnumerable<ClosingPriceResponseTest>>();
        apiResult.Should().NotBeNull();
        foreach (var item in apiResult)
        {
            if (!string.IsNullOrWhiteSpace(dtoFilter.Isin))
                item.SymbolIsin.Should().Be(dtoFilter.Isin);

            if (isins is not null && isins.Any())
            {
                isins.Contains(item.SymbolIsin).Should().BeTrue();
            }

            if (!string.IsNullOrWhiteSpace(startDate))
            {
                var startDateDt = startDate.GetDateFromISOFormat();
                item.DateOfEvent.Should().NotBeNull();
                item.DateOfEvent.GetDateFromISOFormat().Should().BeOnOrAfter(startDateDt);
            }

            if (!string.IsNullOrWhiteSpace(endDate))
            {
                var endDateDt = endDate.GetDateFromISOFormat();
                item.DateOfEvent.Should().NotBeNull();
                item.DateOfEvent.GetDateFromISOFormat().Should().BeOnOrBefore(endDateDt);
            }
        }
    }
}
