namespace MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Responses;

public class AdjustedPriceUITableRowResponseTest
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive => !IsDeleted;
    public bool IsTradable { get; set; }
    public string? Date { get; set; }
    public string? SymbolName { get; set; }
    public string? SymbolIsin { get; set; }
    public decimal AdjustedLastPrice { get; set; }
    public decimal AdjustedPrice { get; set; }
    public decimal ClosingPrice { get; set; }
    public decimal LastTradedPrice { get; set; }

    public bool IsAdjusted { get; set; }

    public int? DividendId { get; set; }
    public int? AnnouncementId { get; set; }
    public int? CapitalChangeId { get; set; }
    public int? CapitalChangeCodalCode { get; set; }
    public int? DividendPerShareCodalCode { get; set; }
    public IList<string> Actions { get; set; }
}
