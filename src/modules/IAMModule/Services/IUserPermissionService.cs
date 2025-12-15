using Infrastructure.Auth.Authorization;

namespace IAMModule.Services;

internal interface IUserPermissionService : IBaseService
{
    Task<HashSet<PermissionEntity>> GetPermissionsAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> HasPermissionAsync(string userId, string permissionName, CancellationToken cancellationToken = default);
}


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

internal class UserPermissionCacheService(IUserPermissionService userPermissionService) : IUserPermissionService
{
    private readonly Dictionary<string, HashSet<PermissionEntity>> _permissionCache = new();
    public async Task<HashSet<PermissionEntity>> GetPermissionsAsync(string userId, CancellationToken cancellationToken = default)
    {
        if (_permissionCache.ContainsKey(userId))
        {
            return _permissionCache[userId];
        }
        var permissions = await userPermissionService.GetPermissionsAsync(userId, cancellationToken);
        _permissionCache[userId] = permissions;
        return permissions;
    }
    public async Task<bool> HasPermissionAsync(string userId, string permissionName, CancellationToken cancellationToken = default)
    {
        var permissions = await GetPermissionsAsync(userId, cancellationToken);
        return permissions.Any(p => p.Name == permissionName);
    }
}