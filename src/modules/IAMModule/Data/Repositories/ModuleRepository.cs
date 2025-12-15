namespace IAMModule.Data.Repositories;

internal class ModuleRepository(IAMModuleDbContext dbContext) :
    EFBaseRepository<int, ModuleEntity>(dbContext), IModuleRepository
{ }
