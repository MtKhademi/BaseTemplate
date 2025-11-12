namespace MDF.Test.Integration.Modules.SymbolModuleTest.MarketChangeTest.FeaturesTest.MarketChangeAddTest;


internal class MarketChangeAddOrUpdateDtoNotValidData : TheoryData<MarketChangeAddOrUpdateDtoV4Test, List<string>>
{
    public MarketChangeAddOrUpdateDtoNotValidData()
    {
        var dto = new MarketChangeAddOrUpdateDtoV4Test();
        Add(dto, [
        "From Symbol Close Date can not be convert to date =\u003E sample YYYY-MM-DD :: ",
        "To Symbol Open Date can not be convert to date =\u003E sample YYYY-MM-DD :: ",
        "ToSymbolIsin can not be nul or empty",
        "FromSymbolIsin can not be nul or empty"
      ]);
    }
}
