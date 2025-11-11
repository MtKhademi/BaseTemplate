using BankingGateWay.Modules.CacheModule.Contract.Commands;
using BankingGateWay.Modules.CacheModule.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace BankingGateWay.Modules.CacheModule.Features.GetOrCreateCache;

internal sealed class ClearCacheHandler : ICommandHandler<ClearCacheCommand, Unit>
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<ClearCacheHandler> _logger;

    public ClearCacheHandler(ILogger<ClearCacheHandler> logger, ICacheService cacheService)
    {
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<Unit> Handle(ClearCacheCommand request, CancellationToken ct)
    {
        await _cacheService.Clear(request.Key, ct);
        return Unit.Value;
    }
}
