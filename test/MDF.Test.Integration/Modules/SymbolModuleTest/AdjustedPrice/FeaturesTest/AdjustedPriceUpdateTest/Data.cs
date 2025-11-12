using MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Requests;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.FeaturesTest.AdjustedPriceUpdateTest;

internal class NotCorrectDataTest : TheoryData<AdjustedPriceUpdateRequestTest, List<string>>
{
    public NotCorrectDataTest()
    {

        Add(new AdjustedPriceUpdateRequestTest(),
            []);

    }
}
