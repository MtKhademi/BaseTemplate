
namespace IAMModule.Data.Repositories;

internal class UserPermissionRepository(IAMModuleDbContext dbContext) :
    EFBaseRepository<int, UserPermissionEntity>(dbContext), IUserPermissionRepository
{

    public async Task<IEnumerable<UserPermissionEntity>> GetPermissionsByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await QueryNoTracking()
            .Where(up => up.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> HasPermissionByUserIdAsync(string userId, string permissionName, CancellationToken cancellationToken = default)
    {
        return await QueryNoTracking()
            .AnyAsync(up => up.UserId == userId && up.Permission.Name == permissionName, cancellationToken);
    }
}
