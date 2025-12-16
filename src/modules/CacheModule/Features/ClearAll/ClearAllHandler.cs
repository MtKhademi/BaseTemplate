namespace IAMModule.User.Features.ClearAll;

internal class ClearAllHandler(ICacheService cacheService)
    : ICommandHandler<CacheClearAllCommand, bool>
{
    public async Task<bool> Handle(CacheClearAllCommand command, CancellationToken cancellationToken)
    {
        await cacheService.RemoveAllAsync(cancellationToken);
        return true;
    }
}