namespace MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Requests;

public class AdjustedPriceUpdateRequestTest
{
    public int? AdjustedPriceId { get; set; }
    public int? CodalCode { get; set; }
    public decimal? AdjustedPrice { get; set; }
    public decimal? AdjustedLastPrice { get; set; }
    public bool? IsAdjusted { get; set; }
}
