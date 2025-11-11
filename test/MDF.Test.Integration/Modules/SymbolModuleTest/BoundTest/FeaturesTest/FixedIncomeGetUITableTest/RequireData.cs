
namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.FeaturesTest.FixedIncomeGetUITableTest;

public partial class FixIncomeGetTableApiTest
{
    private async Task FixIncomeGetTableApiTestRequierAsync()
    {

        await _factory.Repositories.SymbolAddAsync("IRB5AE800002", typeOfSymbol: TypeOfSymbolTest.Bond);
        await _factory.Repositories.SymbolAddAsync("IRB5AE800003", typeOfSymbol: TypeOfSymbolTest.Stock);

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

        await _factory.Repositories.OrganizationAddAsync(organizationId: 23, title: "Publisher");
        var partyType = await _factory.Repositories.PartyTypeAddAsync("NEW TITLE", "1");
        await _factory.Repositories.PartyAddAsync(23, partyType.PartyTypeIdPk);
        await _factory.Repositories.FixedIncomeConstituentsAddAsync(fixedIncome.FixedIncomeIdPk, 23,
             ETypeOfFixedIncomeConstituentTypeTest.Publisher);

        await _factory.Repositories.OrganizationAddAsync(organizationId: 24, title: "MarketMaker");
        await _factory.Repositories.PartyAddAsync(24, partyType.PartyTypeIdPk);
        await _factory.Repositories.FixedIncomeConstituentsAddAsync(fixedIncome.FixedIncomeIdPk, 24,
            ETypeOfFixedIncomeConstituentTypeTest.MarketMaker);



        //================= with fixed income data
        instrument = await _factory.Repositories.InstrumentAddAsync(title: "Instrument Title2");
        await _factory.Repositories.SymbolAddAsync("IRB5AE800004", instrumentId: instrument.InstrumentIdPk, typeOfSymbol: TypeOfSymbolTest.Bond_Ejare);

        dt = new DateTime(2024, 07, 23, 08, 50, 0);
        fixedIncome = await _factory.Repositories.FixedIncomeAddAsync(instrument.InstrumentIdPk,
            dtEntry: dt,
            dtModify: dt,
            dtPublish: dt.AddDays(1),
            dtMaturity: dt.AddDays(5),
            dtSubscriptionEnd: dt.AddDays(10),
            dtSubscriptionStart: dt.AddDays(2),
            dtTradeStart: dt.AddDays(50));


        //================= this symbol is not in market NO and It has to filter
        await _factory.Repositories.SymbolAddAsync("IRB5AE800034", instrumentId: instrument.InstrumentIdPk, typeOfSymbol: TypeOfSymbolTest.BondCallOption);

    }
}