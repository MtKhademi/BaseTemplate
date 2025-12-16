namespace IAMModule.AccessControl.Repositories;

internal interface IUserPermissionRepository : ICRUDRepository<int, UserPermissionEntity>
{
    Task<IEnumerable<UserPermissionEntity>> GetPermissionsByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<bool> HasPermissionByUserIdAsync(string userId, string permissionName, CancellationToken cancellationToken = default);
}
