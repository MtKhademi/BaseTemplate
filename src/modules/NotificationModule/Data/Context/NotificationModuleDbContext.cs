namespace NotificationModule.Data.Context;

public class NotificationModuleDbContext : DbContext
{
    public NotificationModuleDbContext(DbContextOptions<NotificationModuleDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(NotificationModuleDbContext).Assembly);
    }
}