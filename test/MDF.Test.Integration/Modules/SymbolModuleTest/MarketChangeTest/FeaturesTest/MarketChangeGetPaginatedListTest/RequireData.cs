namespace MDF.Test.Integration.Modules.SymbolModuleTest.MarketChangeTest.FeaturesTest.MarketChangeGetPaginatedListTest;

public partial class MarketChangeGetPaginatedListTest
{

    private async Task AddRequierAsync()
    {
        await _factory.Repositories.ChangeMarketAddAsync("IRB1238585", "IRB1238586", new DateTime(2025, 01, 10, 11, 49, 00));
        await _factory.Repositories.ChangeMarketAddAsync("IRB1239090", "IRB1239091", new DateTime(2025, 01, 12, 11, 49, 00));
        await _factory.Repositories.ChangeMarketAddAsync("IRB1239092", "IRB1239093", new DateTime(2025, 01, 13, 11, 49, 00));
        await _factory.Repositories.ChangeMarketAddAsync("IRB1239094", "IRB1239095", new DateTime(2025, 01, 14, 11, 49, 00));
        await _factory.Repositories.ChangeMarketAddAsync("IRB1239096", "IRB1239097", new DateTime(2025, 01, 15, 11, 49, 00));
    }
}