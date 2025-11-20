namespace IAMModule.Role.Extensions;

internal static class RoleManagerExtensions
{
    public static async Task<string> GetRoleIdByNameOrThrowAsync(this RoleManager<ApplicationRole> roleManager, string roleName)
    {
        return (await roleManager.GetRoleByNameOrThrowAsync(roleName)).Id;
    }
    public static async Task<ApplicationRole> GetRoleByNameOrThrowAsync(this RoleManager<ApplicationRole> roleManager, string roleName)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            throw new RoleWithNameNotFoundException(roleName);
        }
        return role;
    }


    public static async Task<bool> RoleExistsByNameAsync(this RoleManager<ApplicationRole> roleManager, string roleName)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        return role != null;
    }

    public static async Task<ApplicationRole> FindByIdOrThrowAsync(this RoleManager<ApplicationRole> roleManager, string roleId)
    {
        var role = await roleManager.FindByIdAsync(roleId);
        if (role == null)
        {
            throw new RoleWithRoleIdNotFoundException(roleId);
        }

        return role;
    }
}
