namespace MDF.Test.Integration.Modules.MutualFundModuleTest.Requests.FundRequests;

internal class FundNavFilterGetPaginatedListDtoV4Test
{
    public string? EndDateTime { get; set; }
    public string? StartDateTime { get; set; }
    public string? Isin { get; set; }
    public int? CurrentPage { get; set; }
    public int? SizeOfPage { get; set; }

}
