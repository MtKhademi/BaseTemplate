using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ECommerceModule.Persistence.Configs;

internal sealed class CategoryEntityConfig
 : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.ToTable("Categories", ECommerceSchemas.Default);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.ParentId)
            .IsRequired(false);

        builder.HasIndex(x => x.Name)
            .IsUnique(false);


        builder.HasIndex(x => x.Name);

        builder.HasIndex(x => x.ParentId);
    }
}
