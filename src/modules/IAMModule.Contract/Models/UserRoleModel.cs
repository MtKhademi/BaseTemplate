namespace IAMModule.Contract.Models;

public record UserRoleModel(
    string UserName,
    string UserId,
    IEnumerable<RoleModel> Roles)
{
    public UserRoleResponse ToUserRoleResponse()
        => new UserRoleResponse(
            UserId: UserId,
            UserName: UserName,
            Roles: Roles.Select(role => role.ToRoleResponse()).ToList()
        );
}