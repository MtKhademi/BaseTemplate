namespace IAMModule.Contract.Commands;

public record UserRoleChangeCommand : ICommand<Unit>
{

    private UserRoleChangeCommand(string userId, List<string> roleIds)
    {

        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(userId))
            errors.Add($"{nameof(userId)} cannot be empty");

        if (roleIds is null || !roleIds.Any() || roleIds.Any(string.IsNullOrWhiteSpace))
            errors.Add($"{nameof(roleIds)} cannot be empty");

        if (errors.Any())
            throw new UserRoleChangeCommandException(errors);


        UserId = userId!;
        RoleIds = roleIds!.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
    }

    public string UserId { get; }
    public List<string> RoleIds { get; }

    public static UserRoleChangeCommand Create(string userId, List<string> roleIds) => new UserRoleChangeCommand(userId, roleIds);
    public static UserRoleChangeCommand Create(UserRolesChangeRequest request) =>
        new UserRoleChangeCommand(request.UserId, request.RoleIds?.ToList() ?? []);
    internal class UserRoleChangeCommandException : NotValidDataException<UserRoleChangeCommandException>
    {
        public UserRoleChangeCommandException(IEnumerable<string> errors) :
            base(errors)
        {
        }
    }
}