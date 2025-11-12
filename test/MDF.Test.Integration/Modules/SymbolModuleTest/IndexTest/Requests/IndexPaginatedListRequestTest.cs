namespace MDF.Test.Integration.Modules.SymbolModuleTest.IndexTest.Requests;

internal class IndexPaginatedListRequestTest
{
    public string? Isin { get; set; }
    public string? StartDateTime { get; set; }
    public string? EndDateTime { get; set; }
    public bool? OnlyGetLastChange { get; set; }
}
