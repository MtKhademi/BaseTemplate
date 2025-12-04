namespace Infrastructure.Web.ApiResult;

public static class ApiResultExtensions
{
    public static ApiResult<T> ToApiResultSuccess<T>(this T data, params string[]? messages)
        => ApiResult<T>.Success(data, messages);
}