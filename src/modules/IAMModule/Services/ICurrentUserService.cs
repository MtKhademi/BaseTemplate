namespace IAMModule.Services;

public interface ICurrentUserService
{
      string? UserId { get; }
}

class CurrentUserService : ICurrentUserService
{
      public CurrentUserService(IHttpContextAccessor httpContextAccessor)
      {
            UserId = httpContextAccessor.HttpContext?
                  .User?
                  .Claims
                  .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?
                  .Value;
      }
      public string? UserId { get; }
}