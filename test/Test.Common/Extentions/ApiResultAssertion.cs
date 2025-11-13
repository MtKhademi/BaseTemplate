namespace Test.Common.Extentions;

public static class ApiResultAssertion
{
    public static void Assertion(this ApiResultTest apiResult, IEnumerable<string>? errors = null, string? errorKey = default!)
    {
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();
        if (errors is not null)
        {
            apiResult.Messages.Should().NotBeNull();
            foreach (var error in errors)
            {
                apiResult.Messages.Should().Contain(error);
            }
        }
    }
}
