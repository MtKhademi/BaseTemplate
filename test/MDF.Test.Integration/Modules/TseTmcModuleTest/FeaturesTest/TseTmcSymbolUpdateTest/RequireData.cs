namespace MDF.Test.Integration.Modules.TseTmcModuleTest.FeaturesTest.TseTmcSymbolUpdateTest;

public partial class TseTmcSymbolUpdateTest
{
    private async Task AddRequierData1Async()
    {
        //====================== ADD SYMBOL ==================
        var dt = new DateTime(2024, 06, 26, 09, 12, 00);
        await _factory.Repositories.SymbolAddAsync(isin: "KORI555567", dtEvent: dt, dtEntry: dt, isDisable: true, dtDisable: dt.AddDays(2));
        await _factory.Repositories.SymbolAddAsync(isin: "KORI555667", dtEvent: dt, dtEntry: dt);
    }
    private async Task AddRequierData2Async()
    {
        //====================== ADD SYMBOL ==================
        var dt = new DateTime(2024, 06, 26, 09, 12, 00);
        await _factory.Repositories.SymbolAddAsync(isin: "KORI556068K", dtEvent: dt, dtEntry: dt);
    }
    private async Task AddRequierData3Async()
    {
        //====================== ADD SYMBOL ==================
        var dt = new DateTime(2024, 06, 26, 09, 12, 00);
        await _factory.Repositories.SymbolAddAsync(isin: "KORI555567", dtEvent: dt, dtEntry: dt, isDisable: true, dtDisable: dt.AddDays(2));
        await _factory.Repositories.SymbolAddAsync(isin: "KORI555667", dtEvent: dt, dtEntry: dt);
        await _factory.Repositories.SymbolAddAsync(isin: "KORI555767", dtEvent: dt, dtEntry: dt);
        await _factory.Repositories.SymbolAddAsync(isin: "KORI555867", dtEvent: dt, dtEntry: dt);
        await _factory.Repositories.SymbolAddAsync(isin: "KORI555967", dtEvent: dt, dtEntry: dt);
        await _factory.Repositories.SymbolAddAsync(isin: "KORI556067", dtEvent: dt, dtEntry: dt);
        await _factory.Repositories.SymbolAddAsync(isin: "KORI556068K", dtEvent: dt, dtEntry: dt);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE8705C1", dtEvent: dt, dtEntry: dt, firmId: 5);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE2504C1", dtEvent: dt, dtEntry: dt, firmId: 6);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB3TA010571", dtEvent: dt, dtEntry: dt, firmId: 6);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB3MA060571", dtEvent: dt, dtEntry: dt, firmId: 6);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB7AE040231", dtEvent: dt, dtEntry: dt, firmId: 6);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB3W0250261", dtEvent: dt, dtEntry: dt, firmId: 6);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB3AR040761", dtEvent: dt, dtEntry: dt, firmId: 6);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB3MD010661", dtEvent: dt, dtEntry: dt, firmId: 6);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB4O03706C1", dtEvent: dt, dtEntry: dt, firmId: 6);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB3TR060481", dtEvent: dt, dtEntry: dt, firmId: 6);
        await _factory.Repositories.SymbolAddAsync(isin: "IRBEMT160411", dtEvent: dt, dtEntry: dt, firmId: 6);
        await _factory.Repositories.SymbolAddAsync(isin: "IRBEC5010311", dtEvent: dt, dtEntry: dt, firmId: 6);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB6AE4805C1", dtEvent: dt, dtEntry: dt, firmId: 6);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB6AE5205C1", dtEvent: dt, dtEntry: dt, firmId: 6);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB3MI0103C1", dtEvent: dt, dtEntry: dt, firmId: 6);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800042", dtEvent: dt, dtEntry: dt, firmId: 6);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB6AE9806A1", dtEvent: dt, dtEntry: dt, firmId: 6);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800035", dtEvent: dt, dtEntry: dt, firmId: 6);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800036", dtEvent: dt, dtEntry: dt, firmId: 6);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800041", dtEvent: dt, dtEntry: dt, firmId: 7);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800046", firmId: 7);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB6AF350761", firmId: 7);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB6AF330751", firmId: 7);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB6AF290541", firmId: 7);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB6AF340761", firmId: 7);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB6AF360661", firmId: 7);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB6AF370761", firmId: 7);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800056", firmId: 7);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800067", firmId: 7);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800070", firmId: 7);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB3PA01037A1", firmId: 7);

        await _factory.Repositories.SymbolAddAsync(isin: "IRB3PA010371", firmId: 7);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB6SEFA0371", firmId: 7);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB6NF0303C1", firmId: 7);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB6GO030391", firmId: 7);
        await _factory.Repositories.SymbolAddAsync(isin: "IRS4KTEK0011", firmId: 7);



        var securityExchangeMarket = await _factory.Repositories.MarketExchangeGetAsync("NO", 1);
        var securityExchangeBoard = await _factory.Repositories.BoardExchangeGetAsync(1, 1);

        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800071", firmId: 7,
            securityExchangeMarket: securityExchangeMarket.ExchangeMarketIdPk,
            securityExchangeBoard: securityExchangeBoard.ExchangeBoardIdPk);

    }
    private async Task AddRequierData4Async()
    {
        //====================== ADD SYMBOL ==================
        var dt = new DateTime(2024, 11, 30, 12, 30, 00);
        await _factory.Repositories.SymbolAddAsync(isin: "KORI5558585", dtEvent: dt, dtEntry: dt, isDisable: true, dtDisable: dt);
    }

    private async Task AddRequierData5Async()
    {
        //====================== ADD SYMBOL ==================
        var dt = new DateTime(2025, 01, 06, 11, 12, 00);
        await _factory.Repositories.SymbolAddAsync(isin: "KORI5558585", dtEvent: dt, dtEntry: dt, symbolName: "");
        await _factory.Repositories.SymbolAddAsync(isin: "KORI5558586", dtEvent: dt, dtEntry: dt, symbolName: "");

        //‌حالتی که نوع نماد دستی قرار داده شده است و در TSETMC
        // چیز دیگری ثبت شده است و نباید تغییر کند
        await _factory.Repositories.SymbolAddAsync(isin: "KORI5558587", dtEvent: dt, dtEntry: dt, symbolName: "", typeOfSymbol: TypeOfSymbolTest.Stock);


        await _factory.Repositories.SymbolAddAsync(isin: "IROTABNY0001", symbolName: "ابنيه1");
        await _factory.Repositories.SymbolAddAsync(isin: "IROTTAKN0001", symbolName: "تك نيرو1");

    }

}
