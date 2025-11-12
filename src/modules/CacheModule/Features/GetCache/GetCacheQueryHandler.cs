using BankingGateWay.Modules.CacheModule.Contract.Queries;
using BankingGateWay.Modules.CacheModule.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace BankingGateWay.Modules.CacheModule.Features.GetCache;

internal sealed class GetCacheQueryHandler : IQueryHandler<GetCacheQuery, string?>
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetCacheQueryHandler> _logger;

    public GetCacheQueryHandler(ILogger<GetCacheQueryHandler> logger, ICacheService cacheService)
    {
        _logger = logger;
        _cacheService = cacheService;
    }

    public Task<string?> Handle(GetCacheQuery request, CancellationToken ct) =>
        _cacheService.GetAsync(request.Key, ct);
}
