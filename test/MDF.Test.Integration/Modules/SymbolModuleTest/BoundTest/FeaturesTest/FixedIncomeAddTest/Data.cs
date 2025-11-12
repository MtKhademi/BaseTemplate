namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.FeaturesTest.FixedIncomeCreateTest;

internal class FixedIncomeCreateNotValidData : TheoryData<FixedIncomeCreateDtoV4Test, List<string>>
{
    public FixedIncomeCreateNotValidData()
    {
        var dto = new FixedIncomeCreateDtoV4Test();
        Add(dto, [
        "MaturityDate can not be null or empty.",
        "SubscriptionEndDate can not be null or empty.",
        "SubscriptionStartDate can not be null or empty.",
        "TradeStartDate can not be null or empty.",
        "PublicationDate can not be null or empty.",
        "SymbolIsin can not be null or empty"]);

    }
}

internal class FixedIncomeSalafAddOrUpdateV2DtoValidData : TheoryData<FixedIncomeSalafAddOrUpdateDtoV2Test>
{
    public FixedIncomeSalafAddOrUpdateV2DtoValidData()
    {
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

        Add(dto);

        dt = new DateTime(2024, 07, 29, 13, 43, 0);
        isin = "IRB5AE800065";
        dto = new FixedIncomeSalafAddOrUpdateDtoV2TestBuilder()
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
           .WithEachContractAmount(20)
           .WithBuyConsequentialPrice(20)
           .WithSellConsequentialPrice(10)
           .WithEachTonIPOPrice(20)
           .WithEachTonNominalPrice(50)
           .WithPublisherId(32)
           .WithPublisherName("MARKET MAKER COMPANY")
           .Build();

        Add(dto);
    }
}
