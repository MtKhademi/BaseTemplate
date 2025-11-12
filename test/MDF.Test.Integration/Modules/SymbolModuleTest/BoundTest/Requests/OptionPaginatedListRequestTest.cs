namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Requests;

public class OptionPaginatedListRequestTest
{
    public string? SymbolBaseIsin { get; set; }
    public string? Isins { get; set; }
    public string? ApplyDate { get; set; }
    public string? ApplyDateFrom { get; set; }
    public string? ApplyDateTo { get; set; }
    public string? StartDate { get; set; }
    public bool? IsDeleted { get; set; }

    public int? CurrentPage { get; set; }
    public int? SizeOfPage { get; set; }
}
