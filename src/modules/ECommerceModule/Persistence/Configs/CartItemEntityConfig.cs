using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ECommerceModule.Persistence.Configs;

internal sealed class CartItemEntityConfig
    : IEntityTypeConfiguration<CartItemEntity>
{
    public void Configure(EntityTypeBuilder<CartItemEntity> builder)
    {
        builder.ToTable("CartItems", ECommerceSchemas.Default);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.HasIndex(x => new { x.CartId, x.ProductId })
            .IsUnique();
    }
}
