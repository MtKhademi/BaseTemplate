namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Responses;

internal class FixedIncomeProfitDailyUITableRowResponseTest
{
    public double? ProfitDailyPrice { get; set; }
    public string? ProfitDailyDate { get; set; }
    public string? ModifiedDate { get; set; }
    public double? SubscriptionProfit { get; set; }
    public decimal? NominalPrice { get; set; }
    public bool? HasAdjusted { get; set; }
    public List<string>? Actions { get; set; } = [];
}
