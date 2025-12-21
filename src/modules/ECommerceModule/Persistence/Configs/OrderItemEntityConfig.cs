using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ECommerceModule.Persistence.Configs;

internal sealed class OrderItemEntityConfig
    : IEntityTypeConfiguration<OrderItemEntity>
{
    public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
    {
        builder.ToTable("OrderItems", ECommerceSchemas.Default);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.UnitPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        // Configure the relationship from the child side as well
        builder.HasOne(x => x.Order)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.OrderId);
    }
}
