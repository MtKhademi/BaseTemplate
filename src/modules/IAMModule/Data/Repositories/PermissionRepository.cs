namespace IAMModule.Data.Repositories;

internal class PermissionRepository(IAMModuleDbContext dbContext) :
    EFBaseRepository<int, PermissionEntity>(dbContext), IPermissionRepository
{ }
