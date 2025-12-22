namespace IAMModule.User.Features.GetByKey;

internal class GetByKeyHandler(ICacheService cacheService)
    : IQueryHandler<CacheGetByKeyQuery, string>
{
    public async Task<string> Handle(CacheGetByKeyQuery query, CancellationToken cancellationToken)
    {
        return await cacheService.GetAsync<string>(query.Key, cancellationToken) ?? string.Empty;
    }
}