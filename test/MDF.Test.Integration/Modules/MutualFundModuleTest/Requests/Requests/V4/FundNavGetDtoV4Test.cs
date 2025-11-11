
namespace MDF.Test.Integration.Modules.MutualFundModuleTest.Dtos.FundDtos.V4;

public record FundNavGetDtoV4Test(
    int Id,
    long NavRedemption,
    long NavSubscription,
    double NavStat,
    double NetAsset,
    double NetChange,
    double ChangePercent,
    long InvestorsUnits,
    long UnitsSubscription,
    long UnitsRedemption,
    string? EventDate,
    string? DateLastChange,
    double? InstitutionInvestmentPercent,
    double? RetailInvestmentPercent,
    int? InstitutionInvestmentNo,
    int? RetailInvestmentNo
    );

