namespace Common.Exceptions;

public abstract class BaseException : Exception
{
    public string? ErrorKey { get; init; } = default!;
    protected BaseException(
        string error, 
        string? errorKey = default!,
        Exception? ex = null) : base(error, ex)
    {
        ErrorKey = errorKey;
    }
}
