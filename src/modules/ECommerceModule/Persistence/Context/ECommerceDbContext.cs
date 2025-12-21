namespace ECommerceModule.Persistence.Context;

internal class ECommerceDbContext : DbContext
{
    public DbSet<ProductEntity> Products => Set<ProductEntity>();
    public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
    public DbSet<CartEntity> Carts => Set<CartEntity>();
    public DbSet<CartItemEntity> CartItems => Set<CartItemEntity>();
    public DbSet<OrderEntity> Orders => Set<OrderEntity>();
    public DbSet<OrderItemEntity> OrderItems => Set<OrderItemEntity>();
    public DbSet<InventoryItemEntity> Inventory => Set<InventoryItemEntity>();
    public DbSet<PaymentEntity> Payments => Set<PaymentEntity>();

    public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options)
        : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ECommerceDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.CreatedAt))
                    .HasColumnType("datetime2");

                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.LastModified))
                    .HasColumnType("datetime2");
            }
        }
    }

}
