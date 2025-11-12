using BankingGateWay.Modules.CacheModule.Contract.Commands;
using BankingGateWay.Modules.CacheModule.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace BankingGateWay.Modules.CacheModule.Features.GetOrCreateCache;

internal sealed class GetOrCreateCacheCommandHandler :
    ICommandHandler<GetOrCreateCacheCommand, string>
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetOrCreateCacheCommandHandler> _logger;

    public GetOrCreateCacheCommandHandler(ILogger<GetOrCreateCacheCommandHandler> logger, ICacheService cacheService)
    {
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<string> Handle(GetOrCreateCacheCommand request, CancellationToken ct)
    {
        var existing = await _cacheService.GetAsync(request.Key, ct);
        if (!string.IsNullOrEmpty(existing))
            return existing;

        var value = await request.Factory(ct);

        if (!string.IsNullOrEmpty(value))
        {
            await _cacheService.SetAsync(
                request.Key,
                value, request.Ttl,
                //new DistributedCacheEntryOptions
                //{
                //    AbsoluteExpirationRelativeToNow = request.Ttl
                //},
                ct);
        }

        return value;
    }
}
