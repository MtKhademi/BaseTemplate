namespace MDF.Test.Integration.Modules.SymbolModuleTest.MarketChangeTest.Requests;

public class MarketChangeGetPaginatedListRequestTest
{
    public string? OldIsin { get; set; }
    public string? OldCloseDate { get; set; }

    public string? NewIsin { get; set; }
    public string? NewOpenDate { get; set; }

    public int? CurrentPage { get; set; }
    public int? SizeOfPage { get; set; }
}
