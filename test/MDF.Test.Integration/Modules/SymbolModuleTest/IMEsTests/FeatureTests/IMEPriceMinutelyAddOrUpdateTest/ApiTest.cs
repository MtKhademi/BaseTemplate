using MDF.Modules.Common.Providers.DateTimeProviders;
using MDF.Test.Integration.Common.Extentions;
using SharedModule.Notifications.SmsService;
using SymbolModule.IMEs.IMEService;
using SymbolModule.IMEs.IMEService.Dtos;
using System.Net;
using Test.Integration.Common.Dtos;
using Test.Integration.ModulesTest.Common;
using Test.Integration.ModulesTest.SymbolModuleTest.Dtos;
using Test.Integration.ModulesTest.SymbolModuleTest.Enumerations;

namespace Test.Integration.ModulesTest.SymbolModuleTest.IMEsTests.FeatureTests.IMEPriceRealTimeAddOrUpdateTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait($"SymbolModule-{nameof(IMEPriceRealTimeAddOrUpdateTest)}", "V4")]
public class IMEPriceRealTimeAddOrUpdateTest : BaseTest
{
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    private string _api = "/api/v4/symbol/ime/tadbir-api/get-and-save-price/minutely";

    public IMEPriceRealTimeAddOrUpdateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _outPutHelper = outPutHelper;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);
    }

    private HttpClient CreateClientWithMocks(PriceDto priceDto, DateTime now)
    {
        // Mock IGetPriceService
        var mockSmsSender = new Mock<ISmsService>();
        mockSmsSender
            .Setup(x => x.SendAsync(It.IsAny<List<string>>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        var mockTadbirService = new Mock<IGetPriceService>();
        mockTadbirService
            .Setup(s => s.GetPriceAsync("TEST1"))
            .ReturnsAsync(priceDto);

        // Mock IDateTimeProvider
        var dateTimeMock = new Mock<IDateTimeProvider>();
        dateTimeMock
            .Setup(x => x.Now)
            .Returns(now);

        // Create a new client with the custom DI setup
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IGetPriceService>();
                services.AddSingleton(mockTadbirService.Object);

                services.RemoveAll<IDateTimeProvider>();
                services.AddScoped(_ => dateTimeMock.Object);

                services.RemoveAll<ISmsService>();
                services.AddSingleton(mockSmsSender.Object);

            });
        }).CreateClient();

        client.DefaultRequestHeaders.Add("Authorization", TestConstants.TOKEN_ACCESS);

        return client;
    }

    //[Fact]
    //public async Task When_empty_bourseCode_then_get_bad_request()
    //{
    //    // ARRANGE
    //    var now = new DateTime(2025, 02, 02, 12, 38, 0);
    //    var priceDto = new PriceDto
    //    {
    //        ClosingPriceDateTime = "1403/11/13 16:2:34",
    //        LastTradePriceDateTime = "1403/11/13 16:2:34",
    //        LastPriceToday = 902510.0,
    //        HighPriceToday = 920000,
    //        PriceYesterday = 907097,
    //        ClosingPrice = 902510,
    //        TradesCount = 457,
    //        TradesVolume = 126166
    //    };
    //    _client = CreateClientWithMocks(priceDto, now);
    //    await _factory.Repositories.SymbolAddAsync(isin: "IRKCD1GOB0001", bourseCode: "");

    //    // ACT
    //    var response = await _client.PostAsync(_api, null);
    //    await response.WriteOnConsoleAsync(_outPutHelper);

    //    // ASSERT
    //    response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    //    var apiResult = await response.Content.ReadFromJsonAsync<ApiResultTest>();
    //    apiResult.Assertion(["Symbol \u0027IRKCD1GOB0001\u0027 is missing a bourse code."]);
    //}

    //[Fact]
    //public async Task When_today_is_holiday_and_not_thursday_then_get_empty_data()
    //{
    //    // ARRANGE
    //    var now = new DateTime(2025, 02, 02, 12, 38, 0);
    //    var priceDto = new PriceDto
    //    {
    //        ClosingPriceDateTime = "1403/11/13 16:2:34",
    //        LastTradePriceDateTime = "1403/11/13 16:2:34",
    //        LastPriceToday = 902510.0,
    //        HighPriceToday = 920000,
    //        PriceYesterday = 907097,
    //        ClosingPrice = 902510,
    //        TradesCount = 457,
    //        TradesVolume = 126166
    //    };
    //    _client = CreateClientWithMocks(priceDto, now);
    //    await _factory.Repositories.SymbolAddAsync(isin: "IRKCD1GOB0001", bourseCode: "TEST1");

    //    // ACT
    //    var response = await _client.PostAsync(_api, null);
    //    await response.WriteOnConsoleAsync(_outPutHelper);

    //    // ASSERT
    //    response.StatusCode.Should().Be(HttpStatusCode.OK);
    //    var apiResult = await response.Content.ReadFromJsonAsync<List<ClosingPriceGetDtoTest>>();
    //    apiResult.Should().NotBeNull().And.BeEmpty();
    //    apiResult.Capacity.Should().Be(0);
    //}


    //[Fact]
    //public async Task When_ClosingPriceDateTime_correct_data_then_we_have_to_get_that()
    //{
    //    // ARRANGE
    //    var now = new DateTime(2025, 02, 05, 12, 38, 0);
    //    var priceDto = new PriceDto
    //    {
    //        ClosingPriceDateTime = "1403/11/17 11:38:34",
    //        LastTradePriceDateTime = "1403/11/17 11:38:34",
    //        LastPriceToday = 902510.0,
    //        HighPriceToday = 920000,
    //        PriceYesterday = 907097,
    //        ClosingPrice = 902510,
    //        TradesCount = 457,
    //        TradesVolume = 126166
    //    };
    //    _client = CreateClientWithMocks(priceDto, now);
    //    await _factory.Repositories.SymbolAddAsync(isin: "IRKCD1GOB0001", bourseCode: "TEST1");

    //    // ACT
    //    var response = await _client.PostAsync(_api, null);
    //    await response.WriteOnConsoleAsync(_outPutHelper);

    //    // ASSERT
    //    response.StatusCode.Should().Be(HttpStatusCode.OK);
    //    var apiResult = await response.Content.ReadFromJsonAsync<List<ClosingPriceGetDtoTest>>();
    //    apiResult.Should().NotBeNull();
    //    apiResult.Capacity.Should().Be(1);
    //    var closingPrice = apiResult.First();
    //    closingPrice.Should().NotBeNull();
    //    closingPrice.SymbolIsin.Should().Be("IRKCD1GOB0001");
    //    closingPrice.ClosingPrice.Should().Be(priceDto.ClosingPrice);
    //    closingPrice.LastTradePrice.Should().Be(priceDto.LastPriceToday);
    //    closingPrice.TotalNumberOfTrade.Should().Be(priceDto.TradesCount);
    //    closingPrice.TotalTradeValue.Should().Be(priceDto.TradesVolume);
    //    closingPrice.TypeOfClosingPriceIndicator.Should().Be(TypeOfClosingPriceIndicatorTest.ClosingPrice);

    //}
}
