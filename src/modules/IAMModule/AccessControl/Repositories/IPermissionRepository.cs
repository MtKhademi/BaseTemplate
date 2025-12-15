namespace IAMModule.AccessControl.Repositories;

internal interface IPermissionRepository : ICRUDRepository<int, PermissionEntity>
{
    Task<IEnumerable<PermissionEntity>> GetPermissionsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> HasPermissionByUserIdAsync(string userId, string permissionName, CancellationToken cancellationToken = default);
}
