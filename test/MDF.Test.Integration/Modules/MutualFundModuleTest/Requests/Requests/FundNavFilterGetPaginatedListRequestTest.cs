namespace MDF.Test.Integration.Modules.MutualFundModuleTest.Requests.Requests;

public record FundNavFilterGetPaginatedListRequestTest(
    string? EndDateTime = default!,
    string? StartDateTime = default!,
    string? SeoRegisterNumber = default!,
    int? CurrentPage = default!,
    int? SizeOfPage = default!
);
