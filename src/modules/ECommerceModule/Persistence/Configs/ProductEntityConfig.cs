using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ECommerceModule.Persistence.Configs;

internal sealed class ProductEntityConfig
    : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.ToTable("Products", ECommerceSchemas.Default);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Price)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.CategoryId)
            .IsRequired();

        builder.HasIndex(x => x.CategoryId);

        builder.HasOne<CategoryEntity>()
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasIndex(x => x.IsActive);

        builder.HasIndex(x => x.CategoryId);

        builder.HasIndex(x => new { x.CategoryId, x.IsActive });

        builder.HasIndex(x => x.Price);
    }
}