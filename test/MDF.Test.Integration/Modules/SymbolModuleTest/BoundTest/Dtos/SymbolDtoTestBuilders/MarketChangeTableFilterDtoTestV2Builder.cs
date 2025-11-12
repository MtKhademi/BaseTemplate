using MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos.MarketChangeDtosTestV2;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.SymbolDtoTestBuilders;

internal class MarketChangeTableFilterDtoTestV2Builder
{
    private readonly MarketChangeTableFilterDtoTestV2 _dto = new MarketChangeTableFilterDtoTestV2();
    public MarketChangeTableFilterDtoTestV2Builder()
    {
    }

    public MarketChangeTableFilterDtoTestV2Builder WithOldIsin(string value)
    {
        _dto.SymbolOldIsin = value;
        return this;
    }
    public MarketChangeTableFilterDtoTestV2Builder WithNewIsin(string value)
    {
        _dto.SymbolNewIsin = value;
        return this;
    }

    public MarketChangeTableFilterDtoTestV2 Build() => _dto;
}
