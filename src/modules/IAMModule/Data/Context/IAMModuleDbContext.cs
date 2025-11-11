
namespace IAMModule.Data.Context;

public class IAMModuleDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
    IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, ApplicationRoleClaim,
    IdentityUserToken<string>>
{
    public IAMModuleDbContext(DbContextOptions<IAMModuleDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        
    }
}