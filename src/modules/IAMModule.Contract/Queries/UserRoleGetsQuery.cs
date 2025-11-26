namespace IAMModule.Contract.Queries;

public record UserRoleGetsQuery : IQuery<IEnumerable<UserRoleModel>>
{
    public string UserName { get; }

    private UserRoleGetsQuery(string userName)
    {
        UserName = userName;
    }


    public static UserRoleGetsQuery Create(string userName) => new UserRoleGetsQuery(userName: userName);
}