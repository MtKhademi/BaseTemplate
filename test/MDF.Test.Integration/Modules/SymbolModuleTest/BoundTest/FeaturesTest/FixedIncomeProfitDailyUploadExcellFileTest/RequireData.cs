namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.FeaturesTest.FixedIncomeProfitDailyUploadExcellFileTest;

public partial class FixedIncomeProfitDailyUploadExcellFileTest
{
    private async Task AddRequierAsync()
    {

        var dt = new DateTime(2024, 07, 30, 14, 41, 0);
        var instrument = await _factory.Repositories.InstrumentAddAsync(title: "XXX");

        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800017", instrumentId: instrument.InstrumentIdPk);

        await _factory.Repositories.FixedIncomeAddAsync(instrumentId: instrument.InstrumentIdPk,
            dtPublish: null, dtEntry: dt, dtModify: dt, dtMaturity: dt.AddDays(5),
            dtSubscriptionEnd: dt.AddDays(10),
            dtSubscriptionStart: dt.AddDays(2),
            dtTradeStart: dt.AddDays(50));
    }

}
