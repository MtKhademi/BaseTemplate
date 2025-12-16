namespace IAMModule.User.Features.Set;

internal class SetHandler(ICacheService cacheService)
    : ICommandHandler<CacheSetCommand<string>, bool>
{
    public async Task<bool> Handle(CacheSetCommand<string> command, CancellationToken cancellationToken)
    {
        await cacheService.SetAsync<string>(
            command.Key, 
            command.Value,
            absoluteExpiration: command.Expiration, cancellationToken);
        return true;
    }
}