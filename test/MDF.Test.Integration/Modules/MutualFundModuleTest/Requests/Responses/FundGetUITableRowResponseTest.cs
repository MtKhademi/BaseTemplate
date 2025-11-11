namespace MDF.Test.Integration.Modules.MutualFundModuleTest.Requests.Responses;

public class FundGetUITableRowResponseTest : IUIGridRow
{
    public int? FundId { get; set; }
    public string? SeoregisterNumber { get; set; }
    public string? Title { get; set; }
    public FundProviderTest? FundProvider { get; set; }
    public FundTypeTest? FundType { get; set; }
    public FundXMLTypeTest? FundXMLType { get; set; }
    public string? EnTitle { get; set; }
    public string? DateStart { get; set; }
    public string? DateLastChanged { get; set; }
    public string? DateOfLastRecordNav { get; set; }
    public string? Website { get; set; }
    public string? Isin { get; set; }
    public string? SymbolIsin { get; set; }

    public IList<string> Actions { get; set; } = [];
}
