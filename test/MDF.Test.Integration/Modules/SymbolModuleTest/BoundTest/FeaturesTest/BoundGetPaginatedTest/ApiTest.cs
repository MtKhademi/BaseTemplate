using MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Requests;
using MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Responses;

namespace MDF.Test.Integration.SUTS.APIS.BoundTest.FeaturesTest.BoundGetPaginatedTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "bound-paginated")]
public partial class BoundGetPaginatedTest : BaseTest
{
    private readonly string _apiGetTable = $"/api/v4/symbol/bound/paginated";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public BoundGetPaginatedTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }





    [Fact]
    public async Task When_call_api_without_filter_Expect_get_ok_response_and_currect_data()
    {
        //-ARRANGE
        var dtoFilter = new FixedIncomeTableFilterDtoV4TestBuilder().Build();
        var api = $"{_apiGetTable}?{dtoFilter.ToQueryString()}";

        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<PaginatedList<BoundResponseTest>>();
        apiResult.Should().NotBeNull();
        //apiResult.Data.Content.Should().HaveCount(count);

    }

    [Theory]
    [InlineData(true, null)]
    [InlineData(null, "IRB5AE800002,IRB5AE800003")]
    public async Task Should_get_data_with_filter(
        bool? IsLoadInterestPaymentInterval = null,
        string? isins = default!)
    {
        //-ARRANGE
        await FixIncomeGetTableApiTestRequierAsync();
        var filterDto = new BoundGetPaginatedListRequestTest
        {
            IsLoadInterestPaymentInterval = IsLoadInterestPaymentInterval,
            Isins = isins
        };

        var api = $"{_apiGetTable}?{filterDto.ToQueryString()}";
        List<string> isinsForChecking = string.IsNullOrWhiteSpace(filterDto.Isins) ? [] : filterDto.Isins.Split(",").ToList();


        //-ACT
        var response = await _client.GetAsync(api);
        await response.WriteOnConsoleAsync(_outPutHelper);


        //-ASSERT
        var apiResult = await response.Content.ReadModelFromJsonAsync<PaginatedList<BoundResponseTest>>();
        apiResult.Should().NotBeNull();
        apiResult.Data.Should().NotBeNull();

        foreach (var bound in apiResult.Data)
        {
            if (filterDto.IsLoadInterestPaymentInterval.HasValue)
            {
                if (filterDto.IsLoadInterestPaymentInterval.Value)
                {
                    bound.InterestPaymentDates.Should().NotBeNull();
                }
                else
                {
                    bound.InterestPaymentDates.Should().BeNull();
                }
            }

            if (isinsForChecking.Any())
            {
                isinsForChecking.Should().Contain(bound.SymbolIsin);
            }
        }

    }


    private async Task FixIncomeGetTableApiTestRequierAsync()
    {

        await _factory.Repositories.SymbolAddAsync("IRB5AE800002", typeOfSymbol: TypeOfSymbolTest.Bond);
        await _factory.Repositories.SymbolAddAsync("IRB5AE800003", typeOfSymbol: TypeOfSymbolTest.Stock);

        var dt = new DateTime(2024, 07, 23, 08, 50, 0);
        var instrument = await _factory.Repositories.InstrumentAddAsync(title: "Instrument Title");
        var symbol = await _factory.Repositories.SymbolAddAsync("IRB5AE800041", instrumentId: instrument.InstrumentIdPk,
            typeOfSymbol: TypeOfSymbolTest.Bond_Ejare);
        var fixedIncome = await _factory.Repositories.FixedIncomeAddAsync(
               instrumentId: instrument.InstrumentIdPk,
               dtPublish: dt.AddDays(1),
               dtEntry: dt,
               dtModify: dt,
               dtMaturity: dt.AddDays(5),
               dtSubscriptionEnd: dt.AddDays(10),
               dtSubscriptionStart: dt.AddDays(2),
               dtTradeStart: dt.AddDays(50));

        await _factory.Repositories.OrganizationAddAsync(organizationId: 23, title: "Publisher");
        var partyType = await _factory.Repositories.PartyTypeAddAsync("NEW TITLE", "1");
        await _factory.Repositories.PartyAddAsync(23, partyType.PartyTypeIdPk);
        await _factory.Repositories.FixedIncomeConstituentsAddAsync(fixedIncome.FixedIncomeIdPk, 23,
             ETypeOfFixedIncomeConstituentTypeTest.Publisher);

        await _factory.Repositories.OrganizationAddAsync(organizationId: 24, title: "MarketMaker");
        await _factory.Repositories.PartyAddAsync(24, partyType.PartyTypeIdPk);
        await _factory.Repositories.FixedIncomeConstituentsAddAsync(fixedIncome.FixedIncomeIdPk, 24,
            ETypeOfFixedIncomeConstituentTypeTest.MarketMaker);



        //================= with fixed income data
        instrument = await _factory.Repositories.InstrumentAddAsync(title: "Instrument Title2");
        await _factory.Repositories.SymbolAddAsync("IRB5AE800001", instrumentId: instrument.InstrumentIdPk,
            typeOfSymbol: TypeOfSymbolTest.Bond_Ejare);

        dt = new DateTime(2024, 07, 23, 08, 50, 0);
        fixedIncome = await _factory.Repositories.FixedIncomeAddAsync(instrument.InstrumentIdPk,
            dtEntry: dt,
            dtModify: dt,
            dtPublish: dt.AddDays(1),
            dtMaturity: dt.AddDays(5),
            dtSubscriptionEnd: dt.AddDays(10),
            dtSubscriptionStart: dt.AddDays(2),
            dtTradeStart: dt.AddDays(50));

        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncome.FixedIncomeIdPk, dt.AddDays(10), isHasAdjusted: true);
        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncome.FixedIncomeIdPk, dt.AddDays(20), isHasAdjusted: true);
        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncome.FixedIncomeIdPk, dt.AddDays(30), isHasAdjusted: true);
        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncome.FixedIncomeIdPk, dt.AddDays(40), isHasAdjusted: true);
        await _factory.Repositories.FixedIncomeProfitDailyAddAsync(fixedIncome.FixedIncomeIdPk, dt.AddDays(50), isHasAdjusted: true);


        //================= this symbol is not in market NO and It has to filter
        await _factory.Repositories.SymbolAddAsync("IRB5AE800031", instrumentId: instrument.InstrumentIdPk,
            typeOfSymbol: TypeOfSymbolTest.BondCallOption);



    }
}
