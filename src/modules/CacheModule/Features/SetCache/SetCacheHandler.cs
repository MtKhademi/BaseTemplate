using BankingGateWay.Modules.CacheModule.Contract.Commands;
using BankingGateWay.Modules.CacheModule.Services;

namespace CacheModule.Features.SetCache;

internal sealed class SetCacheHandler :
    ICommandHandler<SetCacheCommand, Unit>
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetOrCreateCacheCommandHandler> _logger;

    public SetCacheHandler(ILogger<GetOrCreateCacheCommandHandler> logger, ICacheService cacheService)
    {
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<Unit> Handle(SetCacheCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Setting cache for key: {Key} with TTL: {Ttl}", request.Key, request.Ttl);
        await _cacheService.SetAsync(
                request.Key,
                request.Value, request.Ttl,
                cancellationToken);


        return Unit.Value;
    }
}
