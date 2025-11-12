namespace Test.Integration.ModulesTest.SymbolModuleTest.BoundTest.FeaturesTest.SalafGetByIsinApiTest;
public partial class SalafGetByIsinTest
{

    private async Task RequierDataTestAsync()
    {
        var dt = new DateTime(2024, 09, 28, 15, 15, 0);

        var instrument = await _factory.Repositories.InstrumentAddAsync(title: "XXX-NEW");
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800061",
            instrumentId: instrument.InstrumentIdPk,
            typeOfSymbol: TypeOfSymbolTest.Bond_Salaf);

        var fixedincome = await _factory.Repositories
            .FixedIncomeAddAsync(
            instrument.InstrumentIdPk,
            dtSubscriptionEnd: dt.AddDays(10),
            dtSubscriptionStart: dt.AddDays(-5));

        await _factory.Repositories.SalafFixedIncomeAddAsync(
            FixedIncomeIdPk: fixedincome.FixedIncomeIdPk,
            EachContractAmount: 100,
            Producer: 10,
            BuyConsequentialPrice: 9,
            SellConsequentialPrice: 8,
            EachTonNominalPrice: 1000,
            EachTonIpoprice: 100,
            SecondaryTradeStartDate: dt.AddDays(-1),
            SecondrayTradeEndDate: dt.AddDays(5));
    }
}
