using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ECommerceModule.Persistence.Configs;

internal sealed class CartEntityConfig
    : IEntityTypeConfiguration<CartEntity>
{
    public void Configure(EntityTypeBuilder<CartEntity> builder)
    {
        builder.ToTable("Carts", ECommerceSchemas.Default);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.HasIndex(x => x.UserId)
            .IsUnique();

        builder.HasMany<CartItemEntity>()
            .WithOne()
            .HasForeignKey(x => x.CartId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
