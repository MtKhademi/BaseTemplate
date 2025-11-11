using MDF.Test.Integration.Modules.SymbolModuleTest.IndexTest.Requests;
using MDF.Test.Integration.Modules.SymbolModuleTest.IndexTest.Responses;
using System.Net;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.IndexTest.FeaturesTest.IndexPricePaginatedListTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "index-price")]
public partial class IndexPricePaginatedListTest : BaseTest
{
    private readonly string _api = $"/api/v4/symbol/index/price";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public IndexPricePaginatedListTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }



    [Fact]
    public async Task Should_be_able_get_index_price()
    {
        //-ARRANGE
        var isin = "IRKOC8585";
        var dtEntry = "2025-09-24".GetDateFromISOFormat();
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dtEntry.AddDays(-1));
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dtEntry);


        var symbolEntity = await _factory.Repositories.SymbolAddAsync(
              isin: isin,
              cdsSymbolName: "CDS-SYMBOL-NAMe",
              dtEntry: dtEntry.AddDays(-1),
              dtEvent: dtEntry.AddDays(-1),
              typeOfSymbol: TypeOfSymbolTest.Index);

        var indexEntity = await _factory.Repositories.IndexDataAddAsync(symbolId: symbolEntity.SymbolIdPk,
             indexData: 120, dtOfEvent: dtEntry.AddMinutes(1));

        var indexEntity2 = await _factory.Repositories.IndexDataAddAsync(symbolId: symbolEntity.SymbolIdPk,
            indexData: 100, dtOfEvent: dtEntry.AddMinutes(3));

        var indexEntity3 = await _factory.Repositories.IndexDataAddAsync(symbolId: symbolEntity.SymbolIdPk,
            indexData: 150, dtOfEvent: dtEntry.AddMinutes(5));


        //-ACT
        var api = $"{_api}";
        var response = await _client.GetAsync(api + $"?{new IndexPaginatedListRequestTest
        {
            Isin = isin,
            OnlyGetLastChange = false,
            StartDateTime = "2025-09-24",
            EndDateTime = "2025-09-24"
        }.ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var symbolGet = await response.Content.ReadModelFromJsonAsync<PaginatedList<IndexResponseTest>>();

        symbolGet.Should().NotBeNull();
        symbolGet.TotalItems.Should().Be(3);
        //symbolGet.TotalPages.Should().Be(1);
        //symbolGet.CurrentPage.Should().Be(1);
        //symbolGet.SizeOfPage.Should().Be(50);
        symbolGet.HasPreviousPage.Should().BeFalse();
        symbolGet.HasNextPage.Should().BeFalse();

        symbolGet.Data.Should().NotBeNull();
        symbolGet.Data.Should().HaveCount(3);

        var item = symbolGet.Data.ToList()[2];
        item.IndexDataId.Should().Be(indexEntity.IndexDataIdPk);
        item.SymbolId.Should().Be(symbolEntity.SymbolIdPk);
        item.DateOfEvent.Should().Be("2025-09-24T00:01:00");
        item.IndexLevelId.Should().Be(0);
        item.IndexValue.Should().Be(120);
        item.SymbolIsin.Should().Be("IRKOC8585");
        item.SymbolName.Should().Be("SYMBOL-NAME");
        item.IndexLevelCode.Should().Be("");
        item.IndexLevelTitle.Should().Be("");
        item.PercentVariation.Should().Be(0);
        item.SignVariation.Should().Be(0);


        item = symbolGet.Data.ToList()[1];
        item.IndexDataId.Should().Be(indexEntity2.IndexDataIdPk);
        item.SymbolId.Should().Be(symbolEntity.SymbolIdPk);
        item.DateOfEvent.Should().Be("2025-09-24T00:03:00");
        item.IndexLevelId.Should().Be(0);
        item.IndexValue.Should().Be(100);
        item.SymbolIsin.Should().Be("IRKOC8585");
        item.SymbolName.Should().Be("SYMBOL-NAME");
        item.IndexLevelCode.Should().Be("");
        item.IndexLevelTitle.Should().Be("");
        item.PercentVariation.Should().Be(0);
        item.SignVariation.Should().Be(0);

        item = symbolGet.Data.ToList()[0];
        item.IndexDataId.Should().Be(indexEntity3.IndexDataIdPk);
        item.SymbolId.Should().Be(symbolEntity.SymbolIdPk);
        item.DateOfEvent.Should().Be("2025-09-24T00:05:00");
        item.IndexLevelId.Should().Be(0);
        item.IndexValue.Should().Be(150);
        item.SymbolIsin.Should().Be("IRKOC8585");
        item.SymbolName.Should().Be("SYMBOL-NAME");
        item.IndexLevelCode.Should().Be("");
        item.IndexLevelTitle.Should().Be("");
        item.PercentVariation.Should().Be(0);
        item.SignVariation.Should().Be(0);

    }

    [Fact]
    public async Task Should_be_able_get_Last_Index_data()
    {
        //-ARRANGE
        var isin = "IRKOC8585";
        var dtEntry = "2025-09-23".GetDateFromISOFormat();

        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dtEntry.AddDays(-1));
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dtEntry);


        var symbolEntity = await _factory.Repositories.SymbolAddAsync(
              isin: isin,
              cdsSymbolName: "CDS-SYMBOL-NAMe",
              dtEntry: dtEntry.AddDays(-1),
              dtEvent: dtEntry.AddDays(-1),
              typeOfSymbol: TypeOfSymbolTest.Index);

        var indexEntity = await _factory.Repositories.IndexDataAddAsync(symbolId: symbolEntity.SymbolIdPk,
             indexData: 120, dtOfEvent: dtEntry.AddMinutes(1));

        var indexEntity2 = await _factory.Repositories.IndexDataAddAsync(symbolId: symbolEntity.SymbolIdPk,
            indexData: 100, dtOfEvent: dtEntry.AddMinutes(3));

        var indexEntity3 = await _factory.Repositories.IndexDataAddAsync(symbolId: symbolEntity.SymbolIdPk,
            indexData: 150, dtOfEvent: dtEntry.AddMinutes(5),
            PercentVariation: -152.25,
            SignVariation: 12);


        //-ACT
        var api = $"{_api}";
        var response = await _client.GetAsync(api + $"?{new IndexPaginatedListRequestTest
        {
            Isin = isin,
            OnlyGetLastChange = true,
            StartDateTime = "2025-09-23",
            EndDateTime = "2025-09-23"
        }.ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var symbolGet = await response.Content.ReadModelFromJsonAsync<PaginatedList<IndexResponseTest>>();

        symbolGet.Should().NotBeNull();
        symbolGet.TotalItems.Should().Be(1);
        //symbolGet.TotalPages.Should().Be(1);
        //symbolGet.CurrentPage.Should().Be(1);
        //symbolGet.SizeOfPage.Should().Be(50);
        symbolGet.HasPreviousPage.Should().BeFalse();
        symbolGet.HasNextPage.Should().BeFalse();

        symbolGet.Data.Should().NotBeNull();
        symbolGet.Data.Should().HaveCount(1);

        var item = symbolGet.Data.First();
        item.IndexDataId.Should().Be(indexEntity3.IndexDataIdPk);
        item.SymbolId.Should().Be(symbolEntity.SymbolIdPk);
        item.DateOfEvent.Should().Be("2025-09-23T00:05:00");
        item.IndexLevelId.Should().Be(0);
        item.IndexValue.Should().Be(150);
        item.SymbolIsin.Should().Be("IRKOC8585");
        item.SymbolName.Should().Be("SYMBOL-NAME");
        item.IndexLevelCode.Should().Be("");
        item.IndexLevelTitle.Should().Be("");
        item.PercentVariation.Should().Be(-152.25);
        item.SignVariation.Should().Be(0);

    }



    [Fact]
    public async Task Should_get_with_last_date_data()
    {
        //-ARRANGE
        var isin = "IRKOC8585";
        var dtEntry = "2025-09-30".GetDateFromISOFormat();

        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dtEntry.AddDays(-1));
        await _factory.Repositories.CalendarAddAsync(dtOfEvent: dtEntry);


        var symbolEntity2 = await _factory.Repositories.SymbolAddAsync(
          isin: "INDEXIRKOC8585",
          cdsSymbolName: "CDS-SYMBOL-NAMe",
          dtEntry: dtEntry.AddDays(-1),
          dtEvent: dtEntry.AddDays(-1),
          typeOfSymbol: TypeOfSymbolTest.Index);

        var symbolEntity = await _factory.Repositories.SymbolAddAsync(
              isin: isin,
              cdsSymbolName: "CDS-SYMBOL-NAMe",
              dtEntry: dtEntry.AddDays(-1),
              dtEvent: dtEntry.AddDays(-1),
              typeOfSymbol: TypeOfSymbolTest.Index);

        await _factory.Repositories.IndexDataAddAsync(symbolId: symbolEntity.SymbolIdPk,
            indexData: 420, dtOfEvent: dtEntry.AddDays(-1).AddMinutes(3));
        await _factory.Repositories.IndexDataAddAsync(symbolId: symbolEntity.SymbolIdPk,
           indexData: 550, dtOfEvent: dtEntry.AddDays(-1).AddMinutes(10));

        var indexEntity = await _factory.Repositories.IndexDataAddAsync(symbolId: symbolEntity.SymbolIdPk,
             indexData: 120, dtOfEvent: dtEntry.AddMinutes(1));

        var indexEntity2 = await _factory.Repositories.IndexDataAddAsync(symbolId: symbolEntity.SymbolIdPk,
            indexData: 100, dtOfEvent: dtEntry.AddMinutes(3));

        var indexEntity3 = await _factory.Repositories.IndexDataAddAsync(symbolId: symbolEntity.SymbolIdPk,
            indexData: 150, dtOfEvent: dtEntry.AddMinutes(5),
            PercentVariation: -152.25,
            SignVariation: 12);


        var indexEntit4 = await _factory.Repositories.IndexDataAddAsync(symbolId: symbolEntity2.SymbolIdPk,
            indexData: 20, dtOfEvent: dtEntry.AddMinutes(10),
            PercentVariation: -152.25,
            SignVariation: 12);


        //-ACT
        var api = $"{_api}";
        var response = await _client.GetAsync(api + $"?{new IndexPaginatedListRequestTest
        {
            Isin = isin,
            OnlyGetLastChange = true,
            StartDateTime = "2025-09-30",
            EndDateTime = "2025-09-30"
        }.ToQueryString()}");
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var symbolGet = await response.Content.ReadModelFromJsonAsync<PaginatedList<IndexResponseTest>>();

        symbolGet.Should().NotBeNull();
        symbolGet.TotalItems.Should().Be(1);
        //symbolGet.TotalPages.Should().Be(1);
        //symbolGet.CurrentPage.Should().Be(1);
        //symbolGet.SizeOfPage.Should().Be(50);
        symbolGet.HasPreviousPage.Should().BeFalse();
        symbolGet.HasNextPage.Should().BeFalse();

        symbolGet.Data.Should().NotBeNull();
        symbolGet.Data.Should().HaveCount(1);

        var item = symbolGet.Data.First();
        item.IndexDataId.Should().Be(indexEntity3.IndexDataIdPk);
        item.SymbolId.Should().Be(symbolEntity.SymbolIdPk);
        item.DateOfEvent.Should().Be("2025-09-30T00:05:00");
        item.IndexLevelId.Should().Be(0);
        item.IndexValue.Should().Be(150);
        item.SymbolIsin.Should().Be("IRKOC8585");
        item.SymbolName.Should().Be("SYMBOL-NAME");
        item.IndexLevelCode.Should().Be("");
        item.IndexLevelTitle.Should().Be("");
        item.PercentVariation.Should().Be(-152.25);
        item.SignVariation.Should().Be(0);
        item.IndexChange.Should().NotBeNull();
        item.IndexChange.Should().Be(-400); // 550 - 150 = 400

    }
}
