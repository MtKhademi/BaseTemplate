using MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Requests;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.FeaturesTest.AdjustedPriceGetPaginatedList;

internal class AdjustedPriceGetListRequestNotValidData : TheoryData<AdjustedPriceGetPaginatedListRequestTest, List<string>>
{
    public AdjustedPriceGetListRequestNotValidData()
    {
        Add(new AdjustedPriceGetPaginatedListRequestTest
        {
            Date = "123",
            EndDate = "123",
            StartDate = "2025-05"
        },
            [
               "Date can not be convert to date =\u003E sample YYYY-MM-DD :: 123",
               "StartDate can not be convert to date =\u003E sample YYYY-MM-DD :: 2025-05",
               "EndDate can not be convert to date =\u003E sample YYYY-MM-DD :: 123"
  ]);
    }
}
