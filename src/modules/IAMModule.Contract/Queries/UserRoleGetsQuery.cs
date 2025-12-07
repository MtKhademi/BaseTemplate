namespace IAMModule.Contract.Queries;

public record UserRoleGetsQuery : IQuery<UserRoleModel>
{
    public string UserId { get; }

    private UserRoleGetsQuery(string userId)
    {
        UserId = userId;
    }


    public static UserRoleGetsQuery Create(string userId) => new UserRoleGetsQuery(userId: userId);
}