using IAMModule.Services;

namespace IAMModule.Data.Repositories;

internal class PermissionRepository(IAMModuleDbContext context) :
    EFBaseRepository<int, PermissionEntity>(context), IPermissionRepository
{


    public override IQueryable<PermissionEntity> QueryTracking()
    {
        return context.Permissions
            .Include(x => x.Feature)
            .ThenInclude(x => x.Module);
    }

    public override IQueryable<PermissionEntity> QueryNoTracking()
        => QueryTracking().AsNoTracking();


    public async Task<IEnumerable<PermissionEntity>> GetPermissionsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.UserPermissions
            .Include(x => x.Permission)
            .Where(x => x.UserId == userId)
            .Select(x => x.Permission)
            .ToHashSetAsync(cancellationToken);
    }

    public async Task<bool> HasPermissionByUserIdAsync(string userId, string permissionName, CancellationToken cancellationToken = default)
    {
        return await context.UserPermissions
            .Include(x => x.Permission)
            .Where(x => x.UserId == userId)
            .Select(x => x.Permission.Name)
            .AnyAsync(x => x == permissionName, cancellationToken);
    }
}