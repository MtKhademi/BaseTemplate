namespace MDF.Test.Integration.Modules.MutualFundModuleTest.Requests.FundRequests;

public record FundCreaterRequestTest(
string? SeoregisterNumber = default!,
string? Title = default!,
FundProviderTest? FundProvider = default!,
FundTypeTest? FundType = default!,
FundXMLTypeTest? FundXML = default!,
string? EnTitle = default!,
string? DateStart = default!,
string? DateLastChanged = default!,
string? Website = default!,
string? Isin = default!,
string? SymbolIsin = default!,
int? OrganizationId = default,
int? ManagerId = default);
