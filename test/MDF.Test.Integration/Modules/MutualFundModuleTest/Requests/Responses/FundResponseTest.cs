using MutualFundModule.Contract.Fund.Responses;

namespace MDF.Test.Integration.Modules.MutualFundModuleTest.Requests.Responses;

public record FundResponseTest(
    int? FundId,
    string? SeoregisterNumber,
    string? Title,
    FundProviderTest? FundProvider,
    FundTypeTest? FundType,
    FundXMLTypeTest? FundXMLType,
    string? EnTitle,
    string? DateStart,
    string? DateLastChanged,
    string? DateOfLastRecordNav,
    string? Website,
    string? Isin,
    string? SymbolIsin,

    int? OrganizationId = default,
    string? OrganizationName = default,

    int? ManagerId = default,
    string? ManagerName = default,

    string? FundNationalCode = default,
    List<FundFeeResponseTest>? FundFees = default
    );



