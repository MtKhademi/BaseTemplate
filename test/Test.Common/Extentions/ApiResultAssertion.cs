namespace Test.Infrastructure.Extentions;

public static class ApiResultAssertion
{

    public static void AssertionJwtForbidden(this ApiResultTest? apiResult)
        => apiResult.Assertion(expectedErrorKey: "JwtRESTForbiddenException", expectedStatusCode: ETypeOfApiResultStatusCodeTest.Forbidden);
    public static void Assertion(this ApiResultTest? apiResult,
        ETypeOfApiResultStatusCodeTest? expectedStatusCode = null,
        IEnumerable<string>? expectedErrors = null,
        string? expectedErrorKey = default!)
    {
        apiResult.Should().NotBeNull();
        apiResult.IsSuccess.Should().BeFalse();

        if (expectedStatusCode is not null)
        {
            apiResult.StatusCode.Should().Be(expectedStatusCode);
        }

        if (expectedErrors is not null)
        {
            apiResult.Messages.Should().NotBeNull();
            foreach (var error in expectedErrors)
            {
                apiResult.Messages.Should().Contain(error);
            }
        }
    }
}
