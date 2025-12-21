using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ECommerceModule.Persistence.Configs;

internal sealed class InventoryItemEntityConfig
    : IEntityTypeConfiguration<InventoryItemEntity>
{
    public void Configure(EntityTypeBuilder<InventoryItemEntity> builder)
    {
        builder.ToTable("InventoryItems", ECommerceSchemas.Default);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductId)
            .IsRequired();

        builder.Property(x => x.AvailableQuantity)
            .IsRequired();

        builder.Property(x => x.ReservedQuantity)
            .IsRequired();

        builder.HasIndex(x => x.ProductId)
            .IsUnique();

        builder.HasOne<ProductEntity>()
            .WithOne()
            .HasForeignKey<InventoryItemEntity>(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
