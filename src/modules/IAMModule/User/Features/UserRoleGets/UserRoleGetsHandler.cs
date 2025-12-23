namespace IAMModule.User.Features.UserRoleGets;

internal class UserRoleGetsHandler(
    UserManager<ApplicationUser> userManager,
    RoleManager<ApplicationRole> roleManager) :
    IQueryHandler<UserRoleGetsQuery, UserRoleModel>
{
    public async Task<UserRoleModel> Handle(UserRoleGetsQuery query, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdOrThrowAsync(query.UserId);

        var roleNames = await userManager.GetRolesAsync(user);

        var roles = roleNames.SelectMany(roleName => roleManager.Roles.Where(r => r.Name == roleName)).ToList();

        return new UserRoleModel(
            UserName: user.UserName!,
            UserId: user.Id,
            Roles: roles.Select(role => new RoleModel(
                Id: role.Id,
                Name: role.Name!,
                Description: role.Description
            )
        ));
    }
}
