namespace Infrastructure.Exceptions;

public class UnauthorizedException : BaseException
{
    public string? Location { get; init; } = default!;

    public UnauthorizedException(
        string error = "Unauthorized",
         string errorKey = "unauthorized",
         string location = "") :
        base(error, errorKey)
    {
        Location = location;
    }
}

public class UnauthorizedException<TException> : UnauthorizedException
{
    public UnauthorizedException(string location) :
        base(location, typeof(TException).Name)
    {
    }
}