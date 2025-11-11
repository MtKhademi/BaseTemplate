namespace MDF.Test.Integration.Modules.MutualFundModuleTest.ExternalResourcesTest.ApiTest.ResponsesTest;

public record ERApiFundNavResponseTest(
     string SeoRegisterNumber,
     long NavRedemption,
     long NavSubscription,
     double NavStat,
     long NetAsset,
     long InvestorsUnits,
     long UnitsSubscription,
     long UnitsRedemption,
     string? EventDate,
     double? InstitutionInvestmentPercent,
     double? RetailInvestmentPercent,
     int? InstitutionInvestmentNo,
     int? RetailInvestmentNo
    );


public record ERFundNavApiCreateOrUpdateResponseTest(
    FundProviderTest Provider,
    string Message,
    IEnumerable<ERFundNavApiCreateOrUpdateResponseBaseProviderTest> NavResults);

public record ERFundNavApiCreateOrUpdateResponseBaseProviderTest(
    string SeoRegisterNumber, string Message, ERApiFundNavResponseTest? Nav);