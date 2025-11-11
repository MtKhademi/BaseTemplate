using MDF.Test.Integration.Modules.SymbolModuleTest.ClosingPrice.Requests;
using MDF.Test.Integration.Modules.SymbolModuleTest.ClosingPrice.Responses;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.ClosingPrice.FeaturesTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "closing-price-create-or-update")]
public partial class ClosingPriceCreateOrUpdateTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/closing-price";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public ClosingPriceCreateOrUpdateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }

    [Theory]
    [InlineData(null, null, null, null, null, null, null)]
    [InlineData("IRB123", null, null, null, null, null, null)]
    [InlineData("IRBXX8585123", null, null, null, null, null, "2025")]
    [InlineData("IRBXX8585123", null, null, null, null, null, "2025-11-04")]
    [InlineData("IRBXX8585123", "Closing", null, null, null, null, "2025-11-04")]
    public async Task Should_not_be_able_when_not_correct_data(
            string? SymbolIsin = null,
            string? TypeOfClosingPriceIndicator = null,
            double? ClosingPrice = null,
            double? LastTradePrice = null,
            int? TradesCount = null,
            long? TradesVolume = null,
            string? DateTimeOfEvent = null)
    {
        //-ARRANGE
        var dto = new ClosingPriceCreateOrUpdateRequestTest(
            SymbolIsin,
            TypeOfClosingPriceIndicator,
            ClosingPrice,
            LastTradePrice,
            TradesCount,
            TradesVolume,
            DateTimeOfEvent
        );
        var api = $"{_api}?{dto.ToQueryString()}";

        //-ACT
        var response = await _client.PostAsync(api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
        apiResult.ErrorKey.Should().Be("ClosingPriceCreateOrUpdateRequestException");
    }


    [Fact]
    public async Task Should_not_be_able_add_if_not_exist_symbol()
    {
        //-ARRANGE
        var dto = new ClosingPriceCreateOrUpdateRequestTest(
            SymbolIsin: "IRBXX1251251",
            TypeOfClosingPriceIndicator: "RealTime",
            ClosingPrice: 10, LastTradePrice: 120,
            TotalNumberOfTrade: 1, TotalNumberOfSharesTrade: 1,
            DateTimeOfEvent: "2025-11-04T07:46:00"
        );
        var api = $"{_api}?{dto.ToQueryString()}";

        //-ACT
        var response = await _client.PostAsync(api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.NotFound);
        apiResult.ErrorKey.Should().Be("SymbolNotExistIsinException");
    }

    [Fact]
    public async Task Should_not_be_able_add_when_not_exist_calendar_for_currectDate()
    {
        //-ARRANGE
        var isin = "IRBXX1251251";
        var dt = "2025-11-04T07:46:00".GetDateTimeFromISOFormat();
        var symbol = await _factory.Repositories.SymbolAddAsync(isin: isin);
        var dto = new ClosingPriceCreateOrUpdateRequestTest(
            SymbolIsin: isin,
            TypeOfClosingPriceIndicator: "RealTime",
            ClosingPrice: 10, LastTradePrice: 120,
            TotalNumberOfTrade: 1, TotalNumberOfSharesTrade: 1,
            DateTimeOfEvent: dt.GetISOStringDateTime()
        );
        var api = $"{_api}?{dto.ToQueryString()}";

        //-ACT
        var response = await _client.PostAsync(api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.NotFound);
        apiResult.ErrorKey.Should().Be("CalendarNotFoundWithDateException");
    }

    [Fact]
    public async Task Should_not_be_able_add_when_not_exist_calendar_for_nextWorkingDate()
    {
        //-ARRANGE
        var isin = "IRBXX1251251";
        var dt = "2025-11-04T07:46:00".GetDateTimeFromISOFormat();
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt);
        var symbol = await _factory.Repositories.SymbolAddAsync(isin: isin);
        var dto = new ClosingPriceCreateOrUpdateRequestTest(
            SymbolIsin: isin,
            TypeOfClosingPriceIndicator: "RealTime",
            ClosingPrice: 10, LastTradePrice: 120,
            TotalNumberOfTrade: 1, TotalNumberOfSharesTrade: 1,
            DateTimeOfEvent: dt.GetISOStringDateTime()
        );
        var api = $"{_api}?{dto.ToQueryString()}";

        //-ACT
        var response = await _client.PostAsync(api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.NotFound);
        apiResult.ErrorKey.Should().Be("CalendarNotFoundNextWorkingDayException");
    }

    // باید بتونیم چندین رکورد قیمت لحظه ای وارد کنیم و آپدیت نکنه . قیمت لحظه ی رو نمیشه اپدیت کرد
    [Fact]
    public async Task Should_be_able_add_multi_realTimePrice()
    {
        //-ARRANGE
        var isin = "IRBXX1251251";
        var symbol = await _factory.Repositories.SymbolAddAsync(isin: isin);
        var dt = "2025-11-04T07:46:00".GetDateTimeFromISOFormat();
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt);
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt.AddDays(1), isHoliday: false);
        //-> ده تا ارسال میکنیم و هرکدوم با فاصله 10 ثانیه باید ثبت بشن و خروجی بگیریم
        for (int i = 0; i < 10; i++)
        {
            var dto = new ClosingPriceCreateOrUpdateRequestTest(
                SymbolIsin: isin,
                TypeOfClosingPriceIndicator: "RealTime",
                ClosingPrice: 10, LastTradePrice: 120,
                TotalNumberOfTrade: 10, TotalNumberOfSharesTrade: 1,
                DateTimeOfEvent: dt.AddSeconds(i * 10).GetISOStringDateTime()
            );
            var api = $"{_api}?{dto.ToQueryString()}";

            //-ACT
            var response = await _client.PostAsync(api, dto.ToContentHttpString());
            await response.WriteOnConsoleAsync(_outPutHelper);

            //-ASSERT
            var apiResult = await response.Content.ReadModelFromJsonAsync<ClosingPriceResponseTest>();
            apiResult.Should().NotBeNull();
            apiResult.SymbolIsin.Should().NotBeNull();
            apiResult.SymbolIsin.Should().Be(isin);
            apiResult.ClosingPrice.Should().Be(10);
            apiResult.LastTradePrice.Should().Be(120);
            apiResult.TypeOfClosingPriceIndicator.Should().Be(TypeOfClosingPriceIndicatorTest.RealTime);
            apiResult.TotalNumberOfTrade.Should().Be(dto.TotalNumberOfTrade);
            apiResult.TotalNumberOfSharesTrade.Should().Be(dto.TotalNumberOfSharesTrade);
            apiResult.DateTimeOfEvent.Should().NotBeNull();
            apiResult.DateTimeOfEvent.Should().Be(dto.DateTimeOfEvent);
        }
    }


    // باید بتونیم چندین رکورد قیمت لحظه ای وارد کنیم و آپدیت نکنه . قیمت لحظه ی رو نمیشه اپدیت کرد
    [Fact]
    public async Task Should_be_able_add_one_closingPrice_and_then_update_that()
    {
        //-ARRANGE
        var isin = "IRBXX1251251";
        var symbol = await _factory.Repositories.SymbolAddAsync(isin: isin);
        var dt = "2025-11-04T07:46:00".GetDateTimeFromISOFormat();
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt);
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt.AddDays(1), isHoliday: false);
        //-> ده تا ارسال میکنیم و هرکدوم با فاصله 10 ثانیه باید ثبت بشن و خروجی بگیریم
        for (int i = 0; i < 10; i++)
        {
            var dto = new ClosingPriceCreateOrUpdateRequestTest(
                SymbolIsin: isin,
                TypeOfClosingPriceIndicator: "ClosingPrice",
                ClosingPrice: 10, LastTradePrice: 120,
                TotalNumberOfTrade: 10, TotalNumberOfSharesTrade: 1,
                DateTimeOfEvent: dt.AddSeconds(i * 10).GetISOStringDateTime()
            );
            var api = $"{_api}?{dto.ToQueryString()}";

            //-ACT
            var response = await _client.PostAsync(api, dto.ToContentHttpString());
            await response.WriteOnConsoleAsync(_outPutHelper);

            //-ASSERT
            var apiResult = await response.Content.ReadModelFromJsonAsync<ClosingPriceResponseTest>();
            apiResult.Should().NotBeNull();
            apiResult.SymbolIsin.Should().NotBeNull();
            apiResult.SymbolIsin.Should().Be(isin);
            apiResult.ClosingPrice.Should().Be(10);
            apiResult.LastTradePrice.Should().Be(120);
            apiResult.TypeOfClosingPriceIndicator.Should().Be(TypeOfClosingPriceIndicatorTest.ClosingPrice);
            apiResult.TotalNumberOfTrade.Should().Be(dto.TotalNumberOfTrade);
            apiResult.TotalNumberOfSharesTrade.Should().Be(dto.TotalNumberOfSharesTrade);
            apiResult.DateTimeOfEvent.Should().NotBeNull();
            apiResult.DateTimeOfEvent.Should().Be("2025-11-05T00:00:00");
            apiResult.DateOfEvent.Should().Be("2025-11-05");
        }
    }
}
