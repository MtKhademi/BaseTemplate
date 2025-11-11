namespace MDF.Test.Integration.Modules.SymbolModuleTest.MarketChangeTest.FeaturesTest.MarketChangeUpdateTest;

internal class MarketChangeAddOrUpdateDtoNotValidData : TheoryData<MarketChangeUpdateDtoV4Test, List<string>>
{
    public MarketChangeAddOrUpdateDtoNotValidData()
    {
        var dto = new MarketChangeUpdateDtoV4Test();
        Add(dto, [
            "From Symbol Close Date can not be convert to date =\u003E sample YYYY-MM-DD :: ",
            "From Symbol Close Date can not be convert to date =\u003E sample YYYY-MM-DD :: ",
            "ToSymbolIsin can not be nul or empty",
            "FromSymbolIsin can not be nul or empty"
      ]);
    }
}
