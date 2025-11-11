namespace Common.Exceptions;

public class ForbiddenException : BaseException
{
    public string? Location { get; init; } = default!;

    public ForbiddenException(
        string error = "Unauthorized",
         string errorKey = "unauthorized",
         string location = "") :
        base(error, errorKey)
    {
        Location = location;
    }
}

public class ForbiddenException<TException> : ForbiddenException
{
    public ForbiddenException(string location) :
        base(typeof(TException).Name, location)
    {
    }
}