using MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos.MarketChangeDtosTestV2;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.SymbolDtoTestBuilders;

internal class MarketChangeAddOrUpdateDtoTestV2Builder
{
    private readonly MarketChangeAddOrUpdateDtoTestV2 _dto = new MarketChangeAddOrUpdateDtoTestV2();
    public MarketChangeAddOrUpdateDtoTestV2Builder()
    {
    }

    public MarketChangeAddOrUpdateDtoTestV2Builder WithOldIsin(string value)
    {
        _dto.FromSymbolIsin = value;
        return this;
    }
    public MarketChangeAddOrUpdateDtoTestV2Builder WithNewIsin(string value)
    {
        _dto.ToSymbolIsin = value;
        return this;
    }

    public MarketChangeAddOrUpdateDtoTestV2Builder WithSymbolOldCloseDate(string value)
    {
        _dto.FromSymbolIdCloseDate = value;
        return this;
    }
    public MarketChangeAddOrUpdateDtoTestV2Builder WithSymbolNewOpenDate(string value)
    {
        _dto.ToSymbolIdOpenDate = value;
        return this;
    }


    public MarketChangeAddOrUpdateDtoTestV2 Build() => _dto;
}
