using MDF.Test.Integration.Modules.MutualFundModuleTest.ExternalResourcesTest.ApiTest.ResponsesTest;

namespace MDF.Test.Integration.Modules.MutualFundModuleTest.ExternalResourcesTest.ApiTest.FeaturesTest.ERApiFundNavCreateOrUpdate;

internal class FundNavFromApiDataValid : TheoryData<(
    string seoRegister,
    FundProviderTest fundProvider,
    FundXMLTypeTest? fundXMLType,
    string dateForGetNav,
    string directoryName,
    string xmlFile), ERApiFundNavResponseTest>
{
    public FundNavFromApiDataValid()
    {

        Add(("12033", FundProviderTest.Mofid, FundXMLTypeTest.Leverage,
            "2025-04-28",
            "MofidXmls",
            "Fund_Leverage_XML_Leverage_20250428.xml"),
          new ERApiFundNavResponseTest
          (
              SeoRegisterNumber: "12033",
              NavRedemption: 18762,
              NavSubscription: 18762,
              NavStat: 23020,
              NetAsset: 101452665974442,
              InvestorsUnits: 2544113546,
              UnitsSubscription: 20020524,
              UnitsRedemption: 4785719,
              EventDate: "1404/02/08",
              InstitutionInvestmentPercent: 0,
              RetailInvestmentPercent: 0,
              InstitutionInvestmentNo: 36,
              RetailInvestmentNo: 27538
          ));


        Add(("12290", FundProviderTest.Tadbir, FundXMLTypeTest.FixedIncomeFixed,
            "2025-08-09",
            "TadbirXmls",
            "Fund_12290_14040518.xml"),
          new ERApiFundNavResponseTest
          (
              SeoRegisterNumber: "12290",
              NavRedemption: 12926L,
              NavSubscription: 12994L,
              NavStat: 12926,
              NetAsset: 45169789258317,
              InvestorsUnits: 3494357743L,
              UnitsSubscription: 0,
              UnitsRedemption: 0,
              EventDate: "1404/05/18",
              InstitutionInvestmentPercent: 0,
              RetailInvestmentPercent: 0,
              InstitutionInvestmentNo: 25,
              RetailInvestmentNo: 5
          ));

        Add(("12196", FundProviderTest.Tadbir, FundXMLTypeTest.FixedIncomeFixed,
            "2025-08-09",
            "TadbirXmls",
            "Fund_12196_14040518.xml"),
          new ERApiFundNavResponseTest
          (
              SeoRegisterNumber: "12196",
              NavRedemption: 16897L,
              NavSubscription: 17005L,
              NavStat: 16897,
              NetAsset: 115129816376540,
              InvestorsUnits: 6813658710L,
              UnitsSubscription: 0,
              UnitsRedemption: 0,
              EventDate: "1404/05/18",
              InstitutionInvestmentPercent: 0,
              RetailInvestmentPercent: 0,
              InstitutionInvestmentNo: 52,
              RetailInvestmentNo: 2360
          ));

        Add(("12127", FundProviderTest.Tadbir, FundXMLTypeTest.FixedIncomeFixed,
            "2025-08-09",
            "TadbirXmls",
            "Fund_12127_14040518.xml"),
          new ERApiFundNavResponseTest
          (
              SeoRegisterNumber: "12127",
              NavRedemption: 15034L,
              NavSubscription: 15122L,
              NavStat: 15037,
              NetAsset: 43218234423229,
              InvestorsUnits: 2874640420L,
              UnitsSubscription: 0,
              UnitsRedemption: 0,
              EventDate: "1404/05/18",
              InstitutionInvestmentPercent: 0,
              RetailInvestmentPercent: 0,
              InstitutionInvestmentNo: 16,
              RetailInvestmentNo: 5013
          ));

        Add(("10630", FundProviderTest.Tadbir, FundXMLTypeTest.JointStock,
            "2025-08-11",
            "TadbirXmls",
            "Fund_10630_14040520.xml"),
          new ERApiFundNavResponseTest
          (
              SeoRegisterNumber: "10630",
              NavRedemption: 8691197,
              NavSubscription: 8745536,
              NavStat: 8691197,
              NetAsset: 871136095613,
              InvestorsUnits: 100232,
              UnitsSubscription: 0,
              UnitsRedemption: 0,
              EventDate: "1404/05/20",
              InstitutionInvestmentPercent: 90.72,
              RetailInvestmentPercent: 9.28,
              InstitutionInvestmentNo: 14,
              RetailInvestmentNo: 110
          ));


        Add(("11277", FundProviderTest.Mofid, FundXMLTypeTest.FixedIncomeFixed,
            "2025-11-08T10:37:11.250Z",
            "MofidXmls",
            "Fund_11277_14040814.xml"),
          new ERApiFundNavResponseTest
          (
              SeoRegisterNumber: "11277",
              NavRedemption: 104969,
              NavSubscription: 105018,
              NavStat: 107830,
              NetAsset: 790127805356054,
              InvestorsUnits: 7527252124,
              UnitsSubscription: 173461621,
              UnitsRedemption: 384596758,
              EventDate: "1404/08/14",
              InstitutionInvestmentPercent: 2.2,
              RetailInvestmentPercent: 95.4,
              InstitutionInvestmentNo: 906,
              RetailInvestmentNo: 2241897
          ));
    }
}
