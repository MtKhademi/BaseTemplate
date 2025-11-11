using MDF.Test.Integration.Modules.SymbolModuleTest.SymbolTest.Requests;
using MDF.Test.Integration.Modules.SymbolModuleTest.SymbolTest.Responses;
using System.Net;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.SymbolTest.FeaturesTest.SymbolCreateTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "create")]
public partial class SymbolCreateTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public SymbolCreateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }

    [Theory]
    [ClassData(typeof(NotCorrectDataTest))]
    public async Task Should_not_be_able_to_create_when_not_exist_instrument(SymbolCreateDtoTest dto, List<string> errorMessages)
    {
        // Arrange

        // Act
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Assertion(errorMessages);
    }


    [Fact]
    public async Task Should_be_able_to_create_a_symbol()
    {
        // Arrange
        var dtEvent = (new DateTime(2025, 05, 06, 14, 50, 00));
        var dto = new SymbolCreateDtoTest
        {
            BoardCode = 1,
            BoardName = "تابلو اصلي",

            MarketCode = "NO",
            MarketName = "بازار عادی",

            BoardSecurityExchangeCode = 1,
            MarketSecurityExchangeCode = 1,
            BaseVolume = 10,
            BourseCode = "BourseCode",
            CdsSymbolName = "CdsSymbolName",
            DateOfEvent = dtEvent.GetISOStringDateTime(),
            EnSymbol = "EnSymbol",

            SymbolGroupId = 1,
            InstrumentId = 1,
            Isin = "Isin8585XX",
            Lot = 10,
            MaxQuantityOrder = 10,
            MinQuantityOrder = 10,
            SymbolCodeTse = "SymbolCodeTse",
            SymbolCodeTseSafeEncoding = "SymbolCodeTseSafeEncoding",
            SymbolName = "SymbolName",
            SymbolNameModified = "SymbolNameModified",
            SymbolNameTse = "SymbolNameTse",
            Title = "Title",
            TypeOfSymbol = TypeOfSymbolTest.Bond_Salaf,
            TypeOfSymbolInTseTmc = TypeOfSymbolTest.Commodity
        };

        // Act
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var symbolCreated = await response.Content.ReadModelFromJsonAsync<SymbolResponseTest>();
        symbolCreated.Should().NotBeNull();
        symbolCreated.BaseVolume.Should().Be(dto.BaseVolume);
        symbolCreated.BourseCode.Should().Be(dto.BourseCode);
        symbolCreated.DateOfEvent.Should().Be("2025-05-06");
        symbolCreated.EnSymbol.Should().Be(dto.EnSymbol);
        symbolCreated.Isin.Should().Be(dto.Isin);
        symbolCreated.Lot.Should().Be(dto.Lot);
        symbolCreated.MaxQuantityOrder.Should().Be(dto.MaxQuantityOrder);
        symbolCreated.MinQuantityOrder.Should().Be(dto.MinQuantityOrder);
        symbolCreated.SymbolName.Should().Be(dto.SymbolName);
        symbolCreated.SymbolNameTse.Should().Be(dto.SymbolNameTse);
        symbolCreated.Title.Should().Be(dto.Title);
        symbolCreated.TypeOfSymbol.Should().Be(dto.TypeOfSymbol);
        symbolCreated.TypeOfSymbolInTseTmc.Should().Be(dto.TypeOfSymbolInTseTmc);
        symbolCreated.MarketCode.Should().Be(dto.MarketCode);
        symbolCreated.MarketName.Should().Be(dto.MarketName);
        symbolCreated.BoardCode.Should().Be(dto.BoardCode);
        symbolCreated.BoardName.Should().Be(dto.BoardName);
    }

    [Fact]
    public async Task Should_not_be_able_to_create_again_a_symbol()
    {
        // Arrange
        var dtEvent = (new DateTime(2025, 05, 06, 14, 50, 00));
        var dto = new SymbolCreateDtoTest
        {
            BoardCode = 1,
            BoardName = "تابلو اصلي",

            MarketCode = "NO",
            MarketName = "بازار عادی",

            BoardSecurityExchangeCode = 1,
            MarketSecurityExchangeCode = 1,
            BaseVolume = 10,
            BourseCode = "BourseCode",
            CdsSymbolName = "CdsSymbolName",
            DateOfEvent = dtEvent.GetISOStringDateTime(),
            EnSymbol = "EnSymbol",

            SymbolGroupId = 1,
            InstrumentId = 1,
            Isin = "Isin8585XX",
            Lot = 10,
            MaxQuantityOrder = 10,
            MinQuantityOrder = 10,
            SymbolCodeTse = "SymbolCodeTse",
            SymbolCodeTseSafeEncoding = "SymbolCodeTseSafeEncoding",
            SymbolName = "SymbolName",
            SymbolNameModified = "SymbolNameModified",
            SymbolNameTse = "SymbolNameTse",
            Title = "Title",
            TypeOfSymbol = TypeOfSymbolTest.Bond_Salaf,
            TypeOfSymbolInTseTmc = TypeOfSymbolTest.Commodity
        };

        // Act
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        response.Should().NotBeNull();  
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Assert
        response = await _client.PostAsync(_api, dto.ToContentHttpString());
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
       var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Assertion(new List<string>
        {
            "there is already exist a SymbolEntity => Isin : Isin8585XX"
        });

    }

}
