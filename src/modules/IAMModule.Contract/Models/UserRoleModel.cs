namespace IAMModule.Contract.Models;

public record UserRoleModel(
    string UserName,
    string UserId,
    string RoleId,
    string RoleName,
    string RoleDescription)
{
    public UserRoleResponse ToUserRoleResponse()
        => new UserRoleResponse(
            UserId: UserId,
            UserName: UserName,
            RoleId: RoleId,
            RoleName: RoleName,
            RoleDescription: RoleDescription
        );
}