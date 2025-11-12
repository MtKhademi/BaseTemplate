using MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Requests;
using MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Responses;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.FeaturesTest.AdjustedPriceGetPaginatedList;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "adjusted-price-paginated-list")]
public partial class AdjustedPriceGetPaginatedList : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/adjusted-price/paginated-list";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public AdjustedPriceGetPaginatedList(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }

    [Theory]
    [ClassData(typeof(AdjustedPriceGetListRequestNotValidData))]
    public async Task Should_not_be_able_get_data_with_not_correct_filter(AdjustedPriceGetPaginatedListRequestTest dto, List<string> errors)
    {
        //-ARRANGE
        var api = $"{_api}?{dto.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Assertion(errors);
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
        var apiResult = await response.Content.ReadModelFromJsonAsync<PaginatedListTest<AdjustedPriceResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Should().NotBeNullOrEmpty();
        apiResult.Data.Count().Should().Be(8);
    }

    [Theory]
    [InlineData("2025-07-13", "", "","")]
    [InlineData("", "2025-07-13", "", "")]
    [InlineData("", "", "2025-07-11", "")]
    [InlineData("", "", "", "2025-07-11")]
    public async Task Should_be_able_data_with_filter(string date, string startDate, string endDate,string startAnnouncementPublishDate)
    {
        //-ARRANGE
        await AddRequierAsync();
        var dtoFilter = new AdjustedPriceGetPaginatedListRequestTest()
        {
            StartDate = startDate,
            EndDate = endDate,
            Date = date,
            StartAnnouncementPublishDateTime = startAnnouncementPublishDate
        };
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<PaginatedListTest<AdjustedPriceResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Should().NotBeNullOrEmpty();

        foreach (var item in apiResult.Data)
        {
            if (!string.IsNullOrWhiteSpace(date))
                item.Date.Should().Be(date);

            if (!string.IsNullOrWhiteSpace(startDate))
                item.Date.ConvertToDateFromMiladiDate(dateSeperator: "-").Should().BeOnOrAfter(startDate.ConvertToDateFromMiladiDate(dateSeperator: "-"));

            if (!string.IsNullOrWhiteSpace(endDate))
                item.Date.ConvertToDateFromMiladiDate(dateSeperator: "-").Should().BeOnOrBefore(endDate.ConvertToDateFromMiladiDate(dateSeperator: "-"));

            if (!string.IsNullOrWhiteSpace(startAnnouncementPublishDate))
                item.AnnouncementPublishDateTime.ConvertToDateFromMiladiDate(dateSeperator: "-").Should().BeOnOrAfter(startAnnouncementPublishDate.ConvertToDateFromMiladiDate(dateSeperator: "-"));


        }
    }


    //TODO write a test for get archive adjusted price
    [Fact]
    public async Task Should_be_able_get_archive_AdjustedPrice()
    {
        //-before split 
        var dt = new DateTime(2025, 08, 13, 14, 30, 0);
        await _factory.Repositories.AdjustedPriceArchiveAddAsync(isin: "IRB85858", dt: dt.AddDays(-4), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceArchiveAddAsync(isin: "IRB85858", dt: dt.AddDays(-3), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceArchiveAddAsync(isin: "IRB85858", dt: dt.AddDays(-2), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceArchiveAddAsync(isin: "IRB85859", dt: dt.AddDays(-1), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceArchiveAddAsync(isin: "IRB85858", dt: dt.AddDays(-1), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceArchiveAddAsync(isin: "IRB85858", dt: dt, closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceArchiveAddAsync(isin: "IRB85859", dt: dt, closingPrice: 10, lastPrice: 12);

        var dtoFilter = new AdjustedPriceGetPaginatedListRequestTest()
        {
            StartDate = "2025-08-10",
            EndDate = "2025-08-14",
        };
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<PaginatedListTest<AdjustedPriceResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Should().NotBeNull();
        apiResult.Data.Should().HaveCount(6);
        apiResult.Data.Where(x => x.SymbolIsin == "IRB85858").Should().HaveCount(4);
        apiResult.Data.Where(x => x.SymbolIsin == "IRB85859").Should().HaveCount(2);
    }

    //TODO write a test for between archive and new adjusted price
    [Fact]
    public async Task Should_be_able_get_AdjustedPrice_between_now_and_archiveData()
    {
        //-before split 
        var dt = new DateTime(2025, 11, 02, 07, 30, 0);
        await _factory.Repositories.AdjustedPriceArchiveAddAsync(isin: "IRB85858", dt: dt.AddMonths(-4), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceArchiveAddAsync(isin: "IRB85858", dt: dt.AddMonths(-3), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceArchiveAddAsync(isin: "IRB85858", dt: dt.AddMonths(-2), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceArchiveAddAsync(isin: "IRB85859", dt: dt.AddMonths(-1).AddDays(-10), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceArchiveAddAsync(isin: "IRB85858", dt: dt.AddMonths(-1).AddDays(-10), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85858", dt: dt.AddDays(-2), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85859", dt: dt.AddDays(-2), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85858", dt: dt.AddDays(-1), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85859", dt: dt.AddDays(-1), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85858", dt: dt, closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85859", dt: dt, closingPrice: 10, lastPrice: 12);


        var dtoFilter = new AdjustedPriceGetPaginatedListRequestTest()
        {
            StartDate = "2025-05-10",
            EndDate = "2025-11-03",
        };
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<PaginatedListTest<AdjustedPriceResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Should().NotBeNull();
        apiResult.Data.Should().HaveCount(11);
        apiResult.Data.Where(x => x.SymbolIsin == "IRB85858").Should().HaveCount(7);
        apiResult.Data.Where(x => x.SymbolIsin == "IRB85859").Should().HaveCount(4);
    }


}
