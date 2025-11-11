using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.DDD;

namespace Common.Data;

public abstract class EFEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
    where TEntity : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(e => e.CreatedBy).HasMaxLength(100);
        builder.Property(e => e.LastModifiedBy).HasMaxLength(100);
    }
}
