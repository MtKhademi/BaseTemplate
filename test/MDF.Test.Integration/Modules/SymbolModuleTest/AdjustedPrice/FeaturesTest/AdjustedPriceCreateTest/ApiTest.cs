using MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Requests;
using MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Responses;
using System.Net;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.FeaturesTest.AdjustedPriceCreateTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "adjusted-price-create")]
public partial class AdjustedPriceCreateTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/adjusted-price/";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public AdjustedPriceCreateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }

    [Theory]
    [ClassData(typeof(NotCorrectDataTest))]
    public async Task Should_not_be_able_to_create_when_not_valid_data(AdjustedPriceCreateRequestTest dto, List<string> errorMessages)
    {
        // Arrange

        // Act
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Assertion(errorMessages);
    }


    [Fact]
    public async Task Should_not_be_able_to_create_when_not_exist_isin()
    {
        // Arrange
        var dto = new AdjustedPriceCreateRequestTest
        {
            Isin = "IRBXXXX8585",
            Price = 850,
            LastPrice = 250,
            AdjustedPrice = 850,
            AdjustedLastPrice = 250,
            IsAdjusted = true,
            Date = new DateTime(2025, 07, 19, 08, 46, 0).GetISOStringDate()
        };

        // Act
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Assertion(["there is not exist any SymbolEntity =\u003E Isin : IRBXXXX8585"]);
    }

    [Fact]
    public async Task Should_not_be_able_to_create_when_not_exist_CalendarDate()
    {
        // Arrange
        await _factory.Repositories.SymbolAddAsync(isin: "IRBXXXX8585", symbolName: "یک تست");
        var dt = new DateTime(2025, 07, 19, 08, 46, 0);
        var dto = new AdjustedPriceCreateRequestTest
        {
            Isin = "IRBXXXX8585",
            AdjustedLastPrice = 250,
            AdjustedPrice = 850,
            LastPrice = 250,
            Price = 850,
            IsAdjusted = true,
            Date = dt.GetISOStringDate()
        };

        // Act
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.NotFound);
        apiResult.ErrorKey.Should().Be("CalendarNotFoundWithDateException");
    }

    [Fact]
    public async Task Should_be_able_to_create()
    {
        // Arrange
        var dt = new DateTime(2025, 07, 19, 08, 46, 0);
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt);
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt.AddDays(1));
        await _factory.Repositories.SymbolAddAsync(isin: "IRBXXXX8585", symbolName: "یک تست");
        var dto = new AdjustedPriceCreateRequestTest
        {
            Isin = "IRBXXXX8585",
            AdjustedLastPrice = 250,
            AdjustedPrice = 850,
            LastPrice = 250,
            Price = 850,
            IsAdjusted = true,
            Date = dt.GetISOStringDate()
        };

        // Act
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<AdjustedPriceResponseTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsAdjusted.Should().Be(dto.IsAdjusted);
        apiResult.AdjustedLastPrice.Should().Be(dto.AdjustedLastPrice);
        apiResult.AdjustedPrice.Should().Be(dto.AdjustedPrice);
        apiResult.SymbolIsin.Should().Be(dto.Isin);
        apiResult.SymbolName.Should().Be("یک تست");
        apiResult.Date.Should().Be(dt.GetISOStringDate());
    }


    [Fact]
    public async Task Should_not_be_able_to_create_when_exist_already()
    {
        // Arrange
        var dt = new DateTime(2025, 07, 19, 08, 46, 0);
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt);
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dt.AddDays(1));
        await _factory.Repositories.SymbolAddAsync(isin: "IRBXXXX8585", symbolName: "یک تست");
        var dto = new AdjustedPriceCreateRequestTest
        {
            Isin = "IRBXXXX8585",
            AdjustedLastPrice = 250,
            AdjustedPrice = 850,
            LastPrice = 250,
            Price = 850,
            IsAdjusted = true,
            Date = dt.GetISOStringDate()
        };
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Act
        response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.AlreadyExist);
    }
}
