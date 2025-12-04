namespace Infrastructure.Exceptions;

public class NotHandleException : BaseException
{
    public NotHandleException(string error,
        string? errorKey = default, 
        Exception? ex = default!) :
        base($"[NOT HANDLED ERROR] - {error} : ", errorKey, ex)
    {
    }
}

public class NotHandleException<TException> : NotHandleException
{
    public NotHandleException(string error, Exception? ex = default!) :
        base(error, typeof(TException).Name, ex)
    {
    }
}