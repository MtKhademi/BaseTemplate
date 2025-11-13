namespace Test.Common.Requestes;

public enum ETypeOfApiResultStatusCodeTest
{
    Default = 0,
    Success = 1,
    NotFound = 2,
    UnAuthentication = 3,
    UnAuthorization = 4,
    BadRequest = 5,
    InternalServerError = 6,
    AlreadyExist = 7,
}

public record ApiResultTest(
    bool IsSuccess,
    ETypeOfApiResultStatusCodeTest StatusCode = ETypeOfApiResultStatusCodeTest.Default,
    IEnumerable<string> Messages = null!,
    string? ErrorKey = null
);

public record ApiResultTest<TData>(
    bool IsSuccess,
    ETypeOfApiResultStatusCodeTest StatusCode = ETypeOfApiResultStatusCodeTest.Default,
    IEnumerable<string> Messages = null!,
    string? ErrorKey = null,
    TData? Result = default
) : ApiResultTest(IsSuccess, StatusCode, Messages, ErrorKey);