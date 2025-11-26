namespace IAMModule.UserManagement.Features.UserGetRoles;

internal class UserGetRolesHandler(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager) :
    IQueryHandler<UserGetRolesQuery, IEnumerable<UserRoleModel>>
{
    public async Task<IEnumerable<UserRoleModel>> Handle(UserGetRolesQuery query, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameOrThrowAsync(query.UserName);

        var roleNames = await userManager.GetRolesAsync(user);

        var roles = roleNames.SelectMany(roleName => roleManager.Roles.Where(r => r.Name == roleName)).ToList();

        return roles.Select(role => new UserRoleModel(
            UserName: user.UserName!,
            UserId: user.Id,
            RoleId: role.Id,
            RoleName: role.Name!,
            RoleDescription: role.Description
        ));
    }
}
