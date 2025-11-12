namespace Test.Integration.ModulesTest.SymbolModuleTest.BoundTest.FeaturesTest.SalafAddApiTest;

[Collection(TestConstants.COLLECTION_NAME)]
[Trait("SYMBOL", "bound-salaf-add")]
public partial class FixedIncomeSalafAddOrUpdateApiTest : BaseTest
{
    private readonly string _apiAddress = $"/api/V4/symbol/bound/salaf";
    private readonly WebAppFactory _factory;
    private HttpClient _client;
    private readonly ITestOutputHelper _outPutHelper;
    public FixedIncomeSalafAddOrUpdateApiTest(WebAppFactory factory, ITestOutputHelper outPutHelper) : base(factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", "TEST_TOKEN");
        _outPutHelper = outPutHelper;
    }



    [Theory]
    [ClassData(typeof(FixedIncomeSalafAddOrUpdateDtoNotValidData))]
    public async Task When_call_api_with_not_valid_data_Expect_get_bad_response(FixedIncomeSalafAddOrUpdateDtoV2Test dto, IEnumerable<string> messagesException)
    {
        //-ARRANGE

        //-ACT
        var response = await _client.PostAsync(_apiAddress, dto.ToContentHttpString());
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
        var isin = "IRB5AE800063";
        var dto = new FixedIncomeSalafAddOrUpdateDtoV2TestBuilder()
            .WithInterestRate(10)
            .WithRedeemedRate(100)
            .WithDuration(2)
            .WithNominalValue(100)
            .WithInterestPaymentInterval(20)
            .WithSymbolIsin(isin)
            .WithTradeStartDate(dt.GetISOStringDateTime())
            .WithMaturityDate(dt.GetISOStringDateTime())
            .WithPublicationDate(dt.GetISOStringDateTime())
            .WithSubscriptionEndDate(dt.GetISOStringDateTime())
            .WithSubscriptionStartDate(dt.GetISOStringDateTime())
            .WithSecondaryTradeEndDate(dt.GetISOStringDateTime())
            .WithSecondaryTradeStartDate(dt.GetISOStringDateTime())
            .WithEachContractAmount(10)
            .WithBuyConsequentialPrice(10)
            .WithSellConsequentialPrice(10)
            .WithEachTonIPOPrice(10)
            .WithEachTonNominalPrice(10)
            .Build();

        //-ACT
        var response = await _client.PostAsync(_apiAddress, dto.ToContentHttpString());
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
        var dto = new FixedIncomeSalafAddOrUpdateDtoV2TestBuilder()
            .WithInterestRate(10)
            .WithRedeemedRate(100)
            .WithDuration(2)
            .WithNominalValue(100)
            .WithInterestPaymentInterval(20)
            .WithSymbolIsin(isin)
            .WithTradeStartDate(dt.GetISOStringDateTime())
            .WithMaturityDate(dt.GetISOStringDateTime())
            .WithPublicationDate(dt.GetISOStringDateTime())
            .WithSubscriptionEndDate(dt.GetISOStringDateTime())
            .WithSubscriptionStartDate(dt.GetISOStringDateTime())
            .WithSecondaryTradeEndDate(dt.GetISOStringDateTime())
            .WithSecondaryTradeStartDate(dt.GetISOStringDateTime())
            .WithEachContractAmount(10)
            .WithBuyConsequentialPrice(10)
            .WithSellConsequentialPrice(10)
            .WithEachTonIPOPrice(10)
            .WithEachTonNominalPrice(10)
            .WithMarketMakerId(marketMaker)
            .WithPublisherId(publisher)
            .WithGurantorId(gurantourId)
            .Build();

        //-ACT
        var response = await _client.PostAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Should().NotBeNull();
        apiResult.Messages.Should().HaveCount(1);
        apiResult.Messages.Should().Contain(errorMessage);
    }




    [Fact]
    public async Task Should_be_able_insert_salaf_data_for_first_time()
    {
        //-ARRANGE
        await RequierDataTest1Async();
        var dt = new DateTime(2024, 07, 29, 13, 43, 0);
        var isin = "IRB5AE800061";
        var dto = new FixedIncomeSalafAddOrUpdateDtoV2TestBuilder()
            .WithSymbolIsin(isin)
            .WithInterestRate(10)
            .WithRedeemedRate(100)
            .WithDuration(2)
            .WithNominalValue(100)
            .WithInterestPaymentInterval(20)
            .WithTradeStartDate(dt.GetISOStringDateTime())
            .WithMaturityDate(dt.GetISOStringDateTime())
            .WithPublicationDate(dt.GetISOStringDate2())
            .WithSubscriptionEndDate(dt.GetISOStringDateTime())
            .WithSubscriptionStartDate(dt.GetISOStringDateTime())
            .WithSecondaryTradeEndDate(dt.GetISOStringDateTime())
            .WithSecondaryTradeStartDate(dt.GetISOStringDateTime())
            .WithEachContractAmount(10)
            .WithBuyConsequentialPrice(10)
            .WithSellConsequentialPrice(10)
            .WithEachTonIPOPrice(10)
            .WithEachTonNominalPrice(10)
            .WithPublisherId(32)
            .WithPublisherName("MARKET MAKER COMPANY")
            .Build();


        //-ACT
        var response = await _client.PostAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var fixedIncomeInDb = await _factory.Repositories.FixedIncomeGetByIsinAsync(dto.SymbolIsin);
        fixedIncomeInDb.Should().NotBeNull();
        fixedIncomeInDb.Duration.Should().Be(dto.Duration);
        fixedIncomeInDb.InterestRate.Should().Be(dto.InterestRate);
        fixedIncomeInDb.RedeemedRate.Should().Be(dto.RedeemedRate);
        fixedIncomeInDb.NominalValue.Should().Be(dto.NominalValue);
        fixedIncomeInDb.InterestPaymentInterval.Should().Be((byte)dto.InterestPaymentInterval);
        fixedIncomeInDb.MaturityDate.GetISOStringDateTime().Should().Be(dt.GetISOStringDate2());
        fixedIncomeInDb.SubscriptionEndDate.GetISOStringDateTime().Should().Be(dt.GetISOStringDate2());
        fixedIncomeInDb.SubscriptionStartDate.GetISOStringDateTime().Should().Be(dt.GetISOStringDate2());
        fixedIncomeInDb.TradeStartDate.GetISOStringDateTime().Should().Be(dt.GetISOStringDate2());
        fixedIncomeInDb.PublicationDate.Value.GetISOStringDateTime().Should().Be(dt.GetISOStringDate2());
        fixedIncomeInDb.Description.Should().Be(dto.Description ?? "");
        fixedIncomeInDb.FixedIncomeTypeIdFk.Should().Be(5);

        //========================= Constituents Assertion
        var fixedIncomeCons = await _factory.Repositories.FixedIncomeConstituentsGetsAsync(fixedIncomeInDb.FixedIncomeIdPk);
        if (dto.PublisherId.HasValue)
        {
            var publisherInDb = fixedIncomeCons.SingleOrDefault(x => x.FixedIncomeConstituentTypeIdFk == 2
            && x.PartyIdFk == dto.PublisherId.Value);
            publisherInDb.Should().NotBeNull();
        }

        //======================== Salaf FixedIncome Data
        var salafFixedIncome = await _factory.Repositories.SalafFixedIncomeGetByIsinAsync(dto.SymbolIsin);
        salafFixedIncome.Should().NotBeNull();
        salafFixedIncome.BuyConsequentialPrice.Should().Be(dto.BuyConsequentialPrice);
        salafFixedIncome.EachContractAmount.Should().Be(dto.EachContractAmount);
        salafFixedIncome.EachTonIpoprice.Should().Be(dto.EachTonIPOPrice);
        salafFixedIncome.EachTonNominalPrice.Should().Be(dto.EachTonNominalPrice);
        salafFixedIncome.SellConsequentialPrice.Should().Be(dto.SellConsequentialPrice);

        if (!string.IsNullOrWhiteSpace(dto.SecondaryTradeEndDate))
            salafFixedIncome.SecondrayTradeEndDate.Value.GetISOStringDateTime().Should().Be(dt.GetISOStringDate2());

        if (!string.IsNullOrWhiteSpace(dto.SecondaryTradeStartDate))
            salafFixedIncome.SecondaryTradeStartDate.Value.GetISOStringDateTime().Should().Be(dt.GetISOStringDate2());

    }

    [Fact]
    public async Task Should_not_be_able_insert_salaf_that_exist_already()
    {
        //-ARRANGE
        await RequierDataTest2Async();
        var dt = new DateTime(2024, 07, 29, 13, 43, 0);
        var isin = "IRB5AE800065";
        var dto = new FixedIncomeSalafAddOrUpdateDtoV2TestBuilder()
            .WithSymbolIsin(isin)
            .WithInterestRate(10)
            .WithRedeemedRate(100)
            .WithDuration(2)
            .WithNominalValue(100)
            .WithInterestPaymentInterval(20)
            .WithTradeStartDate(dt.GetISOStringDateTime())
            .WithMaturityDate(dt.GetISOStringDateTime())
            .WithPublicationDate(dt.GetISOStringDate2())
            .WithSubscriptionEndDate(dt.GetISOStringDateTime())
            .WithSubscriptionStartDate(dt.GetISOStringDateTime())
            .WithSecondaryTradeEndDate(dt.GetISOStringDateTime())
            .WithSecondaryTradeStartDate(dt.GetISOStringDateTime())
            .WithEachContractAmount(10)
            .WithBuyConsequentialPrice(10)
            .WithSellConsequentialPrice(10)
            .WithEachTonIPOPrice(10)
            .WithEachTonNominalPrice(10)
            .WithPublisherId(32)
            .WithPublisherName("MARKET MAKER COMPANY")
            .Build();


        //-ACT
        var response = await _client.PostAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var apiResult = await response.Content.ReadModelFromJsonAsync<ApiResultTest>();
        apiResult.Messages.Should().Contain(["FixedIncome already exist"]);

    }

    [Fact]
    public async Task Should_be_create_profit_daily_salaf()
    {
        //-ARRANGE
        await RequierDataTest1Async();
        var dt = new DateTime(2025, 03, 03, 10, 43, 0);
        var isin = "IRB5AE800061";
        var dto = new FixedIncomeSalafAddOrUpdateDtoV2TestBuilder()
            .WithSymbolIsin(isin)
            .WithInterestRate(10)
            .WithRedeemedRate(100)
            .WithDuration(2)
            .WithNominalValue(100)
            .WithInterestPaymentInterval(20)
            .WithTradeStartDate(dt.GetISOStringDateTime())
            .WithMaturityDate(dt.AddDays(5).GetISOStringDateTime())
            .WithPublicationDate(dt.GetISOStringDate2())
            .WithSubscriptionEndDate(dt.GetISOStringDateTime())
            .WithSubscriptionStartDate(dt.GetISOStringDateTime())
            .WithSecondaryTradeEndDate(dt.GetISOStringDateTime())
            .WithSecondaryTradeStartDate(dt.GetISOStringDateTime())
            .WithEachContractAmount(10)
            .WithBuyConsequentialPrice(10)
            .WithSellConsequentialPrice(10)
            .WithEachTonIPOPrice(10)
            .WithEachTonNominalPrice(10)
            .WithPublisherId(32)
            .WithPublisherName("MARKET MAKER COMPANY")
            .Build();


        //-ACT
        var response = await _client.PostAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var fixedIncomeInDb = await _factory.Repositories.FixedIncomeGetByIsinAsync(dto.SymbolIsin);
        var profitDaily = await _factory.Repositories.SalafProfitGetsByFixedIncomeIdAsync(fixedIncomeInDb.FixedIncomeIdPk);

        profitDaily.Should().NotBeNull();
    }

    [Fact]
    public async Task Should_be_able_create_a_salaf_with_some_isin_that_have_this_instrumentId()
    {
        //-ARRANGE
        await RequierDataTest3Async();
        var dt = new DateTime(2024, 07, 29, 13, 43, 0);
        var isin = "IRB5AE800061";
        var dto = new FixedIncomeSalafAddOrUpdateDtoV2TestBuilder()
            .WithSymbolIsin(isin)
            .WithInterestRate(10)
            .WithRedeemedRate(100)
            .WithDuration(2)
            .WithNominalValue(100)
            .WithInterestPaymentInterval(20)
            .WithTradeStartDate(dt.GetISOStringDateTime())
            .WithMaturityDate(dt.GetISOStringDateTime())
            .WithPublicationDate(dt.GetISOStringDate2())
            .WithSubscriptionEndDate(dt.GetISOStringDateTime())
            .WithSubscriptionStartDate(dt.GetISOStringDateTime())
            .WithSecondaryTradeEndDate(dt.GetISOStringDateTime())
            .WithSecondaryTradeStartDate(dt.GetISOStringDateTime())
            .WithEachContractAmount(10)
            .WithBuyConsequentialPrice(10)
            .WithSellConsequentialPrice(10)
            .WithEachTonIPOPrice(10)
            .WithEachTonNominalPrice(10)
            .WithPublisherId(32)
            .WithPublisherName("MARKET MAKER COMPANY")
            .WithMarketMakerId(31)
            .Build();


        //-ACT
        var response = await _client.PostAsync(_apiAddress, dto.ToContentHttpString());
        await response.WriteOnConsoleAsync(_outPutHelper);

        //-ASSERT
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var fixedIncomeInDb = await _factory.Repositories.FixedIncomeGetByIsinAsync(dto.SymbolIsin);
        fixedIncomeInDb.Should().NotBeNull();
        fixedIncomeInDb.Duration.Should().Be(dto.Duration);
        fixedIncomeInDb.InterestRate.Should().Be(dto.InterestRate);
        fixedIncomeInDb.RedeemedRate.Should().Be(dto.RedeemedRate);
        fixedIncomeInDb.NominalValue.Should().Be(dto.NominalValue);
        fixedIncomeInDb.InterestPaymentInterval.Should().Be((byte)dto.InterestPaymentInterval);
        fixedIncomeInDb.MaturityDate.GetISOStringDateTime().Should().Be(dt.GetISOStringDate2());
        fixedIncomeInDb.SubscriptionEndDate.GetISOStringDateTime().Should().Be(dt.GetISOStringDate2());
        fixedIncomeInDb.SubscriptionStartDate.GetISOStringDateTime().Should().Be(dt.GetISOStringDate2());
        fixedIncomeInDb.TradeStartDate.GetISOStringDateTime().Should().Be(dt.GetISOStringDate2());
        fixedIncomeInDb.PublicationDate.Value.GetISOStringDateTime().Should().Be(dt.GetISOStringDate2());
        fixedIncomeInDb.Description.Should().Be(dto.Description ?? "");
        fixedIncomeInDb.FixedIncomeTypeIdFk.Should().Be(5);

        //========================= Constituents Assertion
        var fixedIncomeCons = await _factory.Repositories.FixedIncomeConstituentsGetsAsync(fixedIncomeInDb.FixedIncomeIdPk);
        if (dto.PublisherId.HasValue)
        {
            var publisherInDb = fixedIncomeCons.SingleOrDefault(x => x.FixedIncomeConstituentTypeIdFk == 2
            && x.PartyIdFk == dto.PublisherId.Value);
            publisherInDb.Should().NotBeNull();
        }

        //======================== Salaf FixedIncome Data
        var salafFixedIncome = await _factory.Repositories.SalafFixedIncomeGetByIsinAsync(dto.SymbolIsin);
        salafFixedIncome.Should().NotBeNull();
        salafFixedIncome.BuyConsequentialPrice.Should().Be(dto.BuyConsequentialPrice);
        salafFixedIncome.EachContractAmount.Should().Be(dto.EachContractAmount);
        salafFixedIncome.EachTonIpoprice.Should().Be(dto.EachTonIPOPrice);
        salafFixedIncome.EachTonNominalPrice.Should().Be(dto.EachTonNominalPrice);
        salafFixedIncome.SellConsequentialPrice.Should().Be(dto.SellConsequentialPrice);

        if (!string.IsNullOrWhiteSpace(dto.SecondaryTradeEndDate))
            salafFixedIncome.SecondrayTradeEndDate.Value.GetISOStringDateTime().Should().Be(dt.GetISOStringDate2());

        if (!string.IsNullOrWhiteSpace(dto.SecondaryTradeStartDate))
            salafFixedIncome.SecondaryTradeStartDate.Value.GetISOStringDateTime().Should().Be(dt.GetISOStringDate2());

    }

}
