using MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Responses;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.FeaturesTest.FixedIncomeCreateTest;


[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "fixed-income-create")]
public partial class FixedIncomeCreateTest : BaseTest
{
    private readonly string _api = $"/api/V4/symbol/bound/fixed-income";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public FixedIncomeCreateTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }


    [Theory]
    [ClassData(typeof(FixedIncomeCreateNotValidData))]
    public async Task When_call_api_with_not_valid_data_Expect_get_bad_response(FixedIncomeCreateDtoV4Test dto, IEnumerable<string> messagesException)
    {
        //-ARRANGE

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Messages.Should().BeEquivalentTo(messagesException);
    }


    [Fact]
    public async Task When_not_exist_symbolIsin_Expect_get_Not_Found_Isin_response()
    {
        //-ARRANGE
        var dt = new DateTime(2024, 07, 29, 13, 43, 0);
        var isin = "IRBX8585";
        var dto = new FixedIncomeCreateDtoV4Test(
            SymbolIsin: isin,
            MaturityDate: "2025-06-22",
            SubscriptionEndDate: "2025-06-22",
            SubscriptionStartDate: "2025-06-22",
            TradeStartDate: "2025-06-22",
            PublicationDate: "2025-06-22");

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Messages.Should().Contain($"there is not exist any SymbolEntity => Isin : {isin}");
    }

    [Theory]
    [InlineData(0, null, null, "there is not exist any Organization => OrganizationIdPk : 0")]
    [InlineData(null, 0, null, "there is not exist any Organization => OrganizationIdPk : 0")]
    [InlineData(null, null, 0, "there is not exist any Organization => OrganizationIdPk : 0")]
    public async Task When_not_exist_publider_maker_qurantour_Expect_get_Not_Found_organization_response(
        int? marketMaker, int? publisher, int? gurantourId, string errorMessage)
    {
        //-ARRANGE
        await RequierDataTest1Async();
        var dt = new DateTime(2024, 07, 29, 13, 43, 0);
        var isin = "IRB5AE800064";
        var dto = new FixedIncomeCreateDtoV4Test(
                SymbolIsin: isin,
                MaturityDate: "2025-06-22",
                SubscriptionEndDate: "2025-06-22",
                SubscriptionStartDate: "2025-06-22",
                TradeStartDate: "2025-06-22",
                PublicationDate: "2025-06-22",
                Duration: 2,
                InterestRate: 10,
                RedeemedRate: 100,
                NominalValue: 100,
                InterestPaymentInterval: 20,
                MarketMakerId: marketMaker,
                PublisherId: publisher,
                GurantorId: gurantourId);

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Messages.Should().HaveCount(1);
        apiResult.Messages.Should().Contain(errorMessage);
    }



    [Fact]
    public async Task Should_be_able_create_bound()
    {
        //-ARRANGE
        var dt = new DateTime(2025, 08, 09, 12, 43, 0);
        var isin = "IRBX85858581";
        await _factory.Repositories.SymbolAddAsync(isin: isin, typeOfSymbol: TypeOfSymbolTest.Bond);

        var dto = new FixedIncomeCreateDtoV4Test(
            SymbolIsin: isin,
            MaturityDate: dt.AddYears(1).GetISOStringDate(),
            SubscriptionEndDate: dt.AddDays(10).GetISOStringDate(),
            SubscriptionStartDate: dt.AddDays(2).GetISOStringDate(),
            TradeStartDate: dt.AddDays(20).GetISOStringDate(),
            PublicationDate: dt.GetISOStringDate(),
            Duration: 2,
            InterestRate: 10,
            RedeemedRate: 100,
            NominalValue: 100,
            InterestPaymentInterval: 20,
            MarketMakerId: 1,
            PublisherId: 1,
            GurantorId: 1,
            Description: "توضیحات تست",
            CountOfPublished: 12
        );

        //-ACT
        var response = await _client.PostAsync(_api, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var fixedIncome = await response.Content.ReadModelFromJsonAsync<BoundResponseTest>();
        fixedIncome.Should().NotBeNull();

        // Check that the FixedIncome was actually created in the database
        fixedIncome.Should().NotBeNull();
        fixedIncome.SymbolIsin.Should().Be(isin);
        fixedIncome.MaturityDate.Should().Be(dt.AddYears(1).Date.GetISOStringDate());
        fixedIncome.SubscriptionEndDate.Should().Be(dt.AddDays(10).Date.GetISOStringDate());
        fixedIncome.SubscriptionStartDate.Should().Be(dt.AddDays(2).Date.GetISOStringDate());
        fixedIncome.TradeStartDate.Should().Be(dt.AddDays(20).Date.GetISOStringDate());
        fixedIncome.PublicationDate.Should().Be(dt.Date.GetISOStringDate());
        fixedIncome.Duration.Should().Be(2);
        fixedIncome.InterestRate.Should().Be(10);
        fixedIncome.RedeemedRate.Should().Be(100);
        fixedIncome.NominalValue.Should().Be(100);
        fixedIncome.InterestPaymentInterval.Should().Be(20);
        fixedIncome.MarketMakerId.Should().Be(1);
        fixedIncome.PublisherId.Should().Be(1);
        fixedIncome.GurantorId.Should().Be(1);
        fixedIncome.PublisherName.Should().Be("وزارت امور اقتصادی و دارایی");
        fixedIncome.MarketMakerName.Should().Be("وزارت امور اقتصادی و دارایی");
        fixedIncome.GurantorName.Should().Be("وزارت امور اقتصادی و دارایی");
        fixedIncome.Description.Should().Be("توضیحات تست");
        fixedIncome.CountOfPublished.Should().Be(12);
    }
}
