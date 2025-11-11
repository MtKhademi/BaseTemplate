namespace Common.Exceptions;

public class AlreadyExistDataException : BaseException
{
    public AlreadyExistDataException(string error, string? errorKey = default!) : base(error, errorKey) { }

}

public class AlreadyExistDataException<TException> : AlreadyExistDataException
{
    public AlreadyExistDataException(string error) : base(error, typeof(TException).Name)
    {
    }
}
