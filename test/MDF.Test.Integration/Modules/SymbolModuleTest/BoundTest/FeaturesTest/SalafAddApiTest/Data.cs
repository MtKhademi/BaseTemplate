namespace Test.Integration.ModulesTest.SymbolModuleTest.BoundTest.FeaturesTest.SalafAddApiTest;

internal class FixedIncomeSalafAddOrUpdateDtoNotValidData : TheoryData<FixedIncomeSalafAddOrUpdateDtoV2Test, List<string>>
{
    public FixedIncomeSalafAddOrUpdateDtoNotValidData()
    {
        var dto = new FixedIncomeSalafAddOrUpdateDtoV2TestBuilder()
            .Build();
        Add(dto, [
            "Maturity Date can not be convert to date =\u003E sample YYYY-MM-DD :: ",
            "Subscription End Date can not be convert to date =\u003E sample YYYY-MM-DD :: ",
            "Subscription Start Date can not be convert to date =\u003E sample YYYY-MM-DD :: ",
            "Trade Start Date can not be convert to date =\u003E sample YYYY-MM-DD :: ",
            "Publication Date can not be convert to date =\u003E sample YYYY-MM-DD :: ",
            "SymbolIsin can not be nul or empty",
            "EachContractAmount can not be nul or empty",
            "BuyConsequentialPrice can not be nul or empty",
            "SellConsequentialPrice can not be nul or empty",
            "EachTonNominalPrice can not be nul or empty",
            "EachTonIPOPrice can not be nul or empty",
            "Secondary Trade Start Date can not be convert to date =\u003E sample YYYY-MM-DD :: ",
            "Secondary Trade End Date can not be convert to date =\u003E sample YYYY-MM-DD :: "
  ]);

        var dt = new DateTime(2024, 09, 28, 14, 20, 0);
        dto = new FixedIncomeSalafAddOrUpdateDtoV2TestBuilder()
            .WithMaturityDate(dt.GetISOStringDateTime())
            .WithPublicationDate(dt.GetISOStringDateTime())
            .WithSubscriptionEndDate(dt.GetISOStringDateTime())
            .WithSubscriptionStartDate(dt.GetISOStringDateTime())
            .WithTradeStartDate(dt.GetISOStringDateTime())
            .WithSymbolIsin("IRB5AE800062")
           .Build();
        Add(dto, [
            "EachContractAmount can not be nul or empty",
            "BuyConsequentialPrice can not be nul or empty",
            "SellConsequentialPrice can not be nul or empty",
            "EachTonNominalPrice can not be nul or empty",
            "EachTonIPOPrice can not be nul or empty",
            "Secondary Trade Start Date can not be convert to date =\u003E sample YYYY-MM-DD :: ",
            "Secondary Trade End Date can not be convert to date =\u003E sample YYYY-MM-DD :: "
          ]);

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
