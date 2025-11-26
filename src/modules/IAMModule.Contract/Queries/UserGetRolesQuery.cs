namespace IAMModule.Contract.Queries;

public record UserGetRolesQuery : IQuery<IEnumerable<UserRoleModel>>
{
    public string UserName { get; }

    private UserGetRolesQuery(string userName)
    {
        UserName = userName;
    }


    public static UserGetRolesQuery Create(string userName) => new UserGetRolesQuery(userName: userName);
}