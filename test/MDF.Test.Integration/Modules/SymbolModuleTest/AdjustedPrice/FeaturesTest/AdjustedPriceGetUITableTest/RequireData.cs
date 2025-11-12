namespace MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.FeaturesTest.AdjustedPriceGetUITableTest;
public partial class AdjustedPriceGetUITableTest
{

    private async Task AddRequierAsync()
    {
        var dt = new DateTime(2025, 07, 13, 14, 30, 0);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85858", dt: dt.AddDays(-4), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85858", dt: dt.AddDays(-3), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85858", dt: dt.AddDays(-2), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85858", dt: dt.AddDays(-1), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85858", dt: dt, closingPrice: 10, lastPrice: 12);
    }
}