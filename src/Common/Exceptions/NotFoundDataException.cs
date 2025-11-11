namespace Common.Exceptions;

public class NotFoundDataException : BaseException
{
    public NotFoundDataException(string error, string? errorKey = default!) : base(error, errorKey)
    {

    }
}

public class NotFoundDataException<TException> : NotFoundDataException
{
    public NotFoundDataException(string error) : base(error, typeof(TException).Name)
    {

    }
}
