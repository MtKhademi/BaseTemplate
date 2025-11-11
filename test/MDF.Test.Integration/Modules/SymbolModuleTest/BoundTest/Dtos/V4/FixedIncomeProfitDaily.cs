namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Dtos.V4
{
    public partial class FixedIncomeProfitDailyTest
    {
        public int FixedIncomeIdFk { get; set; }
        public DateTime ProfitDailyDate { get; set; }
        public double ProfitDailyPrice { get; set; }
        public DateTime ModifiedDate { get; set; }
        public double? SubscriptionProfit { get; set; }
        public decimal? NominalPrice { get; set; }
        public bool? HasAdjusted { get; set; }
    }
}
