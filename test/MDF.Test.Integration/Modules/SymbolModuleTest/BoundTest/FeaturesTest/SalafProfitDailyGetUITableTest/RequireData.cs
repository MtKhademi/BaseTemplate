namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.FeaturesTest.SalafProfitDailyGetUITableTest;

public partial class SalafProfitDailyGetUITable
{
    private async Task RequierTest1Async()
    {

        var dt = new DateTime(2024, 07, 23, 08, 50, 0);

        var instrument = await _factory.Repositories.InstrumentAddAsync(title: "Instrument Title");
        var symbol = await _factory.Repositories.SymbolAddAsync("IRB5AE800045", instrumentId: instrument.InstrumentIdPk, typeOfSymbol: TypeOfSymbolTest.Bond_Ejare);
        var fixedIncome = await _factory.Repositories.FixedIncomeAddAsync(
               instrumentId: instrument.InstrumentIdPk,
               dtPublish: dt.AddDays(1),
               dtEntry: dt,
               dtModify: dt,
               dtMaturity: dt.AddDays(5),
               dtSubscriptionEnd: dt.AddDays(10),
               dtSubscriptionStart: dt.AddDays(2),
               dtTradeStart: dt.AddDays(50));


        await _factory.Repositories.SalafFixedIncomeAddAsync(fixedIncome.FixedIncomeIdPk);

        await _factory.Repositories.SalafProfitAddAsync(
            fixedIncomeId: fixedIncome.FixedIncomeIdPk,
            dtOfEvent: dt);


        await _factory.Repositories.SalafProfitAddAsync(
            fixedIncomeId: fixedIncome.FixedIncomeIdPk,
            dtOfEvent: dt.AddDays(1));


        await _factory.Repositories.SalafProfitAddAsync(
            fixedIncomeId: fixedIncome.FixedIncomeIdPk,
            dtOfEvent: dt.AddDays(2));
    }
}