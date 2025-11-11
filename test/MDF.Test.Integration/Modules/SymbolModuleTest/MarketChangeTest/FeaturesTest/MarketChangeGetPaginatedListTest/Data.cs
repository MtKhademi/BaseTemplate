using MDF.Test.Integration.Modules.SymbolModuleTest.MarketChangeTest.Requests;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.MarketChangeTest.FeaturesTest.MarketChangeGetPaginatedListTest;

internal class MarketChangeFilterGetDtoV4NotValidData : TheoryData<MarketChangeGetPaginatedListRequestTest, List<string>>
{
    public MarketChangeFilterGetDtoV4NotValidData()
    {
        Add(new MarketChangeGetPaginatedListRequestTest
        {
            NewOpenDate = "123"
        },
            ["New Open Date can not be convert to date => sample YYYY-MM-DD :: 123"]);

        Add(new MarketChangeGetPaginatedListRequestTest
        {
            OldCloseDate = "123"
        },
            ["Old Close Date can not be convert to date => sample YYYY-MM-DD :: 123"]);
    }
}
