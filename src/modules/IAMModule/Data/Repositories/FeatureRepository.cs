
namespace IAMModule.Data.Repositories;

internal class FeatureRepository(IAMModuleDbContext dbContext) :
    EFBaseRepository<int, FeatureEntity>(dbContext), IFeatureRepository
{ }