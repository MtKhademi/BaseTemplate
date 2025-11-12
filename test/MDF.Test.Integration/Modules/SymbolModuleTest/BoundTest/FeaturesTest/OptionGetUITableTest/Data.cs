using MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.Requests;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.BoundTest.FeaturesTest.OptionGetUITableTest;

internal class OptionPaginatedListRequestTestNotValidData : TheoryData<OptionPaginatedListRequestTest, List<string>>
{
    public OptionPaginatedListRequestTestNotValidData()
    {
        var dto = new OptionPaginatedListRequestTest()
        {
            ApplyDate = "2025"
        };
        Add(dto, [
            "ApplyDate is not in a valid format."]);

    }
}
