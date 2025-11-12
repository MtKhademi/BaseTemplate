using SymbolUpdateDtoTest = MDF.Test.Integration.Modules.SymbolModuleTest.SymbolTest.Requests.SymbolUpdateDtoTest;

namespace MDF.Test.Integration.Modules.SymbolModuleTest.SymbolTest.FeaturesTest.SymbolUpdateTest;

internal class NotCorrectDataTest : TheoryData<SymbolUpdateDtoTest, List<string>>
{
    public NotCorrectDataTest()
    {
        var dtEvent = (new DateTime(2025, 05, 06, 14, 50, 00));
        var dto = new SymbolUpdateDtoTest();
        Add(dto, new List<string>
        {
            "ISIN must be at least 7 characters long."
        });

    }
}
