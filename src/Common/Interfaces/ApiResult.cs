
namespace Common.Interfaces;

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

public class ApiResult
{
    public bool IsSuccess { get; set; }
    public ApiResultStatusCode StatusCode { get; set; } = ApiResultStatusCode.Default;
    public List<string> Messages { get; set; } = new();
    public string? ErrorKey { get; set; }

    public override string ToString() => $"{IsSuccess} - {StatusCode} - [{string.Join(" | ", Messages)}]";

    public static ApiResult Success(params string[]? messages)
        => new()
        {
            IsSuccess = true,
            StatusCode = ApiResultStatusCode.Success,
            Messages = messages?.ToList() ?? new List<string> { "SUCCESS" }
        };

    public static ApiResult BadRequest(params string[] messages)
        => new()
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.BadRequest,
            Messages = messages.ToList()
        };
    public static ApiResult BadRequest(IEnumerable<string> messages)
        => new()
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.BadRequest,
            Messages = messages.ToList()
        };
    public static ApiResult BadRequest(NotValidDataException ex)
        => new()
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.BadRequest,
            Messages = ex.Errors.ToList(),
            ErrorKey = ex.ErrorKey
        };

    public static ApiResult NotFound(params string[] messages)
        => new()
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.NotFound,
            Messages = messages.ToList()
        };
    public static ApiResult NotFound(NotFoundException ex)
        => new()
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.NotFound,
            Messages = [ex.Message],
            ErrorKey = ex.ErrorKey
        };



    public static ApiResult AlreadyExists(params string[] messages)
        => new()
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.AlreadyExists,
            Messages = messages.ToList()
        };
    public static ApiResult AlreadyExists(AlreadyExistException ex)
        => new()
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.AlreadyExists,
            Messages = [ex.Message],
            ErrorKey = ex.ErrorKey
        };

    //public static object? UnAuthorize(string v1, string v2)


    public static ApiResult Unauthorized()
        => new()
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.Unauthorized,
            Messages = new List<string> { "Unauthorized" }
        };
    public static ApiResult Unauthorized(UnauthorizedException ex)
        => new()
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.Unauthorized,
            Messages = new List<string> { "Unauthorized", ex.Location },
            ErrorKey = ex.ErrorKey
        };
    public static ApiResult Unauthenticated(params string[] messages)
        => new()
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.Unauthenticated,
            Messages = messages.ToList()
        };

    public static ApiResult InternalServerError(Exception ex)
        => new()
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.InternalServerError,
            Messages = new List<string> { "Internal server error" }
        };
    public static ApiResult InternalServerError(params string[]? messages)
        => new()
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.InternalServerError,
            Messages = messages?.ToList() ?? new List<string> { "Internal server error" }
        };

    public static ApiResult Forbidden(ForbiddenException ex)
        => new()
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.Forbidden,
            Messages = new List<string> { "Forbidden", ex.Location },
            ErrorKey = ex.ErrorKey
        };
}

public class ApiResult<T> : ApiResult
{
    public T? Result { get; set; }

    public override string ToString()
        => base.ToString() + $" RESULT : {Result}";

    public static ApiResult<T> Success(T data, params string[]? messages)
        => new()
        {
            IsSuccess = true,
            StatusCode = ApiResultStatusCode.Success,
            Messages = messages?.ToList() ?? new List<string> { "SUCCESS" },
            Result = data
        };

    public static ApiResult<T> BadRequest(params string[] messages)
        => new()
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.BadRequest,
            Messages = messages.ToList(),
            Result = default
        };

    public static ApiResult<T> BadRequest(IEnumerable<string> messages)
        => new()
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.BadRequest,
            Messages = messages.ToList(),
            Result = default
        };

    public static ApiResult<T> BadRequest(T result, string message)
        => new()
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.BadRequest,
            Messages = new List<string> { message },
            Result = result
        };

    public static ApiResult<T> InternalServerError(params string[]? messages)
        => new()
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.InternalServerError,
            Messages = messages?.ToList() ?? new List<string> { "Internal server error" },
            Result = default
        };

    public static ApiResult<T> NotFound(params string[] messages)
        => new()
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.NotFound,
            Messages = messages.ToList(),
            Result = default
        };
}

public static class ApiResultExtensions
{
    public static ApiResult<T> ToApiResultSuccess<T>(this T data, params string[]? messages)
        => ApiResult<T>.Success(data, messages);
}