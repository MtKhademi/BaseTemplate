using MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.Requests;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.AdjustedPrice.FeaturesTest.AdjustedPriceCreateTest;

internal class NotCorrectDataTest : TheoryData<AdjustedPriceCreateRequestTest, List<string>>
{
    public NotCorrectDataTest()
    {

        Add(new AdjustedPriceCreateRequestTest(),
            []);

    }
}
