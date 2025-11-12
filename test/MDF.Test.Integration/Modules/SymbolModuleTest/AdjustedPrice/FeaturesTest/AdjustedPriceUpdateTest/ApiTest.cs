using MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Requests;
using MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Responses;
using System.Net;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.FeaturesTest.AdjustedPriceUpdateTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "adjusted-price-update")]
public partial class AdjustedPriceUpdateTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/adjusted-price/";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public AdjustedPriceUpdateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }

    [Theory]
    [ClassData(typeof(NotCorrectDataTest))]
    public async Task Should_not_be_able_to_update_when_not_valid_data(AdjustedPriceUpdateRequestTest dto, List<string> errorMessages)
    {
        // Arrange

        // Act
        var response = await _client.PutAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Assertion(errorMessages);
    }


    [Fact]
    public async Task Should_not_be_able_to_update_when_not_exist_isin()
    {
        // Arrange
        var dto = new AdjustedPriceUpdateRequestTest
        {
            AdjustedLastPrice = 250,
            AdjustedPrice = 850,
            AdjustedPriceId = 125,
            IsAdjusted = true,
        };

        // Act
        var response = await _client.PutAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Assertion(["there is not exist any AdjustedPriceEntity =\u003E Id : 125"]);
    }

    [Fact]
    public async Task Should_be_able_to_update()
    {
        // Arrange
        var adjustedPrice = await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB8585",
              closingPrice: 1000,
              lastPrice: 500,
              dt: new DateTime(2025, 05, 06, 14, 50, 00));
        var dto = new AdjustedPriceUpdateRequestTest
        {
            AdjustedLastPrice = 250,
            AdjustedPrice = 850,
            AdjustedPriceId = adjustedPrice.Id,
            IsAdjusted = true,
        };

        // Act
        var response = await _client.PutAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<AdjustedPriceResponseTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsAdjusted.Should().Be(dto.IsAdjusted ?? false);
        apiResult.AdjustedLastPrice.Should().Be(dto.AdjustedLastPrice);
        apiResult.AdjustedPrice.Should().Be(dto.AdjustedPrice);
        apiResult.Id.Should().Be(dto.AdjustedPriceId);
    }


    [Fact]
    public async Task Should_be_able_to_update_with_capitalChangeAnnouncement()
    {
        // Arrange
        var dt = new DateTime(2025, 10, 28, 10, 50, 00);
        var codalCode = 8585;
        var announcement = await _factory.Repositories.AnnouncementAddAsync(codalCode, dtPublish: dt);
        var capitalChange = await _factory.Repositories.CapitalChangeAddAsync(announcement.AnnouncementIdPk,
            approvalType: true, dtEntry: dt, dtModify: dt);
        await _factory.Repositories.CapitalChangeItemsAddAsync(capitalChange.CapitalChangeIdPk, 1);
        await _factory.Repositories.CapitalChangeItemsAddAsync(capitalChange.CapitalChangeIdPk, 2);
        await _factory.Repositories.CapitalChangeItemsAddAsync(capitalChange.CapitalChangeIdPk, 3);
        await _factory.Repositories.CapitalChangeItemsAddAsync(capitalChange.CapitalChangeIdPk, 4);

        var adjustedPrice = await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB8585",
              closingPrice: 1000,
              lastPrice: 500,
              dt: dt);
        var dto = new AdjustedPriceUpdateRequestTest
        {
            CodalCode = codalCode,
            AdjustedLastPrice = 250,
            AdjustedPrice = 850,
            AdjustedPriceId = adjustedPrice.Id,
            IsAdjusted = true,
        };

        // Act
        var response = await _client.PutAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var apiResult = await response.Content.ReadModelFromJsonAsync<AdjustedPriceResponseTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsAdjusted.Should().Be(dto.IsAdjusted ?? false);
        apiResult.AdjustedLastPrice.Should().Be(dto.AdjustedLastPrice);
        apiResult.AdjustedPrice.Should().Be(dto.AdjustedPrice);
        apiResult.Id.Should().Be(dto.AdjustedPriceId);
        apiResult.CapitalChangeCodalCode.Should().Be(codalCode);
        apiResult.CapitalChangeId.Should().Be(capitalChange.CapitalChangeIdPk);
    }
}
