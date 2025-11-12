namespace MDF.Test.Integration.Modules.MutualFundModuleTest.ExternalResourcesTest.ApiTest.ResponsesTest;

public class ERApiFundResponseTest
{
    public string RegNo { get; set; }
    public int FundType { get; set; }
    public int FundSize { get; set; }
    public string Date { get; set; }
    public long NAVRed { get; set; }
    public List<ERApiProfitItemResponseTest> FundProfits { get; set; }
    public long NAVSub { get; set; }
    public double NAVStat { get; set; }
    public string InitiationDate { get; set; }
    public long NetAsset { get; set; }
    public long Units { get; set; }
    public long UnitsSubDAY { get; set; }
    public long UnitsSubFromFirst { get; set; }
    public long UnitsRedDAY { get; set; }
    public long UnitsRedFromFirst { get; set; }
    public ERApiPortfolioResponseTest Portfolio { get; set; }
    public string Custodian { get; set; }
    public string CustodianEng { get; set; }
    public string Guarantor { get; set; }
    public string GuarantorEng { get; set; }
    public string ProfitGuarantor { get; set; }
    public string Manager { get; set; }
    public string ManagerEng { get; set; }
    public string InvestmentManager { get; set; }
    public string InvestmentManagerEng { get; set; }
    public string RegistrationManager { get; set; }
    public string ExecutiveManager { get; set; }
    public string MarketMaker { get; set; }
    public string Auditor { get; set; }
    public string AuditorEN { get; set; }
    public string Name { get; set; }
    public string NameEng { get; set; }
    public string WebSite { get; set; }
    public int RetInvNo { get; set; }
    public int InsInvNo { get; set; }
    public double RetInvPercent { get; set; }
    public double InsInvPercent { get; set; }
    public double NaturalPercent { get; set; }
    public double LegalPercent { get; set; }
    public string GuaranteedEarningRate { get; set; }
    public string EstimatedEarningRate { get; set; }
    public int DividentIntervalPeriod { get; set; }
    public double Day1Return { get; set; }
    public double Day7Return { get; set; }
    public double Day30Return { get; set; }
    public double Day90Return { get; set; }
    public double Day180Return { get; set; }
    public double Day365Return { get; set; }
    public double DayFirstReturn { get; set; }
    public long UnitsSub { get; set; }
    public long UnitsRed { get; set; }
    public string ManagerNationalCode { get; set; }
    public string FixedRedemptionFee { get; set; }
    public string FixedSubscriptionFee { get; set; }
    public string VariableSubscriptionFee { get; set; }
    public string VariableSubscriptionFeeUpperLimit { get; set; }
    public string SplitRate { get; set; }

    // TAVAN specific fields
    public long? BaseUnitsSubscriptionNAV { get; set; }
    public long? BaseUnitsCancelNAV { get; set; }
    public long? BaseUnitsTotalNetAssetValue { get; set; }
    public long? BaseTotalUnit { get; set; }
    public long? BaseUnitsTotalSubscription { get; set; }
    public long? BaseUnitsTotalCancel { get; set; }
    public long? SuperUnitsSubscriptionNAV { get; set; }
    public long? SuperUnitsCancelNAV { get; set; }
    public long? SuperUnitsTotalNetAssetValue { get; set; }
    public long? SuperTotalUnit { get; set; }
    public long? SuperUnitsTotalSubscription { get; set; }
    public long? SuperUnitsTotalCancel { get; set; }

    public class ERApiPortfolioResponseTest
    {
        public double Cash { get; set; }
        public double Deposit { get; set; }
        public double Bond { get; set; }
        public double FiveBest { get; set; }
        public double Stock { get; set; }
        public double Other { get; set; }
    }

    public class ERApiProfitItemResponseTest
    {
        public string Date { get; set; }
        public string Value { get; set; }
    }

}

