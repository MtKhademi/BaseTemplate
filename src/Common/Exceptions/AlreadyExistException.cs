namespace Common.Exceptions;

public class AlreadyExistException : BaseException
{
    public AlreadyExistException(string error, string? errorKey = default!) : base(error, errorKey) { }

}

public class AlreadyExistException<TException> : AlreadyExistException
{
    public AlreadyExistException(string error) : base(error, typeof(TException).Name)
    {
    }
}
