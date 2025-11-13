
namespace UserManagementModule.Data.Context;

public class UserManagementModuleDbContext : 
    IdentityDbContext<ApplicationUser, ApplicationRole, string,
    IdentityUserClaim<string>, 
    IdentityUserRole<string>, 
    IdentityUserLogin<string>, 
    ApplicationRoleClaim,
    IdentityUserToken<string>>
{
    public UserManagementModuleDbContext(DbContextOptions<UserManagementModuleDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}