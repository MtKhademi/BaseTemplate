using MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Requests;
using MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Responses;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.FeaturesTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "adjusted-price-calculating")]
public partial class AdjustedPriceCalculatingTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/adjusted-price/calculating";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public AdjustedPriceCalculatingTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }

    [Theory]
    [InlineData("2020", null, null)]
    public async Task Should_not_be_able_to_create_when_not_valid_data(string? date, string? isin, int? firmId)
    {
        // Arrange
        var dto = new AdjustedPriceCalculatingRequestTest(
            Isin: isin,
            Date: date,
            FirmId: firmId
        );

        // Act
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
    }




    /// <summary>
    /// سناریو :‌
    /// اگر اطلاعیه ای وجود داشته باشد که برای آن نماد فعالی نباشد باید خطا بگیریم
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Get_error_when_exist_confirmedAnnouncement_that_dont_have_any_symbol()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 11, 09, 08, 20, 0);
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt);
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt.AddDays(1));
        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: 8585, dtPublish: dt);
        var dividend = await _factory.Repositories.DividendPerShareAddAsync(announcementId: announcement.AnnouncementIdPk,
            dtEntry: dt, dtModify: dt, isConfirm: true);


        var dto = new AdjustedPriceCalculatingRequestTest(Date: dt.GetISOStringDate());

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.ErrorKey.Should().Be("AnnouncementCheckingExistConfirmedAndDontHaveSymbolRuleException");
    }


    /// <summary>
    /// سناریو :‌
    /// اگر اطلاعیه ای وجود داشته باشد که برای آن نماد فعالی نباشد در بازار "NO" باید خطا بگیریم
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Get_error_when_exist_confirmAnnouncement_that_dont_have_any_symbol_in_marketNO()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 11, 09, 08, 20, 0);
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt);
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt.AddDays(1));
        var exchangeMarket = await _factory.Repositories
            .MarketExchangeAddAsync(market: new DAL.Modules.Entities.OldEntities.Market
            {
                Code = "COM",
                EnTitle = "Second Market",
                Title = "بازار دوم"
            }, securitiesExchange: new DAL.Modules.Entities.OldEntities.SecuritiesExchange
            {
                Title = "Test - exchange",
            });
        var symbol = await _factory.Repositories.SymbolAddAsync(isin: "IRB858581", firmId: 1,
            typeOfSymbol: TypeOfSymbolTest.Stock, exchangeMarketId: exchangeMarket.ExchangeMarketIdPk);

        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode: 8585, firmId: 1, dtPublish: dt);
        var dividend = await _factory.Repositories.DividendPerShareAddAsync(announcementId: announcement.AnnouncementIdPk,
            dtEntry: dt, dtModify: dt, isConfirm: true);

        var dto = new AdjustedPriceCalculatingRequestTest(Date: dt.GetISOStringDate());

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.ErrorKey.Should().Be("AnnouncementCheckingExistConfirmedAndNotExistSymbolInMarketNORuleException");
    }


    /// <summary>
    /// سناریو :‌
    /// اگر برای همه نمادها قیمت پایانی ثبت نشده باشد خطا دریافت کنیم
    /// اینجوریه که تعداد کل قیمت های پایانی رو از کل نمادهای مورد نظر کم میکنه و نباید منفی باشه
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Get_error_when_not_exist_all_closingPrices()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 11, 09, 08, 20, 0);
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt);
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt.AddDays(1));
        await _factory.Repositories.SymbolAddAsync(isin: "IROTTX812581");
        await _factory.Repositories.SymbolAddAsync(isin: "IROTTX812582");
        await _factory.Repositories.SymbolAddAsync(isin: "IROTTX812583");
        await _factory.Repositories.SymbolAddAsync(isin: "IROTTX812584");
        await _factory.Repositories.ClosingPriceAddAsync(isin: "IROTTX812583", dtOfEvent: dt.AddDays(1));
        await _factory.Repositories.ClosingPriceAddAsync(isin: "IROTTX812584", dtOfEvent: dt.AddDays(1));

        var dto = new AdjustedPriceCalculatingRequestTest(Date: dt.GetISOStringDate());

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.ErrorKey.Should().Be("ClosingPriceCheckingReceiveAllClosingPriceRuleException");
    }


    /// <summary>
    /// سناریو :‌
    /// اگر قیمت های پایانی وجود نداشته باشد باید خطا بگیریم
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Get_error_when_not_exist_ClosingPrices()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 11, 09, 08, 20, 0);
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt);
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt.AddDays(1));
        await _factory.Repositories.ClosingPriceAddAsync(isin: "IROTTX812583", dtOfEvent: dt);
        await _factory.Repositories.ClosingPriceAddAsync(isin: "IROTTX812584", dtOfEvent: dt);

        var dto = new AdjustedPriceCalculatingRequestTest(Date: dt.GetISOStringDate());

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.ErrorKey.Should().Be("ClosingPriceCheckingExistClosingPricesRuleException");
    }


    /// <summary>
    /// سناریو :‌
    /// اگر قیمت های پایانی تکراری وجود داشته باشد باید خطا بگیریم
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Get_error_when_exist_repeated_ClosingPrices()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 11, 09, 08, 20, 0);
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt);
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt.AddDays(1));
        await _factory.Repositories.ClosingPriceAddAsync(isin: "IROTTX812583", dtOfEvent: dt.AddDays(1));
        await _factory.Repositories.ClosingPriceAddAsync(isin: "IROTTX812584", dtOfEvent: dt.AddDays(1));
        await _factory.Repositories.ClosingPriceAddAsync(isin: "IROTTX812584", dtOfEvent: dt.AddDays(1));

        var dto = new AdjustedPriceCalculatingRequestTest(Date: dt.GetISOStringDate());

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.ErrorKey.Should().Be("ClosingPriceCheckingDontExistRepeatedClosingPricesRuleException");
    }
}
