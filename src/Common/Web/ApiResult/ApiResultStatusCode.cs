namespace Infrastructure.Web.ApiResult;

public enum ApiResultStatusCode
{
    Default = 0,
    Success = 1,
    NotFound = 2,
    Unauthenticated = 3,
    Unauthorized = 4,
    BadRequest = 5,
    InternalServerError = 6,
    AlreadyExists = 7,
    Forbidden = 8
}
