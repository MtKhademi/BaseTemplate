namespace MDF.Test.Integration.SUTS.APIS.BoundTest.FeaturesTest.FixedIncomeGetUITableTest;

internal class FixedIncomeTableFilterDtoNotValidData : TheoryData<FixedIncomeTableFilterDtoV4Test, List<string>>
{
    public FixedIncomeTableFilterDtoNotValidData()
    {
        var dto = new FixedIncomeTableFilterDtoV4TestBuilder()
            .WithMaturityDate("2025-")
            .Build();
        Add(dto, [
            "MaturityDate '2025-' is not in correct format"]);


    }
}


internal class FixedIncomeTableValidData : TheoryData<FixedIncomeTableFilterDtoV4Test, FixedIncomeGetUIDtoV4Test>
{
    public FixedIncomeTableValidData()
    {
        var dt = new DateTime(2024, 07, 23, 08, 50, 0);
        var dto = new FixedIncomeTableFilterDtoV4TestBuilder()
           .WithIsin("IRB5AE800041")
           .Build();
        Add(dto, new FixedIncomeGetUIDtoV4Test
        {
            SymbolIsin = "IRB5AE800041",
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
            PublicationDate = "2024-07-24",
            TradeStartDate = dt.AddDays(50).GetISOStringDate(),
            SubscriptionEndDate = dt.AddDays(10).GetISOStringDate(),
            SubscriptionStartDate = dt.AddDays(2).GetISOStringDate(),
        });

    }
}

internal class FixedIncomeTableFilterValidData : TheoryData<FixedIncomeTableFilterDtoV4Test>
{
    public FixedIncomeTableFilterValidData()
    {
        var dt = new DateTime(2024, 07, 08, 12, 03, 0);
        var dto = new FixedIncomeTableFilterDtoV4TestBuilder()
            .WithIsin("IRB5AE800004")
            .Build();
        Add(dto);

        dto = new FixedIncomeTableFilterDtoV4TestBuilder()
            .WithTypeOfSymbols([TypeOfSymbolTest.Bond_Ejare])
           .Build();
        Add(dto);
    }
}
