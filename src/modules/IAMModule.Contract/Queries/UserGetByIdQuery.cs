namespace IAMModule.Contract.Queries;

public record UserGetByIdQuery : IQuery<ApplicationUserModel>
{
    public string UserId { get; init; }


    public UserGetByIdQuery(string userId)
    {
        if(string.IsNullOrWhiteSpace(userId))
        {
            throw new UserGetByIdQueryException($"{nameof(UserId)} cannot be null or empty.");
        }

        this.UserId = userId;
    }

    internal class UserGetByIdQueryException : NotValidDataException<UserGetByIdQueryException>
    {
        public UserGetByIdQueryException(string error) : base(error)
        {
        }
    }
}