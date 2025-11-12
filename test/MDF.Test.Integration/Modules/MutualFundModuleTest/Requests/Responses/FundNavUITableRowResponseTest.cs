namespace MDF.Test.Integration.Modules.MutualFundModuleTest.Requests.Responses;

internal class FundNavUITableRowResponseTest : IUIGridRow
{
    public int? Id { get; set; }
    public long? NavRedemption { get; set; }
    public long? NavSubscription { get; set; }
    public double? NavStat { get; set; }
    public double? NetAsset { get; set; }
    public double? NetChange { get; set; }
    public double? ChangePercent { get; set; }
    public long? InvestorsUnits { get; set; }
    public long? UnitsSubscription { get; set; }
    public long? UnitsRedemption { get; set; }
    public string? EventDate { get; set; }
    public string? DateLastChange { get; set; }
    public double? InstitutionInvestmentPercent { get; set; }
    public double? RetailInvestmentPercent { get; set; }
    public int? InstitutionInvestmentNo { get; set; }
    public int? RetailInvestmentNo { get; set; }
    

    public int? FundId { get; set; }
    public string? FundSeoRegisterNumber { get; set; }
    
    public IList<string> Actions { get; set; }
}