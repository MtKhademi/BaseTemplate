namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.FeaturesTest.SalafProfitDailyGetUITableTest;

internal class Data
{
    internal class FixedIncomeTableFilterDtoNotValidData : TheoryData<FixedIncomeTableFilterDtoV4Test, List<string>>
    {
        public FixedIncomeTableFilterDtoNotValidData()
        {
            var dto = new FixedIncomeTableFilterDtoV4TestBuilder()
                .WithMaturityDate("2025-")
                .Build();
            Add(dto, [
                "Maturity Date can not be convert to date => sample YYYY-MM-DD :: 2025-"]);


        }
    }


    internal class FixedIncomeTableValidData : TheoryData<FixedIncomeTableFilterDtoV4Test, FixedIncomeGetUIDtoV4Test>
    {
        public FixedIncomeTableValidData()
        {
            var dt = new DateTime(2024, 07, 23, 08, 50, 0);
            var dto = new FixedIncomeTableFilterDtoV4TestBuilder()
               .WithIsin("IRB5AE800045")
               .Build();
            Add(dto, new FixedIncomeGetUIDtoV4Test
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
}
