using Infrastructure.Auth.Authorization;

namespace IAMModule.Auth.Services;

internal interface IUserPermissionService : IBaseRepository
{
    Task<HashSet<PermissionEntity>> GetPermissionsAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> HasPermissionAsync(string userId, string permissionName, CancellationToken cancellationToken = default);
}


[DIScope(DIScopeType.Scope)]
internal class UserPermissionSerivce(IAMModuleDbContext context) : IUserPermissionService
{
    public async Task<HashSet<PermissionEntity>> GetPermissionsAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.UserPermissions
            .Include(x => x.AppPermission)
            .Where(x => x.UserId == userId)
            .Select(x => x.AppPermission)
            .ToHashSetAsync(cancellationToken);
    }

    public async Task<bool> HasPermissionAsync(string userId, string permissionName, CancellationToken cancellationToken = default)
    {

        var xx = context.UserPermissions.ToList();
        var xx2 = context.UserPermissions
            .Include(x => x.AppPermission)
            .Where(x => x.UserId == userId)
            .Select(x => x.AppPermission.Name).ToList();

        return await context.UserPermissions
            .Include(x => x.AppPermission)
            .Where(x => x.UserId == userId)
            .Select(x => x.AppPermission.Name)
            .AnyAsync(x => x == permissionName, cancellationToken);
    }
}