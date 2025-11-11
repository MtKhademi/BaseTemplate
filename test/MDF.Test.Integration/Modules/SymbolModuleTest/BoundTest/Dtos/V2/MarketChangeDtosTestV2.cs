using MDF.Common.Infrastructure.TableInfra.TableInfra;
using MDF.Common.Infrastructure;

namespace MDF.Test.Integration.SUTS.APIS.V2.Dtos.SymbolDtos.MarketChangeDtosTestV2;


#region MarketChangeAddOrUpdateDtoTestV2

public class MarketChangeAddOrUpdateDtoTestV2
{

    public string? FromSymbolIsin { get; set; }
    public string? FromSymbolIdCloseDate { get; set; }

    public string? ToSymbolIsin { get; set; }
    public string? ToSymbolIdOpenDate { get; set; }

}

#endregion

#region MarketChangeFilterDtoTestV2

public class MarketChangeFilterDtoTestV2
{
    public string? SymbolOldIsin { get; set; }
    public string? SymbolNewIsin { get; set; }

}
public class MarketChangeTableFilterDtoTestV2 : MarketChangeFilterDtoTestV2, IBasePagination
{
    public int? CurrentPage { get; set; }
    public int? SizeOfPage { get; set; }
}

#endregion

#region MarketChangeGetDtoTestV2

public class MarketChangeGetDtoTestV2
{
    public string? SymbolOldName { get; set; }
    public string? SymbolOldIsin { get; set; }
    public string? SymbolOldCloseDate { get; set; }
    public string? SymbolOldMarketName { get; set; }


    public string? SymbolNewName { get; set; }
    public string? SymbolNewIsin { get; set; }
    public string? SymbolNewOpenDate { get; set; }
    public string? SymbolNewMarketName { get; set; }

    public string? State { get; set; }

}
public class MarketChangeGetUIDtoV2Test : MarketChangeGetDtoTestV2, IUIGridRow
{
    public IList<string> Actions { get; set; } = new List<string>();
}

#endregion
