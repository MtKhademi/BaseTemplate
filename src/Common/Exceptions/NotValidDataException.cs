namespace Infrastructure.Exceptions;

public class NotValidDataException : BaseException
{
    public IEnumerable<string> Errors { get; init; } = new List<string>();
    public NotValidDataException(
        string error,
        string? errorKey = default!) : base(error, errorKey)
    {
    }

    public NotValidDataException(
        IEnumerable<string> errors,
        string? errorKey = default!) : base("Not valid data", errorKey)
    {
        this.Errors = errors;
    }

}


public class NotValidDataException<TException> : NotValidDataException
{
    public NotValidDataException(string error) : base(error, typeof(TException).Name)
    {
    }
    public NotValidDataException(IEnumerable<string> errors) : base(errors, typeof(TException).Name)
    {
    }
}