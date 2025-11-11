using MDF.Test.Integration.Modules.MutualFundModuleTest.Requests.FundRequests;

namespace MDF.Test.Integration.Modules.MutualFundModuleTest.FundsTest.FeaturesTest.FundUpdateTest;

internal class FundCreateNotValidData : TheoryData<FundUpdateRequestTest, List<string>>
{
    public FundCreateNotValidData()
    {
        Add(new FundUpdateRequestTest(), [
            "SeoregisterNumber can not be null or empty"
            ]);

        Add(new FundUpdateRequestTest(SeoregisterNumber: "123", DateStart: "123-"), [
            "Not correct format : 123-"
    ]);
    }
}
