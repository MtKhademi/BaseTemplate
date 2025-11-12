using MDF.Test.Integration.Modules.SymbolModuleTest.SymbolTest.Responses;
using System.Net;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.SymbolTest.FeaturesTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "get-by-isin")]
public partial class SymbolGetByIsinTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public SymbolGetByIsinTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }



    [Theory]
    [InlineData("05", "ISIN must be at least 7 characters long.")]
    public async Task Should_not_be_able_get_data_with_not_correct_filter(string isin, string errorMessage)
    {
        //-ARRANGE
        var api = $"{_api}{isin}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Assertion([errorMessage]);
    }


    [Fact]
    public async Task Should_not_be_able_get_data_with_not_exist_symbol()
    {
        //-ARRANGE
        var api = $"{_api}IRK123123123";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Assertion(["there is not exist any SymbolEntity =\u003E Isin : IRK123123123"]);
    }


    [Fact]
    public async Task Should_be_able_get_symbol()
    {
        //-ARRANGE
        var isin = "IRKOC8585";
        var dtEntry = DateTime.Parse("2025-09-07T13:06:00");

        var industrial = await _factory.Repositories.IndustrialCategoryAddAsync(
            code: "IND1", title: "Industrial category1");

        var industrial2 = await _factory.Repositories.IndustrialCategoryAddAsync(
            code: "IND2", title: "Industrial category2", parentId: industrial.IndustrialCategoryIdPk);

        var company = await _factory.Repositories.CompanyAddAsync(
            code: "COM", title: "COMPANY-NAME",
            industrialCategoryId: industrial2.IndustrialCategoryIdPk);

        var instrument = await _factory.Repositories.InstrumentAddAsync(
            title: "INST-NAME",
            isin: "IRINST12345",
            companyId: company.CompanyIdPk,
            unitCount: 200);

        var exchangeBoard = await _factory.Repositories.BoardExchangeAddAsync(boardCode: 1, securitiesExchange: 1);
        var exchangeMarket = await _factory.Repositories.MarketExchangeAddAsync(marketCode: "NO", securitiesExchangeCode: 1);

        await _factory.Repositories.SymbolAddAsync(
            isin: isin,
            cdsSymbolName: "CDS-SYMBOL-NAMe",
            instrumentId: instrument.InstrumentIdPk,
            dtEntry: dtEntry,
            dtEvent: dtEntry,
            exchangeBoardId: exchangeBoard.ExchangeBoardIdPk,
            exchangeMarketId: exchangeMarket.ExchangeMarketIdPk
            );


        //-ACT
        var api = $"{_api}{isin}";
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var symbolGet = await response.Content.ReadModelFromJsonAsync<SymbolResponseTest>();

        symbolGet.Should().NotBeNull();
        symbolGet.Isin.Should().Be("IRKOC8585");
        symbolGet.BourseCode.Should().BeNull();
        symbolGet.FirmId.Should().Be(1);
        symbolGet.IsActive.Should().BeTrue();
        symbolGet.IsDisabled.Should().BeFalse();
        symbolGet.DisableDateTime.Should().Be("");
        symbolGet.MarketCode.Should().Be("NO");
        symbolGet.MarketName.Should().Be("Market Title");
        symbolGet.BoardCode.Should().NotBeNull();
        symbolGet.BoardName.Should().NotBeNull();
        symbolGet.SecurityExchangeNameForExchangeMarket.Should().NotBeNull();
        symbolGet.SecurityExchangeCodeForExchangeMarket.Should().NotBeNull();
        symbolGet.SecurityExchangeNameForExchangeBoard.Should().NotBeNull();
        symbolGet.SecurityExchangeCodeForExchangeBoard.Should().NotBeNull();
        symbolGet.TypeOfSymbolPersianName.Should().Be("نامشخص");
        symbolGet.BaseVolume.Should().BeNull();
        symbolGet.EnSymbol.Should().Be("EN-SYMBOL");
        symbolGet.Lot.Should().Be(0);
        symbolGet.MaxQuantityOrder.Should().Be(0);
        symbolGet.MinQuantityOrder.Should().Be(0);
        symbolGet.SymbolNameTse.Should().BeNull();
        symbolGet.Title.Should().BeNull();
        symbolGet.CdsSymbolName.Should().Be("CDS-SYMBOL-NAMe");
        symbolGet.CompanyCode.Should().Be("COM");
        symbolGet.CompanyName.Should().Be("COMPANY-NAME");
        symbolGet.DateOfEvent.Should().Be("2025-09-07");
        symbolGet.EntryDate.Should().Be("2025-09-07");
        symbolGet.BoardCode.Should().Be(1);
        symbolGet.BoardName.Should().Be("تابلو اصلي");
        symbolGet.SecurityExchangeCodeForExchangeMarket.Should().Be(1);
        symbolGet.SecurityExchangeNameForExchangeMarket.Should().Be("Securities Exchange Title");
        symbolGet.SecurityExchangeCodeForExchangeBoard.Should().Be(1);
        symbolGet.SecurityExchangeNameForExchangeBoard.Should().Be("بازار بورس");

        symbolGet.IndustrialCategoryCode.Should().Be("IND2");
        symbolGet.IndustrialCategoryTitle.Should().Be("Industrial category2");
        symbolGet.IndustrialCategoryId.Should().Be(industrial2.IndustrialCategoryIdPk.ToString());
        symbolGet.IndustrialCategoryParentCode.Should().Be("IND1");
        symbolGet.IndustrialCategoryParentId.Should().Be(industrial.IndustrialCategoryIdPk);
        symbolGet.IndustrialCategoryParentTitle.Should().Be("Industrial category1");

        symbolGet.InstrumentISIN.Should().Be("IRINST12345");
        symbolGet.InstrumentTitle.Should().Be("INST-NAME");
        symbolGet.InstrumentTypeCode.Should().Be(300);
        symbolGet.InstrumentType.Should().Be("سهام");
        symbolGet.InstrumentUnitCount.Should().Be(200);
    }



    [Fact]
    public async Task Should_be_able_get_symbol_with_marketChange()
    {
        //-ARRANGE
        var isin = "IRKOC8585";
        var newIsin = "IRKOC8586";
        var dtEntry = DateTime.Parse("2025-09-08T13:06:00");

        var industrial = await _factory.Repositories.IndustrialCategoryAddAsync(
            code: "IND1", title: "Industrial category1");

        var industrial2 = await _factory.Repositories.IndustrialCategoryAddAsync(
            code: "IND2", title: "Industrial category2", parentId: industrial.IndustrialCategoryIdPk);

        var company = await _factory.Repositories.CompanyAddAsync(
            code: "COM", title: "COMPANY-NAME",
            industrialCategoryId: industrial2.IndustrialCategoryIdPk);

        var instrument = await _factory.Repositories.InstrumentAddAsync(
            title: "INST-NAME",
            isin: "IRINST12345",
            companyId: company.CompanyIdPk,
            unitCount: 200);

        var exchangeBoard = await _factory.Repositories.BoardExchangeAddAsync(boardCode: 1, securitiesExchange: 1);
        var exchangeMarket = await _factory.Repositories.MarketExchangeAddAsync(marketCode: "NO", securitiesExchangeCode: 1);

        await _factory.Repositories.SymbolAddAsync(
            isin: isin,
            cdsSymbolName: "CDS-SYMBOL-NAMe",
            instrumentId: instrument.InstrumentIdPk,
            dtEntry: dtEntry,
            dtEvent: dtEntry,
            exchangeBoardId: exchangeBoard.ExchangeBoardIdPk,
            exchangeMarketId: exchangeMarket.ExchangeMarketIdPk,
            isDisable: true,
            instCodeTse: 125523
            );

        await _factory.Repositories.SymbolAddAsync(isin: newIsin);

        await _factory.Repositories.ChangeMarketAddAsync(
            fromIsin: isin,
            toIsin: newIsin,
            dt: dtEntry.AddDays(1));


        //-ACT
        var api = $"{_api}{isin}";
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var symbolGet = await response.Content.ReadModelFromJsonAsync<SymbolResponseTest>();

        symbolGet.Should().NotBeNull();
        symbolGet.Isin.Should().Be("IRKOC8585");
        symbolGet.BourseCode.Should().BeNull();
        symbolGet.FirmId.Should().Be(1);
        symbolGet.IsActive.Should().BeFalse();
        symbolGet.IsDisabled.Should().BeTrue();
        symbolGet.DisableDateTime.Should().Be("");
        symbolGet.MarketCode.Should().Be("NO");
        symbolGet.MarketName.Should().Be("Market Title");
        symbolGet.BoardCode.Should().NotBeNull();
        symbolGet.BoardName.Should().NotBeNull();
        symbolGet.SecurityExchangeNameForExchangeMarket.Should().NotBeNull();
        symbolGet.SecurityExchangeCodeForExchangeMarket.Should().NotBeNull();
        symbolGet.SecurityExchangeNameForExchangeBoard.Should().NotBeNull();
        symbolGet.SecurityExchangeCodeForExchangeBoard.Should().NotBeNull();
        symbolGet.TypeOfSymbolPersianName.Should().Be("نامشخص");
        symbolGet.BaseVolume.Should().BeNull();
        symbolGet.EnSymbol.Should().Be("EN-SYMBOL");
        symbolGet.Lot.Should().Be(0);
        symbolGet.MaxQuantityOrder.Should().Be(0);
        symbolGet.MinQuantityOrder.Should().Be(0);
        symbolGet.SymbolNameTse.Should().BeNull();
        symbolGet.Title.Should().BeNull();
        symbolGet.CdsSymbolName.Should().Be("CDS-SYMBOL-NAMe");
        symbolGet.CompanyCode.Should().Be("COM");
        symbolGet.CompanyName.Should().Be("COMPANY-NAME");
        symbolGet.DateOfEvent.Should().Be("2025-09-08");
        symbolGet.EntryDate.Should().Be("2025-09-08");
        symbolGet.BoardCode.Should().Be(1);
        symbolGet.BoardName.Should().Be("تابلو اصلي");
        symbolGet.SecurityExchangeCodeForExchangeMarket.Should().Be(1);
        symbolGet.SecurityExchangeNameForExchangeMarket.Should().Be("Securities Exchange Title");
        symbolGet.SecurityExchangeCodeForExchangeBoard.Should().Be(1);
        symbolGet.SecurityExchangeNameForExchangeBoard.Should().Be("بازار بورس");

        symbolGet.IndustrialCategoryCode.Should().Be("IND2");
        symbolGet.IndustrialCategoryTitle.Should().Be("Industrial category2");
        symbolGet.IndustrialCategoryId.Should().Be(industrial2.IndustrialCategoryIdPk.ToString());
        symbolGet.IndustrialCategoryParentCode.Should().Be("IND1");
        symbolGet.IndustrialCategoryParentId.Should().Be(industrial.IndustrialCategoryIdPk);
        symbolGet.IndustrialCategoryParentTitle.Should().Be("Industrial category1");

        symbolGet.InstrumentISIN.Should().Be("IRINST12345");
        symbolGet.InstrumentTitle.Should().Be("INST-NAME");
        symbolGet.InstrumentTypeCode.Should().Be(300);
        symbolGet.InstrumentType.Should().Be("سهام");
        symbolGet.InstrumentUnitCount.Should().Be(200);

        symbolGet.IsOldSymbol.Should().BeTrue();
        symbolGet.OldSymbolIsin.Should().BeNull();

        symbolGet.InstCodeTse.Should().Be(125523);


        //-ACT
        api = $"{_api}{newIsin}";
        response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var symbolNewGet = await response.Content.ReadModelFromJsonAsync<SymbolResponseTest>();

        symbolNewGet.IsOldSymbol.Should().BeFalse();
        symbolNewGet.OldSymbolIsin.Should().Be(isin);
    }

    [Theory]
    [InlineData("IRT1C", 1, "ETFTypeTitle_Complex", TypeOfSymbolTest.ETF_Energy)]
    [InlineData("IRT3C", 1, "ETFTypeTitle_Complex", TypeOfSymbolTest.ETF_Sector)]
    [InlineData("IRT3S", 2, "ETFTypeTitle_Stocks", TypeOfSymbolTest.ETF_Stock)]
    [InlineData("IRT32x", 0, null, TypeOfSymbolTest.Stock)]
    public async Task Should_be_able_get_symbol_with_EtfType(
        string perfixIsin,
        int etfType,
        string etfTypeTitle,
        TypeOfSymbolTest typeOfSymbol)
    {
        //-ARRANGE
        var isin = perfixIsin + "CX8585";
        var dtEntry = DateTime.Parse("2025-09-22");

        await _factory.Repositories.SymbolAddAsync(
            isin: isin,
            cdsSymbolName: "CDS-SYMBOL-NAMe",
            dtEntry: dtEntry,
            dtEvent: dtEntry,
            instCodeTse: 125523,
            typeOfSymbol: typeOfSymbol);


        //-ACT
        var api = $"{_api}{isin}";
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var symbolGet = await response.Content.ReadModelFromJsonAsync<SymbolResponseTest>();

        symbolGet.Should().NotBeNull();
        symbolGet.Isin.Should().Be(isin);
        symbolGet.EtfType.Should().Be(etfType);
        symbolGet.EtfTypeTitle.Should().Be(etfTypeTitle);

    }


    [Fact]
    public async Task Should_be_able_get_symbol_with_SymbolGroupData()
    {
        //-ARRANGE
        var isin = "IRBX1CX8585";
        var dtEntry = DateTime.Parse("2025-09-22");

        var symbolGroupId = await _factory.Repositories.SymbolGroupAddAsync(code: "1", title: "SymbolGroupTitle1");

        await _factory.Repositories.SymbolAddAsync(
            isin: isin,
            cdsSymbolName: "CDS-SYMBOL-NAMe",
            dtEntry: dtEntry,
            dtEvent: dtEntry,
            instCodeTse: 125523,
            symbolGroupId: symbolGroupId.SymbolGroupIdPk,
            settlementPeriod: 10);


        //-ACT
        var api = $"{_api}{isin}";
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var symbolGet = await response.Content.ReadModelFromJsonAsync<SymbolResponseTest>();

        symbolGet.Should().NotBeNull();
        symbolGet.Isin.Should().Be(isin);
        symbolGet.SymbolGroupId.Should().Be(symbolGroupId.SymbolGroupIdPk);
        symbolGet.SymbolGroupCode.Should().Be("1");
        symbolGet.SymbolGroupTitle.Should().Be("SymbolGroupTitle1");
        symbolGet.SettlementPeriod.Should().Be(10);

    }


    [Fact]
    public async Task Should_be_able_get_symbol_with_lastState()
    {
        //-ARRANGE
        var isin = "IRBX1CX8585";
        var dtEntry = DateTime.Parse("2025-11-09");

        var symbol = await _factory.Repositories.SymbolAddAsync(
              isin: isin,
              cdsSymbolName: "CDS-SYMBOL-NAMe",
              dtEntry: dtEntry.AddDays(-1),
              dtEvent: dtEntry.AddDays(-1),
              instCodeTse: 125523,
              settlementPeriod: 10);

        var symbolState = await _factory.Repositories.SymbolStateAddAsync(
            symbolId: symbol.SymbolIdPk,
            stateTypeCode: "A",
            tradingStateCode: "AD",
            stateActionCode: "AC",
            dateOfEvent: dtEntry);


        //-ACT
        var api = $"{_api}{isin}";
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var symbolGet = await response.Content.ReadModelFromJsonAsync<SymbolResponseTest>();

        symbolGet.Should().NotBeNull();
        symbolGet.Isin.Should().Be(isin);

    }
}
