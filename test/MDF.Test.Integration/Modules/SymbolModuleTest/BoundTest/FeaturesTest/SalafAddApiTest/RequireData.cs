namespace Test.Integration.ModulesTest.SymbolModuleTest.BoundTest.FeaturesTest.SalafAddApiTest;
public partial class FixedIncomeSalafAddOrUpdateApiTest
{
    private async Task RequierDataTest1Async()
    {
        var dt = new DateTime(2024, 09, 28, 15, 15, 0);

        var instrument = await _factory.Repositories.InstrumentAddAsync(title: "XXX-NEW");
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800061", instrumentId: instrument.InstrumentIdPk, typeOfSymbol: TypeOfSymbolTest.Bond_Ejare);

        instrument = await _factory.Repositories.InstrumentAddAsync(title: "XXX");
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800064", instrumentId: instrument.InstrumentIdPk, typeOfSymbol: TypeOfSymbolTest.Bond_Ejare);


        await _factory.Repositories.OrganizationAddAsync(organizationId: 31, nationalCode: "PUB123", title: "PUBLISHER COMPANY");
        await _factory.Repositories.OrganizationAddAsync(organizationId: 32, nationalCode: "MARKET123", title: "MARKET MAKER COMPANY");
        await _factory.Repositories.OrganizationAddAsync(organizationId: 33, nationalCode: "GURANTOUR123", title: "GURANTOUR COMPANY");
    }

    private async Task RequierDataTest2Async()
    {
        var dt = new DateTime(2024, 09, 28, 15, 15, 0);

        var instrument = await _factory.Repositories.InstrumentAddAsync(title: "XXX-NEW");
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800065", instrumentId: instrument.InstrumentIdPk, typeOfSymbol: TypeOfSymbolTest.Bond_Ejare);

        await _factory.Repositories.FixedIncomeAddAsync(instrument.InstrumentIdPk);

        await _factory.Repositories.OrganizationAddAsync(organizationId: 31, nationalCode: "PUB123", title: "PUBLISHER COMPANY");
        await _factory.Repositories.OrganizationAddAsync(organizationId: 32, nationalCode: "MARKET123", title: "MARKET MAKER COMPANY");
        await _factory.Repositories.OrganizationAddAsync(organizationId: 33, nationalCode: "GURANTOUR123", title: "GURANTOUR COMPANY");
    }

    private async Task RequierDataTest3Async()
    {
        var dt = new DateTime(2024, 09, 28, 15, 15, 0);

        var instrument = await _factory.Repositories.InstrumentAddAsync(title: "XXX-NEW");
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800061", instrumentId: instrument.InstrumentIdPk, typeOfSymbol: TypeOfSymbolTest.Bond_Ejare);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800062", instrumentId: instrument.InstrumentIdPk, typeOfSymbol: TypeOfSymbolTest.Bond_Ejare);
        await _factory.Repositories.SymbolAddAsync(isin: "IRB5AE800063", instrumentId: instrument.InstrumentIdPk, typeOfSymbol: TypeOfSymbolTest.Bond_Ejare);


        await _factory.Repositories.OrganizationAddAsync(organizationId: 31, nationalCode: "PUB123", title: "PUBLISHER COMPANY");
        await _factory.Repositories.OrganizationAddAsync(organizationId: 32, nationalCode: "MARKET123", title: "MARKET MAKER COMPANY");
        await _factory.Repositories.OrganizationAddAsync(organizationId: 33, nationalCode: "GURANTOUR123", title: "GURANTOUR COMPANY");
    }
}
