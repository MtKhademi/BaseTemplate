namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Responses;

internal class FixedIncomeProfitDailyResponseTest
{
    public string? Isin { get; set; }
    public double? ProfitDailyPrice { get; set; }
    public string? ProfitDailyDate { get; set; }
    public string? ModifiedDate { get; set; }
    public double? SubscriptionProfit { get; set; }
    public decimal? NominalPrice { get; set; }
    public bool? HasAdjusted { get; set; }
}
