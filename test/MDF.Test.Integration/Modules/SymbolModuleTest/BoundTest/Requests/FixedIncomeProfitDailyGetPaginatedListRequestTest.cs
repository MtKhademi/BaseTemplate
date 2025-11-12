namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Requests;

internal class FixedIncomeProfitDailyGetPaginatedListRequestTest
{
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public int? CurrentPage { get; set; }
    public int? SizeOfPage { get; set; }
}
