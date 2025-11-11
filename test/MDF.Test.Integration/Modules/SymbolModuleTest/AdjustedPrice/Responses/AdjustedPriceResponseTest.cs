namespace MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Responses;

internal class AdjustedPriceResponseTest
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
    public int? AnnouncementId { get; set; }
    public int? DividendId { get; set; }
    public int? CapitalChangeId { get; set; }
    public decimal? AdjustedLastPrice { get; set; }
    public decimal? AdjustedPrice { get; set; }
    public double ClosingPrice { get; set; }
    public double LastTradedPrice { get; set; }
    public bool IsAdjusted { get; set; }
    public bool IsTradable { get; set; }
    public string Date { get; set; }
    public string SymbolName { get; set; }
    public string SymbolIsin { get; set; }

    public int? CapitalChangeCodalCode { get; set; }
    public int? DividendPerShareCodalCode { get; set; }
    public string? AnnouncementPublishDateTime { get; set; }
}
