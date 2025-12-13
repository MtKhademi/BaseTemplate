
using Microsoft.EntityFrameworkCore.Internal;

namespace IAMModule.Data.Context;

internal class IAMModuleDbContext :
    IdentityDbContext<ApplicationUser, ApplicationRole, string,
    IdentityUserClaim<string>,
    IdentityUserRole<string>,
    IdentityUserLogin<string>,
    ApplicationRoleClaim,
    IdentityUserToken<string>>
{
    public IAMModuleDbContext(DbContextOptions<IAMModuleDbContext> options) : base(options)
    {

    }

    internal DbSet<FeatureEntity> Features => Set<FeatureEntity>();
    internal DbSet<PermissionEntity> Permissions => Set<PermissionEntity>();
    internal DbSet<UserAppPermissionEntity> UserPermissions => Set<UserAppPermissionEntity>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(IAMModuleDbContext).Assembly);
    }
}