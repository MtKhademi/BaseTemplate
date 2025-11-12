using MDF.Test.Integration.Modules.MutualFundModuleTest.Requests.FundRequests;

namespace MDF.Test.Integration.Modules.MutualFundModuleTest.FundsTest.FeaturesTest.FundCreateTest;

internal class FundCreateNotValidData : TheoryData<FundCreaterRequestTest, List<string>>
{
    public FundCreateNotValidData()
    {
        Add(new FundCreaterRequestTest(), [
        "Seoregister Number can not be null or empty",
        "Fund Provider can not be null or empty",
        "Fund Type can not be null or empty",
        "Fund XML can not be null or empty"
            ]);
    }
}
