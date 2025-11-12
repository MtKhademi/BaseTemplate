namespace MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Requests;

public class AdjustedPriceGetPaginatedListRequestTest
{
    public string? StartDate = default!;
    public string? EndDate = default!;
    public string? Date = default!;
    public string? Isins = default!;
    public bool? IsDeleted = default!;

    public string? StartAnnouncementPublishDateTime { get; set; } = default!;
    public string? EndAnnouncementPublishDateTime { get; set; } = default!;
    public string? CodalCode { get; set; } = default!;
}
