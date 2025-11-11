namespace SymbolModule.Contract.Symbold.Requests;

public class SymbolGetPaginatedListRequestTest
{
    public bool? IsDisable { get; set; }
    public string? MarketCode { get; set; }
    public bool? HasFirm { get; set; }
    public int? FirmId { get; set; }
    public string? SymbolNameOrIsin { get; set; }
    public string[]? Isins { get; set; }
    public string[]? TypeOfSymbols { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public string? FromDateChanged { get; set; }
    public string? ToDateChanged { get; set; }
    public int? CurrentPage { get; set; }
    public int? SizeOfPage { get; set; }

}

