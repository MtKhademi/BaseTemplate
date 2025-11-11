namespace MDF.Test.Integration.Modules.SymbolModuleTest.ClosingPrice.Requests;

public record ClosingPriceGetPaginatedListRequestTest
{
    public string? Isin { get; set; }
    public string[]? Isins { get; set; }
    public TypeOfClosingPriceIndicatorTest? IndicatorType { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public int? CurrentPage { get; set; }
    public int? SizeOfPage { get; set; }
}
