using MDF.DAL.Modules.Entities.OldEntities;
using MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Requests;
using MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Responses;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.FeaturesTest.AdjustedPriceGetListTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "adjusted-price-list")]
public partial class AdjustedPriceGetListTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/adjusted-price/list";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public AdjustedPriceGetListTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }

    [Theory]
    [InlineData(null, null)]
    public async Task Should_not_be_able_get_data_with_not_correct_filter(string? date, string isins)
    {
        //-ARRANGE
        AdjustedPriceGetListRequestTest dto = new AdjustedPriceGetListRequestTest
        {
            Date = date,
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
        apiResult.ErrorKey.Should().Be("AdjustedPriceGetListRequestException");
    }

    [Fact]
    public async Task Should_not_be_able_data_without_any_filter()
    {
        //-ARRANGE
        await AddRequierAsync();
        var dtoFilter = new AdjustedPriceGetListRequestTest();
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        apiResult.StatusCode.Should().Be(ETypeOfApiResultStatusCodeTest.BadRequest);
        apiResult.ErrorKey.Should().Be("AdjustedPriceGetListRequestException");
    }


    [Fact]
    public async Task Should_be_able_data_without_any_filter()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 07, 13, 14, 30, 0);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85851", dt: dt, closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85852", dt: dt, closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85853", dt: dt, closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85854", dt: dt, closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85855", dt: dt, closingPrice: 10, lastPrice: 12);

        var dtoFilter = new AdjustedPriceGetListRequestTest()
        {
            Date = "2025-07-13",
            Isins = "IRB85851,IRB85852,IRB85853,IRB85854,IRB85855"
        };
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<IEnumerable<AdjustedPriceResponseTest>>();
        apiResult.Should().NotBeNull();
        foreach (var item in apiResult)
        {
            item.Date.Should().Be("2025-07-13");
            dtoFilter.Isins.Contains(item.SymbolIsin).Should().BeTrue();
        }
    }


    [Fact]
    public async Task Should_be_able_get_data_from_NegotiatedMarketIsin()
    {

        var companyType = await _factory.Repositories.CompanyTypeAddAsync(new CompanyType
        {
            Code = "A",
            EnTitle = "Share‌",
            Title = "شرکت"
        });
        var industrialCategoryParent = await _factory.Repositories
            .IndustrialCategoryAddAsync(new IndustrialCategory
            {
                Title = "انبوه سازي، املاك و مستغلات",
                Code = "70",
                ParentId = null
            });

        var industrialCategory = await _factory.Repositories
            .IndustrialCategoryAddAsync(new IndustrialCategory
            {
                Title = "املاك و مستغلات با ملك خود يا ليزينگ شده",
                Code = "7010",
                ParentId = industrialCategoryParent.IndustrialCategoryIdPk
            });

        var company = await _factory.Repositories
            .CompanyAddAsync(new Company
            {
                IndustrialCategoryIdFk = industrialCategory.IndustrialCategoryIdPk,
                CompanyTypeIdFk = companyType.CompanyTypeIdPk,
                Title = "‌‌‌‌‌ابنيه وساختمان پديده شانديز",
                CompanyCode = "ABNY",
                DateOfEvent = "2025-07-26T17:45:52".GetDateTimeFromISOFormat(),
                IsCompelete = true,
                IsImport = true
            });

        var instrumentType = await _factory.Repositories
            .InstrumentTypeAddAsync(new InstrumentType
            {
                Title = "Unknown",
                InstrumentTypeCode = 319
            });

        var instrument = await _factory.Repositories
            .InstrumentAddAsync(new Instrument
            {
                CompanyIdFk = company.CompanyIdPk,
                InstrumentTypeIdFk = instrumentType.InstrumentTypeIdPk,
                EnTitle = "Padide Building",
                Isin = "IROTABNY0006",
                UnitCount = 13500000000,
                DateOfEvent = "2025-07-26T17:45:52".GetDateTimeFromISOFormat(),
                IsCompelete = true,
                IsImport = false,
                IsImported = false
            });

        var board = new Board
        {
            BoardCode = 7,
            Title = "فهرست مشروط"
        };
        var securityExchange = new SecuritiesExchange
        {
            Title = "بورس انرژی",
            Code = 0
        };

        var boardExchange = await _factory.Repositories
            .BoardExchangeAddAsync(board, securityExchange);


        var market = new Market
        {
            Title = "بازار عادی",
            EnTitle = "Normal Market",
            Code = "NO"
        };
        var security2 = new SecuritiesExchange
        {
            Title = "بازار فرابورس",
            Code = 3
        };

        var marketExchange = await _factory.Repositories
            .MarketExchangeAddAsync(market, security2);

        var symbolGroup = await _factory.Repositories
            .SymbolGroupAddAsync(title: "Unknown", code: "OT");

        var isin = "IROTABNY0001";
        var symbol = await _factory.Repositories.SymbolAddAsync(new Symbol
        {

            InstrumentIdFk = instrument.InstrumentIdPk,
            ExchangeBoardIdFk = boardExchange.ExchangeBoardIdPk,
            ExchangeMarketIdFk = marketExchange.ExchangeMarketIdPk,
            SymbolGroupIdFk = symbolGroup.SymbolGroupIdPk,
            Title = "ابنيه وساختمان پديده شانديز",
            SymbolName = "ابنيه1",
            Isin = isin,
            EnSymbol = "ABNY1",
            MinQuantityOrder = 1,
            MaxQuantityOrder = 134999999,
            BaseVolume = 1,
            Lot = 1,
            DateOfEvent = "2023-10-15T19:11:56".GetDateTimeFromISOFormat(),
            EntryDate = "2023-10-14T19:12:00".GetDateTimeFromISOFormat(),
            IsCompelete = true,
            IsImport = false,
            IsImported = false,
            InstCodeTse = 7798165508516798,
            SymbolNameTse = "ابنیه وساختمان پدیده شاندیز",
            SymbolNameModified = "ابنيه",
            SymbolCodeTse = "ابنيه",
            SymbolCodeTseSafeEncoding = "ابنیه",
            FirmId = 367249,
            CdsSymbolName = "ابنيه",
            IsDisabled = false,
            DisableDateTime = null,
            CreatedDateTime = "2023-10-14T19:12:00".GetDateTimeFromISOFormat(),
            LastModifiedDate = "2025-07-23T17:45:56".GetDateTimeFromISOFormat(),
            TypeOfSymbol = (MDF.Modules.SymbolModule.Depricate.Abstractions.ETypeOfSymbol)1,
            TypeOfSymbolInTseTmc = 0
        });


        var adjustedPrice = await _factory.Repositories
            .AdjustedPriceAddAsync(isin: isin,
            closingPrice: 120,
            lastPrice: 300,
            dt: "2025-10-28".GetDateTimeFromISOFormat());

        var dtoFilter = new AdjustedPriceGetListRequestTest()
        {
            Date = "2025-10-28",
            Isins = isin
        };
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<IEnumerable<AdjustedPriceResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Should().HaveCount(1);
        apiResult.First().SymbolIsin.Should().Be(isin);
        apiResult.First().AdjustedLastPrice.Should().Be(300);
        apiResult.First().LastTradedPrice.Should().Be(300);
        apiResult.First().AdjustedPrice.Should().Be(120);
        apiResult.First().ClosingPrice.Should().Be(120);
    }


    // در این سناریو اگر قیمت سود سلف در اون روز از قیمت سلف
    // توی بازار بیشتر باشه باید اونو بدیم و همچنین تگ 
    // IsAdJusted ‌هم ترو باشه
    [Fact]
    public async Task Should_be_able_get_salaf_price_when_profitSalaf_increase_from_priceInMarket()
    {
        var dt = "2025-10-28".GetDateFromISOFormat();
        var isin = "IRBKMLG20481";
        var instrument = await _factory.Repositories.InstrumentAddAsync(title: "اوراق سلف موازی کالایی کماسه");
        var symbol = await _factory.Repositories.SymbolAddAsync(isin: isin, instrumentId: instrument.InstrumentIdPk, typeOfSymbol: TypeOfSymbolTest.Bond_Salaf);
        var adjustedPrice = await _factory.Repositories.AdjustedPriceAddAsync(isin: isin,
            closingPrice: 2651260,
            lastPrice: 2651260, dt: dt);
        var fixedIncome = await _factory.Repositories.FixedIncomeAddAsync(
            instrumentId: instrument.InstrumentIdPk,
            dtPublish: dt.AddDays(-10),
            dtSubscriptionStart: dt.AddDays(-10));

        var salaf = await _factory.Repositories.SalafFixedIncomeAddAsync(
            FixedIncomeIdPk: fixedIncome.FixedIncomeIdPk,
            EachContractAmount: 1000000);

        var profitSalaf = await _factory.Repositories.SalafProfitAddAsync(
            fixedIncomeId: fixedIncome.FixedIncomeIdPk,
            dtOfEvent: dt,
            price: 3345585.87704918m // قیمت سود سلف بیشتر از قیمت بازار
            );

        var dtoFilter = new AdjustedPriceGetListRequestTest()
        {
            Date = "2025-10-28",
            Isins = isin
        };
        var api = $"{_api}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<IEnumerable<AdjustedPriceResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Should().HaveCount(1);
        var adjustedPriceResponse = apiResult.First();
        adjustedPriceResponse.SymbolIsin.Should().Be(isin);
        adjustedPriceResponse.AdjustedLastPrice.Should().Be(3345585);
        adjustedPriceResponse.LastTradedPrice.Should().Be(2651260);
        adjustedPriceResponse.AdjustedPrice.Should().Be(3345585);
        adjustedPriceResponse.ClosingPrice.Should().Be(2651260);
        adjustedPriceResponse.IsAdjusted.Should().BeTrue();
    }


}
