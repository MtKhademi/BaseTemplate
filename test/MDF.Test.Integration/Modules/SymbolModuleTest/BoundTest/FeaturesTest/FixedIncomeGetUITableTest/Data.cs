using MDF.Modules.SymbolModule.Depricate.Abstractions;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.FeaturesTest.FixedIncomeGetUITableTest;

internal class FixedIncomeTableFilterDtoNotValidData : TheoryData<FixedIncomeTableFilterDtoV2Test, List<string>>
{
    public FixedIncomeTableFilterDtoNotValidData()
    {
        var dto = new FixedIncomeTableFilterDtoV2TestBuilder()
            .WithIsin("XCBV")
            .Build();
        Add(dto, [
            "Isin has to has a length bigger than 5 char => XCBV"]);

        dto = new FixedIncomeTableFilterDtoV2TestBuilder()
            .WithDueDate("2025-")
            .Build();
        Add(dto, [
            "Maturity Date can not be convert to date => sample YYYY-MM-DD :: 2025-"]);


    }
}


internal class FixedIncomeTableValidData : TheoryData<FixedIncomeTableFilterDtoV2Test, FixedIncomeGetUIDtoV2Test>
{
    public FixedIncomeTableValidData()
    {
        var dt = new DateTime(2024, 07, 23, 08, 50, 0);
        var dto = new FixedIncomeTableFilterDtoV2TestBuilder()
           .WithIsin("IRB5AE800045")
           .Build();
        Add(dto, new FixedIncomeGetUIDtoV2Test
        {
            SymbolIsin = "IRB5AE800045",
            SymbolName = "SYMBOL-NAME",
            TypeOfSymbol = TypeOfSymbolTest.Bond_Ejare,
            InterestRate = 10,
            InterestPaymentInterval = 0,
            NominalValue = 1000,
            Description = "",
            RedeemedRate = 10,
            Duration = 2.0,
            PublisherName = "Publisher",
            GurantorName = "GurantorName",
            MarketMakerName = "MarketMaker",
            TypeOfSymbolPersianName = "اوراق اجاره",
            PublicationDate = "2024-07-24T00:00:00",
            TradeStartDate = dt.AddDays(50).GetISOStringDateTime(),
            SubscriptionEndDate = dt.AddDays(10).GetISOStringDateTime(),
            SubscriptionStartDate = dt.AddDays(2).GetISOStringDateTime(),
        });

    }
}

internal class FixedIncomeTableFilterValidData : TheoryData<FixedIncomeTableFilterDtoV2Test>
{
    public FixedIncomeTableFilterValidData()
    {
        var dt = new DateTime(2024, 07, 08, 12, 03, 0);
        var dto = new FixedIncomeTableFilterDtoV2TestBuilder()
            .WithIsin("IRB5AE800004")
            .Build();
        Add(dto);

        dto = new FixedIncomeTableFilterDtoV2TestBuilder()
            .WithTypeOfSymbols([ETypeOfSymbol.Bond_Ejare])
           .Build();
        Add(dto);
    }
}
