namespace IAMModule.Data.Repositories;

internal class UserPermissionRepository(IAMModuleDbContext dbContext) :
    EFBaseRepository<int, UserPermissionEntity>(dbContext), IUserPermissionRepository
{ }
