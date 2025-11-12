namespace MDF.Test.Integration.Modules.MutualFundModuleTest.Dtos.FundDtos.V4;

public class FundFilterGetPaginatedListDtoTest : FundFilterGetListDtoTest
{
    public int? CurrentPage { get; set; } = default!;
    public int? SizeOfPage { get; set; } = default!;
}
