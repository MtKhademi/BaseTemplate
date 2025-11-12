namespace Test.Integration.ModulesTest.SymbolModuleTest.BoundTest.FeaturesTest.SalafProfitDailyGetExcellFileApiTest;

public partial class FixedIncomeProfitDailyGetExcellFileApiTest
{
    public async Task AddRequierAsync()
    {
        var dt = new DateTime(2025, 03, 09, 14, 41, 0);

        var instrument = await _factory.Repositories.InstrumentAddAsync(title: "XXX");

        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800018",
            instrumentId: instrument.InstrumentIdPk,
            typeOfSymbol: TypeOfSymbolTest.Bond_Salaf);

        var fixedIncome = await _factory.Repositories.FixedIncomeAddAsync(instrumentId: instrument.InstrumentIdPk,
              dtEntry: dt,
              dtPublish: null,
              dtModify: dt, dtMaturity: dt.AddDays(5),
              dtSubscriptionEnd: dt.AddDays(10), dtSubscriptionStart: dt.AddDays(2),
              dtTradeStart: dt.AddDays(50),
              nominalPrice: 1000, interestRate: 10, description: "");

        var salafFixedIncome =
            await _factory.Repositories.SalafFixedIncomeAddAsync(fixedIncome.FixedIncomeIdPk, 1000, 10,
            EachTonNominalPrice: 100,
            EachTonIpoprice:10);

        await _factory.Repositories.SalafProfitAddAsync(fixedIncome.FixedIncomeIdPk, dt.AddDays(-3));
        await _factory.Repositories.SalafProfitAddAsync(fixedIncome.FixedIncomeIdPk, dt.AddDays(-2));
        await _factory.Repositories.SalafProfitAddAsync(fixedIncome.FixedIncomeIdPk, dt.AddDays(-1));
        await _factory.Repositories.SalafProfitAddAsync(fixedIncome.FixedIncomeIdPk, dt.AddDays(0));
        await _factory.Repositories.SalafProfitAddAsync(fixedIncome.FixedIncomeIdPk, dt.AddDays(1));
        await _factory.Repositories.SalafProfitAddAsync(fixedIncome.FixedIncomeIdPk, dt.AddDays(2));

    }
}
