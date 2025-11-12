namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Responses;

internal class OptionResponseTest
{
    public int? OptionId { get; set; }
    public int? SymbolBaseId { get; set; }
    public string? SymbolBaseName { get; set; }
    public string? SymbolBaseIsin { get; set; }
    public int? SymbolOptionId { get; set; }
    public string? SymbolOptionName { get; set; }
    public string? SymbolOptionIsin { get; set; }
    public decimal? ApplyPrice { get; set; }
    public string? State { get; set; }
    public string? ApplyDate { get; set; }
    public string? StartDate { get; set; }
    public string? OptionForFinance { get; set; }
    public string? OptionForFinanceDescription { get; set; }
    public IEnumerable<OptionResponseTest>? Details { get; set; }
    public string? AnnouncementId { get; set; }
    public string? EntryDate { get; set; }
    public string? ModifyDate { get; set; }
    public bool? IsDeleted { get; set; }
    public string? OpFiUpdateDate { get; set; }
    public short? OpFiUpdateMode { get; set; }
    public string? ConfirmedManualTimeAnnoucement { get; set; }
}
