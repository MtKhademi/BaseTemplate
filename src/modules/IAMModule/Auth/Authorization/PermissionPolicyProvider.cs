namespace IAMModule.Auth.Authorization;


public sealed class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private const string Prefix = "Permission.";
    private readonly ConcurrentDictionary<string, AuthorizationPolicy> _cache = new();
    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (!policyName.StartsWith(Prefix))
            return Task.FromResult<AuthorizationPolicy?>(null);


        return Task.FromResult(
            _cache.GetOrAdd(policyName, name =>
            {
                return new AuthorizationPolicyBuilder()
                    .AddRequirements(new PermissionRequirement(name))
                    .RequireAuthenticatedUser()
                    .Build();
            }))!;
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        => Task.FromResult(
            new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build());

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        => Task.FromResult<AuthorizationPolicy?>(null);
}
