namespace Common.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string error, string? errorKey = default!) : base(error, errorKey)
    {

    }
}

public class NotFoundException<TException> : NotFoundException
{
    public NotFoundException(string error) : base(error, typeof(TException).Name)
    {

    }
}
