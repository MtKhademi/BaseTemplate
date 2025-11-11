namespace Common.Exceptions;

public class NotAccessException : BaseException
{
    public string? Action { get; init; } = default!;
    public string? Location { get; init; } = default!;
    public string? Data { get; init; } = default!;

    public NotAccessException(
        string error = "Access denied",
         string errorKey = "not_access",
         string location = "",
         string action = "",
         string data = "") :
        base(error, errorKey)
    {
        Action = action;
        Location = location;
        Data = data;
    }
}

public class NotAccessException<TException> : NotAccessException
{
    public NotAccessException(
        string error = "Access denied",
        string location = "",
        string action = "",
        string data = "") :
        base(error, typeof(TException).Name, location, action, data)
    {
    }
}