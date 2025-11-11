using MDF.Test.Integration.Modules.MutualFundModuleTest.ExternalResourcesTest.ApiTest.ResponsesTest;
using static MDF.Test.Integration.Modules.MutualFundModuleTest.ExternalResourcesTest.ApiTest.ResponsesTest.ERApiFundResponseTest;

namespace MDF.Test.Integration.Modules.MutualFundModuleTest.ExternalResourcesTest.ApiTest.FeaturesTest.ERApiFundCreateOrUpdateTest;

internal class ERApiFundCreateOrUpdateTestValidData : TheoryData<(
   FundProviderTest fundProvider, string seoRegisterNumber,
    string directoryName, string xmlFile), IEnumerable<ERApiFundResponseTest>>
{
    public ERApiFundCreateOrUpdateTestValidData()
    {
        Add((FundProviderTest.Mofid, "10600", "MofidXmls", "FundsXml.xml"),
        [
            new ERApiFundResponseTest
           {
            RegNo = "10600",
            FundType = 1,
            FundSize = 1,
            Date = "۱۴۰۴/۰۴/۲۹",
            NAVRed = 84169,
            FundProfits = new List<ERApiProfitItemResponseTest>
            {
                new ERApiProfitItemResponseTest
                {
                    Date = "",
                    Value = ""
                }
            },
            NAVSub = 84744,
            NAVStat = 84169,
            InitiationDate = "۱۳۸۷/۰۲/۲۰",
            NetAsset = 68834775130086,
            Units = 817811739,
            UnitsSubDAY = 31634,
            UnitsSubFromFirst = 1924257964,
            UnitsRedDAY = 2895779,
            UnitsRedFromFirst = 1106446225,
            Portfolio = new ERApiPortfolioResponseTest
            {
                Cash = 0,
                Deposit = 0.07,
                Bond = 0,
                FiveBest = 24.24,
                Stock = 96.68,
                Other = 3.25
            },
            Custodian = "موسسه حسابرسی داریا روش",
            CustodianEng = "",
            Guarantor = "",
            GuarantorEng = "",
            ProfitGuarantor = "ندارد",
            Manager = "شرکت سبدگردان مفید",
            ManagerEng = "",
            InvestmentManager = "رضا ابراهیمی قلعه حسن",
            InvestmentManagerEng = "",
            RegistrationManager = "شرکت کارگزاری مفید",
            ExecutiveManager = "ندارد",
            MarketMaker = "",
            Auditor = "موسسه حسابرسی ارقام نگر آریا",
            AuditorEN = "",
            Name = "صندوق سرمایه‌گذاری مشترک پیشتاز",
            NameEng = "Pishtaz",
            WebSite = "pishtazfund.com",
            RetInvNo = 28058,
            InsInvNo = 31,
            RetInvPercent = 31.21,
            InsInvPercent = 68.73,
            NaturalPercent = 0,
            LegalPercent = 0,
            GuaranteedEarningRate = "",
            EstimatedEarningRate = "",
            DividentIntervalPeriod = 0,
            Day1Return = 0.893,
            Day7Return = 3.78,
            Day30Return = -6.18,
            Day90Return = -5.709,
            Day180Return = 3.906,
            Day365Return = 36.02,
            DayFirstReturn = 84069,
            UnitsSub = 31634,
            UnitsRed = 2895779,
            ManagerNationalCode = "14005981364",
            FixedRedemptionFee = "0",
            FixedSubscriptionFee = "0",
            VariableSubscriptionFee = "0",
            VariableSubscriptionFeeUpperLimit = "0",
            SplitRate = "9900",
            // TAVAN specific fields
            BaseUnitsSubscriptionNAV = null,
            BaseUnitsCancelNAV = null,
            BaseUnitsTotalNetAssetValue = null,
            BaseTotalUnit = null,
            BaseUnitsTotalSubscription = null,
            BaseUnitsTotalCancel = null,
            SuperUnitsSubscriptionNAV = null,
            SuperUnitsCancelNAV = null,
            SuperUnitsTotalNetAssetValue = null,
            SuperTotalUnit = null,
            SuperUnitsTotalSubscription = null,
            SuperUnitsTotalCancel = null
        }
        ]);

        Add((FundProviderTest.Tadbir, "12290", "TadbirXmls", "FundsXml.xml"),
      [
          new ERApiFundResponseTest
          {
            RegNo = "12290",
            FundType = 19,
            FundSize = 2,
            Date = "۱۴۰۴/۰۲/۰۱",
            NAVRed = 14802,
            FundProfits = new List<ERApiProfitItemResponseTest>
            {
                new ERApiProfitItemResponseTest
                {
                    Date = "",
                    Value = ""
                }
            },
            NAVSub = 14900,
            NAVStat = 14802,
            InitiationDate = "۱۴۰۳/۰۴/۰۳",
            NetAsset = 61405860398327,
            Units = 4148503996,
            UnitsSubDAY = 0,
            UnitsSubFromFirst = 117,
            UnitsRedDAY = 0,
            UnitsRedFromFirst = 66,
            Portfolio = new ERApiPortfolioResponseTest
            {
                Cash = 0,
                Deposit = 8.37,
                Bond = 0,
                FiveBest = 25.52,
                Stock = 91.12,
                Other = 0.51
            },
            Custodian = "مشاور سرمایه گذاری سهم آشنا",
            CustodianEng = "",
            Guarantor = "نامشخص نامشخص",
            GuarantorEng = "",
            ProfitGuarantor = "نامشخص نامشخص",
            Manager = "شرکت سبدگردان اقتصاد بیدار",
            ManagerEng = "",
            InvestmentManager = "پریسا خسروی, سولماز شبانی تپه بر, محمد علی کمالی",
            InvestmentManagerEng = "",
            RegistrationManager = "شرکت سبدگردان اقتصاد بیدار",
            ExecutiveManager = "ندارد",
            MarketMaker = "شرکت سبدگردان اقتصاد بیدار",
            Auditor = "مؤسسه حسابرسی ارقام نگر آریا",
            AuditorEN = "",
            Name = "صندوق سرمایه گذاری سهامی اهرمی بیدار",
            NameEng = "",
            WebSite = "ahrom.ebidar.ir",
            RetInvNo = 8,
            InsInvNo = 26,
            RetInvPercent = 0,
            InsInvPercent = 0,
            NaturalPercent = 0,
            LegalPercent = 0,
            GuaranteedEarningRate = "",
            EstimatedEarningRate = "20",
            DividentIntervalPeriod = 0,
            Day1Return = 1.804,
            Day7Return = 11.212,
            Day30Return = 29.638,
            Day90Return = 17.498,
            Day180Return = 151.636,
            Day365Return = 0,
            DayFirstReturn = 73.728,
            UnitsSub = 0,
            UnitsRed = 0,
            ManagerNationalCode = "14008261181",
            FixedRedemptionFee = "0",
            FixedSubscriptionFee = "0",
            VariableSubscriptionFee = "0",
            VariableSubscriptionFeeUpperLimit = "0",
            SplitRate = "",
            // TAVAN specific fields
            BaseUnitsSubscriptionNAV = 12837, // 12837.25 truncated to long
            BaseUnitsCancelNAV = 12837,       // 12837.25 truncated to long
            BaseUnitsTotalNetAssetValue = 30212519172651,
            BaseTotalUnit = 2353503996,
            BaseUnitsTotalSubscription = 0,
            BaseUnitsTotalCancel = 0,
            SuperUnitsSubscriptionNAV = 17604,
            SuperUnitsCancelNAV = 17378,
            SuperUnitsTotalNetAssetValue = 31193341225676,
            SuperTotalUnit = 1795000000,
            SuperUnitsTotalSubscription = 0,
            SuperUnitsTotalCancel = 0
        }
      ]);
    }
}