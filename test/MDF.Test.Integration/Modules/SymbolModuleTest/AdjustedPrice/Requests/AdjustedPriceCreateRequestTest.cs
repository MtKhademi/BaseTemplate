namespace MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Requests;

public class AdjustedPriceCreateRequestTest
{
    public string? Isin { get; set; } = default!;
    public decimal? Price { get; set; } = default!;
    public decimal? LastPrice { get; set; } = default!;
    public decimal? AdjustedPrice { get; set; } = default!;
    public decimal? AdjustedLastPrice { get; set; } = default!;
    public bool IsAdjusted { get; set; } = default;
    public string? Date { get; set; } = default!;

}