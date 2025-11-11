namespace MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.FeaturesTest.AdjustedPriceGetPaginatedList;
public partial class AdjustedPriceGetPaginatedList
{

    private async Task AddRequierAsync()
    {
        var dt = new DateTime(2025, 07, 13, 14, 30, 0);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85858", dt: dt.AddDays(-4), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85858", dt: dt.AddDays(-3), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85858", dt: dt.AddDays(-2), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85858", dt: dt.AddDays(-1), closingPrice: 10, lastPrice: 12);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85858", dt: dt, closingPrice: 10, lastPrice: 12);


        var announ = await _factory.Repositories.AnnouncementAddAsync(codalCode: 8585, dtPublish: dt.AddDays(-2));
        var capital = await _factory.Repositories.CapitalChangeAddAsync(announcementId: announ.AnnouncementIdPk);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85858", dt: dt.AddDays(-2), closingPrice: 10, lastPrice: 12, capitalChangeId: capital.CapitalChangeIdPk);




        announ = await _factory.Repositories.AnnouncementAddAsync(codalCode: 8586, dtPublish: dt.AddDays(-5));
        capital = await _factory.Repositories.CapitalChangeAddAsync(announcementId: announ.AnnouncementIdPk);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85858", dt: dt.AddDays(-5), closingPrice: 10, lastPrice: 12, capitalChangeId: capital.CapitalChangeIdPk);


        announ = await _factory.Repositories.AnnouncementAddAsync(codalCode: 8587, dtPublish: dt);
        capital = await _factory.Repositories.CapitalChangeAddAsync(announcementId: announ.AnnouncementIdPk);
        await _factory.Repositories.AdjustedPriceAddAsync(isin: "IRB85858", dt: dt, closingPrice: 10, lastPrice: 12, capitalChangeId: capital.CapitalChangeIdPk);


    }
}